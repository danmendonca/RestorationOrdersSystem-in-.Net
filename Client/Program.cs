using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Windows.Forms;

static class Program {
  [STAThread]
  static void Main() {
    IDictionary props = new Hashtable();
    props["port"] = 0;  // let the system choose a free port
    BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
    serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
    BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
    TcpChannel chan = new TcpChannel(props, clientProvider, serverProvider);  // instantiate the channel
    ChannelServices.RegisterChannel(chan, false);                             // register the channel

    ChannelDataStore data = (ChannelDataStore)chan.ChannelData;
    int port = new Uri(data.ChannelUris[0]).Port;                            // get the port

    RemotingConfiguration.Configure("Client.exe.config", false);             // register the server objects
    RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemMessage), "Message", WellKnownObjectMode.Singleton);  // register my remote object for service

    Window myWindow = new Window(port);
    RemMessage r = (RemMessage)RemotingServices.Connect(typeof(RemMessage), "tcp://localhost:" + port.ToString() + "/Message");    // connect to the registered my remote object here
    r.PutMyForm(myWindow);                  // communicate the window reference

    Application.EnableVisualStyles();
    Application.Run(myWindow);
  }
}

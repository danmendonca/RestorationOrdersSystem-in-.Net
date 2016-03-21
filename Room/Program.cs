using RequestLibrary;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.Remoting.Channels;

namespace Room
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            IChannel[] myIChannelArray = ChannelServices.RegisteredChannels;
            for (int i = 0; i < myIChannelArray.Length; i++)
            {
                Console.WriteLine("Name of Channel: {0}", myIChannelArray[i].ChannelName);
                Console.WriteLine("Priority of Channel: {0}",
                   +myIChannelArray[i].ChannelPriority);
            }



            #region mistery
            IDictionary props = new Hashtable();
            props["port"] = 0;
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            TcpChannel channel = new TcpChannel(props, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(channel, false);                             // register the channel

            ChannelDataStore data = (ChannelDataStore)channel.ChannelData;
            int port = new Uri(data.ChannelUris[0]).Port;
            
            #endregion

            RemotingConfiguration.Configure("Room.exe.config", false); // register the server objects
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemMessage), "Message", WellKnownObjectMode.Singleton);  // register my remote object for service

            Window myWindow = new Window(port);
            RemMessage r = (RemMessage)RemotingServices.Connect(typeof(RemMessage), 
                "tcp://localhost:" + port.ToString() + "/Message");    // connect to the registered my remote object here
            r.PutMyForm(myWindow);                  // communicate the window reference

            Application.EnableVisualStyles();
            Application.Run(myWindow);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(myWindow);



        }
    }
}

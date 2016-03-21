using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Windows.Forms;

public partial class Window : Form
{
    int port;
    Guid guid;
    ISingleServer server;
    public List<Product> ps { get; private set; }
    short nrTables;

    public Window(int myPort)
    {
        port = myPort;
        guid = Guid.NewGuid();
        InitializeComponent();
    }

    private void Window_Load(object sender, EventArgs e)
    {
        server = (ISingleServer)R.New(typeof(ISingleServer));  // get reference to the singleton remote object
    }

    private void buttonConnect_Click(object sender, EventArgs e)
    {
        server.ClientAddress(guid, "tcp://localhost:" + port.ToString() + "/Message");
    }

    private void buttonMkReq_Click(object sender, EventArgs e)
    {
        string dsc = textBoxDescription.Text;
        short tblNr = (short)comboBoxTable.SelectedIndex;
        short pIndex = (short)comboBoxProduct.SelectedIndex;
        short qtt = (short)spinnerQuantity.Value;
        server.MakeRequest((short) comboBoxTable.SelectedIndex, (short) comboBoxProduct.SelectedIndex, (short)spinnerQuantity.Value, textBoxDescription.Text);
       // short s = 2;
        //server.MakeRequest(s, ps[2], s, "");
        textBoxReadyReq.Text += ("Calling Server ..." + Environment.NewLine);
        server.SomeCall(guid);
        //ps= server.GetProducts();
    }

    public void AddMessage(string message)
    {
        if (InvokeRequired)                                               // I'm not in UI thread
            BeginInvoke((MethodInvoker)delegate { AddMessage(message); });  // Invoke using an anonymous delegate
        else
            textBoxReadyReq.Text += (message + Environment.NewLine);
    }

    public void SetListProducts(List<Product> lp)
    {
        if (InvokeRequired)                                               // I'm not in UI thread
            BeginInvoke((MethodInvoker)delegate { SetListProducts(lp); });  // Invoke using an anonymous delegate
        else
        {
            ps = lp;
            comboBoxProduct.Items.Clear();
            foreach (Product p in lp) comboBoxProduct.Items.Add(p);
        }
    }
    public void SetNrTables(short nrTables)
    {

        if (InvokeRequired)                                               // I'm not in UI thread
            BeginInvoke((MethodInvoker)delegate { SetNrTables(nrTables); });  // Invoke using an anonymous delegate
        else
        {
            this.nrTables = nrTables;
            comboBoxTable.Items.Clear();
            for (short i = 0; i < nrTables; i++) comboBoxTable.Items.Add($"Mesa {i.ToString(),2}");
        }
        //throw new NotImplementedException();
    }
}

class R
{
    private static IDictionary wellKnownTypes;

    public static object New(Type type)
    {
        if (wellKnownTypes == null)
            InitTypeCache();
        WellKnownClientTypeEntry entry = (WellKnownClientTypeEntry)wellKnownTypes[type];
        if (entry == null)
            throw new RemotingException("Type not found!");
        return Activator.GetObject(type, entry.ObjectUrl);
    }

    public static void InitTypeCache()
    {
        Hashtable types = new Hashtable();
        foreach (WellKnownClientTypeEntry entry in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
        {
            if (entry.ObjectType == null)
                throw new RemotingException("A configured type could not be found!");
            types.Add(entry.ObjectType, entry);
        }
        wellKnownTypes = types;
    }
}

public class RemMessage : MarshalByRefObject, IRoomService
{
    private Window win;

    public void SetNrTables(short nrTbls)
    {
        win.SetNrTables(nrTbls);
    }

    public void SetProducts(List<Product> lp)
    {
        win.SetListProducts(lp);
    }

    public override object InitializeLifetimeService()
    {
        return null;
    }

    public void PutMyForm(Window form)
    {
        win = form;
    }

    public void requestNotification(RequestLine rl)
    {
        throw new NotImplementedException();
    }

    public void SomeMessage(string message)
    {
        win.AddMessage(message);
    }
}

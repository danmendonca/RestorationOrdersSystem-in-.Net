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
    private List<Product> ps { get; private set; }
    private List<RequestLine> toDeliver = new List<RequestLine>(); 
    ushort nrTables;

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
        server.ClientAddress("tcp://localhost:" + port.ToString() + "/Message");
    }

    private void buttonMkReq_Click(object sender, EventArgs e)
    {
        string dsc = textBoxDescription.Text;
        ushort tblNr = (ushort)comboBoxTable.SelectedIndex;
        ushort pIndex = (ushort)comboBoxProduct.SelectedIndex;
        ushort qtt = (ushort)spinnerQuantity.Value;
        server.MakeRequest((ushort)comboBoxTable.SelectedIndex, (ushort)comboBoxProduct.SelectedIndex, (ushort)spinnerQuantity.Value, textBoxDescription.Text);
        // ushort s = 2;
        //server.MakeRequest(s, ps[2], s, "");
        textBoxReadyReq.Text += ("Calling Server ..." + Environment.NewLine);

        //ps= server.GetProducts();
    }

    public void AddMessage(string message)
    {
        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { AddMessage(message); });
        else
            textBoxReadyReq.Text += (message + Environment.NewLine);
    }

    public void SetListProducts(List<Product> lp)
    {
        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { SetListProducts(lp); });
        else
        {
            ps = lp;
            comboBoxProduct.Items.Clear();
            foreach (Product p in lp) comboBoxProduct.Items.Add(p);
        }
    }
    public void SetNrTables(ushort nrTables)
    {

        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { SetNrTables(nrTables); });
        else
        {
            this.nrTables = nrTables;
            comboBoxTable.Items.Clear();
            for (ushort i = 0; i < nrTables; i++) comboBoxTable.Items.Add($"Mesa {i.ToString(),2}");
        }
    }

    public void AddRequestState(RequestLine rl)
    {
        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { AddRequestState(rl); });
        else
            textBoxReadyReq.Text += (rl.ToString() + Environment.NewLine);
    }

    public void deliverRequest(RequestLine rl)
    {
        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { AddMessage(message); });
        else
            textBoxReadyReq.Text += (message + Environment.NewLine);
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

    public void SetNrTables(ushort nrTbls)
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

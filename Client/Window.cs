using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;

public partial class Window : Form
{
    Guid guid;
    ISingleServer registerServer;
    public RoomProxy roomProxy;
    private List<Product> ps { get; set; }
    private List<RequestLine> toDeliver = new List<RequestLine>();
    ushort nrTables;


    public override object InitializeLifetimeService()
    {
        return null;
    }


    public Window()
    {
        InitializeComponent();
        guid = Guid.NewGuid();

        //create proxy to registerServer and subscribe its events
        roomProxy = new RoomProxy();
        roomProxy.rREvent += new RequestReadyDelegate(RoomProxy_rREvent);
        roomProxy.rDEvent += new RequestDeliveredDelegate(RoomProxy_rDEvent);

        //reference to registerServer
        registerServer = (ISingleServer)RemoteNew.New(typeof(ISingleServer));

        //getting info from registerServer and update UI
        ps = registerServer.GetProducts();
        nrTables = registerServer.GetNrTables();
        SetListProducts(ps);
        SetNrTables(nrTables);

        //subscribe proxy to registerServer events
        RequestReadyDelegate rrd = new RequestReadyDelegate(roomProxy.RepeaterRReady);
        registerServer.requestReadyEvent += rrd;
    }

    #region RegisterDirectCalls



    private void buttonMkReq_Click(object sender, EventArgs e)
    {
        //TODO try catch
        string dsc = textBoxDescription.Text;
        ushort tblNr = (ushort)comboBoxTable.SelectedIndex;
        ushort pIndex = (ushort)comboBoxProduct.SelectedIndex;
        ushort qtt = (ushort)spinnerQuantity.Value;
        RequestLine rl = new RequestLine(0, pIndex, qtt, tblNr, dsc);
        registerServer.MakeRequest(rl);
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


    public void deliverRequest(RequestLine rl)
    {
        throw new NotImplementedException();
    }


    private void btnAskBill_Click(object sender, EventArgs e)
    {
        try
        {
            registerServer.RequestBill((ushort)comboBoxTable.SelectedIndex);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Client:btnAskBill_Click ===> " + ex.ToString());
        }
    }



    #endregion



    #region Handlers



    private void RoomProxy_rDEvent(RequestLine rl)
    {
        throw new NotImplementedException();
    }


    private void RoomProxy_rREvent(RequestLine rl)
    {
        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { RoomProxy_rREvent(rl); });
        else
            textBoxRequests.Text += (rl.ToString() + Environment.NewLine);
    }



    #endregion



}

class RemoteNew
{
    private static Hashtable types = null;

    private static void InitTypeTable()
    {
        types = new Hashtable();
        foreach (WellKnownClientTypeEntry entry in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
            types.Add(entry.ObjectType, entry);
    }

    public static object New(Type type)
    {
        if (types == null)
            InitTypeTable();
        WellKnownClientTypeEntry entry = (WellKnownClientTypeEntry)types[type];
        if (entry == null)
            throw new RemotingException("Type not found!");
        return RemotingServices.Connect(type, entry.ObjectUrl);
    }
}
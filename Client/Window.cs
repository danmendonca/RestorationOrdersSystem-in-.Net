using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;

public partial class Window : Form
{
    private static String PTWaiting = "Fila";
    private static String PTInProgress = "Preparação";
    private static String PTReady = "Pronto";
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
        string dsc = textBoxDescription.Text;
        ushort tblNr = (ushort)comboBoxTable.SelectedIndex;
        ushort pIndex = (ushort)comboBoxProduct.SelectedIndex;
        ushort qtt = (ushort)spinnerQuantity.Value;
        RequestLine rl = new RequestLine(0, pIndex, qtt, tblNr, dsc);
        try {
            registerServer.MakeRequest(rl);
            textBoxDescription.Text = "";
            spinnerQuantity.Value = 1;
        }
        catch(Exception ex)
        {
            Console.WriteLine("RoomService:buttonMkReq_Click ==> cannot connect with registerServer\n " + ex.ToString() );
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
            Console.WriteLine("Client:btnAskBill_Click ===> \n" + ex.ToString());
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
        {
            ListViewItem lvi = new ListViewItem(rl.RequestNr.ToString());
            lvi.SubItems.Add(rl.TableNr.ToString());
            lvi.SubItems.Add(ps[rl.Prod].ToString());
            lvi.SubItems.Add(rl.Qtt.ToString());
            if (rl.RState == RequestState.Waiting) lvi.SubItems.Add(PTWaiting);
            else if (rl.RState == RequestState.InProgress) lvi.SubItems.Add(PTInProgress);
            else lvi.SubItems.Add(PTReady);
            listViewRequests.Items.Add(lvi);
        }
      //  else
        //    textBoxRequests.Text += (rl.ToString() + Environment.NewLine);
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
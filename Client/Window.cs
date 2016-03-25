using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;

public partial class Window : Form
{
    #region Statics
    private static String PTWaiting = "Fila";
    private static String PTInProgress = "Preparação";
    private static String PTReady = "Pronto";
    private static int LISTVIEW_REQ_NR_INDEX = 0;
    private static int LISTVIEW_TABLE_INDEX = 1;
    private static int LISTVIEW_PRODUCT_INDEX = 2;
    private static int LISTVIEW_QUANTITY_INDEX = 3;
    private static int LISTVIEW_STATE_INDEX = 4;
    #endregion



    #region Attributes
    Guid guid;
    ISingleServer registerServer;
    public RoomProxy roomProxy;
    private List<Product> ps { get; set; }
    private List<RequestLine> toDeliver = new List<RequestLine>();
    ushort nrTables;

    #endregion


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
        AskListProducts(ps);
        AskNrTables(nrTables);

        //subscribe proxy to registerServer events
        RequestReadyDelegate rrd = new RequestReadyDelegate(roomProxy.RepeaterRReady);
        registerServer.requestReadyEvent += rrd;
    }


    private void RemoveReqFromLView(RequestLine rl)
    {
        if (InvokeRequired) BeginInvoke((MethodInvoker)delegate { RemoveReqFromLView(rl); });
        else
        {
            for (int i = 0; i < listViewRequests.Items.Count; i++)
            {
                ushort reqNr = Convert.ToUInt16(listViewRequests.Items[i].SubItems[LISTVIEW_REQ_NR_INDEX].Text);
                if (rl.RequestNr != reqNr) continue;
                listViewRequests.Items[i].Remove();
            }
        }
    }

    private void ChangeReqStateInLView(RequestLine rl)
    {
        if (InvokeRequired) BeginInvoke((MethodInvoker)delegate { ChangeReqStateInLView(rl); });
        else
        {
            for (int i = 0; i < listViewRequests.Items.Count; i++)
            {
                ushort reqNr = Convert.ToUInt16(listViewRequests.Items[i].SubItems[LISTVIEW_REQ_NR_INDEX].Text);
                if (rl.RequestNr != reqNr) continue;

                listViewRequests.Items[i].SubItems[LISTVIEW_STATE_INDEX].Text =
                    (rl.RState == RequestState.InProgress) ? PTInProgress : PTReady;
            }
        }
    }

    private void AddReqToLView(RequestLine rl)
    {
        if (InvokeRequired) BeginInvoke((MethodInvoker)delegate { AddReqToLView(rl); });
        else
        {
            ListViewItem lvi = new ListViewItem(rl.RequestNr.ToString());
            lvi.SubItems.Add(rl.TableNr.ToString());
            lvi.SubItems.Add(ps[rl.Prod].ToString());
            lvi.SubItems.Add(rl.Qtt.ToString());
            lvi.SubItems.Add(PTWaiting);

            listViewRequests.Items.Add(lvi);
        }
    }



    #region RegisterDirectCalls
    private void buttonMkReq_Click(object sender, EventArgs e)
    {
        string dsc = textBoxDescription.Text;
        ushort tblNr = (ushort)comboBoxTable.SelectedIndex;
        ushort pIndex = (ushort)comboBoxProduct.SelectedIndex;
        ushort qtt = (ushort)spinnerQuantity.Value;
        RequestLine rl = new RequestLine(0, pIndex, qtt, tblNr, dsc);
        try
        {
            registerServer.MakeRequest(rl);
            textBoxDescription.Text = "";
            spinnerQuantity.Value = 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine("RoomService:buttonMkReq_Click ==> cannot connect with registerServer\n " + ex.ToString());
        }
    }


    private void btnReqDelivered_Click(object sender, EventArgs e)
    {

        if (listViewRequests.SelectedItems.Count < 1)
            return;

        try
        {
            ushort nrRequestDelivered = Convert.ToUInt16(listViewRequests.SelectedItems[0].SubItems[0].Text);
            int tblNr = Convert.ToInt32(listViewRequests.SelectedItems[0].SubItems[1].Text);
            registerServer.SetRequestDelivered(tblNr, nrRequestDelivered);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception at RoomService:btnDelivered_Click \n" + ex.ToString());
        }
    }


    public void AskNrTables(ushort nrTables)
    {
        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { AskNrTables(nrTables); });
        else
        {
            this.nrTables = nrTables;
            comboBoxTable.Items.Clear();
            for (ushort i = 0; i < nrTables; i++) comboBoxTable.Items.Add($"Mesa {i.ToString(),2}");
        }
    }


    public void AskListProducts(List<Product> lp)
    {
        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { AskListProducts(lp); });
        else
        {
            ps = lp;
            comboBoxProduct.Items.Clear();
            foreach (Product p in lp) comboBoxProduct.Items.Add(p);
        }
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

        switch (rl.RState)
        {
            case RequestState.Waiting:
                AddReqToLView(rl);
                break;
            case RequestState.Delivered:
                RemoveReqFromLView(rl);
                break;
            default:
                ChangeReqStateInLView(rl);
                break;
        }

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
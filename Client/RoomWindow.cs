using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;

public partial class RoomWindow : Form
{
    #region Statics
    private static String PTInProgress = "Preparação";
    private static String PTReady = "Pronto";
    private static String PTDelivered = "Entregue";
    private static String PTWaiting = "Em espera";
    private static int LISTVIEW_REQ_NR_INDEX = 0;
    private static int LISTVIEW_TABLE_INDEX = 1;
    private static int LISTVIEW_PRODUCT_INDEX = 2;
    private static int LISTVIEW_QUANTITY_INDEX = 3;
    private static int LISTVIEW_STATE_INDEX = 4;
    #endregion



    #region Attributes
    Guid guid;
    ISingleServer registerServer;
    private RoomProxy _roomProxy;
    private List<Product> _ps { get; set; }
    private ushort _nrTables = 0;
    private List<RequestLine> _requestLines;

    #endregion



    #region initialization
    public override object InitializeLifetimeService()
    {
        return null;
    }


    public RoomWindow()
    {
        InitializeComponent();
        guid = Guid.NewGuid();
        _requestLines = new List<RequestLine>();
        //create proxy to registerServer and subscribe its events
        _roomProxy = new RoomProxy();
        _roomProxy.rREvent += new RequestReadyDelegate(RoomProxy_rREvent);
        _roomProxy.rDEvent += new RequestDeliveredDelegate(RoomProxy_rDEvent);

        //reference to registerServer
        registerServer = (ISingleServer)RemoteNew.New(typeof(ISingleServer));

        //getting info from registerServer and update UI
        _ps = registerServer.GetProducts();
        _nrTables = registerServer.GetNrTables();
        ChangeProductListUI();
        ChangeTableListUI();

        //subscribe proxy to registerServer events
        RequestReadyDelegate rrd = new RequestReadyDelegate(_roomProxy.RepeaterRReady);
        registerServer.requestReadyEvent += rrd;

        
        _requestLines = _requestLines.Concat(registerServer.GetActiveRequests(PreparationRoomID.Bar)).ToList();
        _requestLines = _requestLines.Concat(registerServer.GetActiveRequests(PreparationRoomID.Kitchen)).ToList();
        RefreshListViewRequests();

    }
    #endregion


    #region updateGUI
    public void ChangeTableListUI()
    {
        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { ChangeTableListUI(); });
        else
        {
            comboBoxTable.Items.Clear();
            for (ushort i = 0; i < _nrTables; i++) comboBoxTable.Items.Add($"Mesa {i.ToString(),2}");
        }
    }


    public void ChangeProductListUI()
    {
        if (InvokeRequired)
            BeginInvoke((MethodInvoker)delegate { ChangeProductListUI(); });
        else
        {
            comboBoxProduct.Items.Clear();
            foreach (Product p in _ps) comboBoxProduct.Items.Add(p);
        }
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


    //private void ChangeReqStateInLView(RequestLine rl)
    //{
    //    if (InvokeRequired) BeginInvoke((MethodInvoker)delegate { ChangeReqStateInLView(rl); });
    //    else
    //    {
    //        for (int i = 0; i < listViewRequests.Items.Count; i++)
    //        {
    //            ushort reqNr = Convert.ToUInt16(listViewRequests.Items[i].SubItems[LISTVIEW_REQ_NR_INDEX].Text);
    //            if (rl.RequestNr != reqNr) continue;
    //            string state = StateToString(rl);
    //            //Console.WriteLine($"adding requestLine {i,2} state {state}");
    //            listViewRequests.Items[i].SubItems[LISTVIEW_STATE_INDEX].Text = state;
    //            break;
    //        }
    //    }
    //}

        /*
        */
    private void AddRequest(RequestLine rl)
    {
        _requestLines.Add(rl);
    }


    private void RemoveRequest(RequestLine rl)
    {
        _requestLines.Remove(rl);
        RefreshListViewRequests();
    }


    private void RefreshListViewRequests()
    {
        ClearRequestLinesView();
        foreach (var requestLine in _requestLines) AddOneReqToLView(requestLine);
    }


    private void ClearRequestLinesView()
    {
        if (InvokeRequired) BeginInvoke((MethodInvoker)delegate { ClearRequestLinesView(); });
        else
        {
            listViewRequests.Items.Clear();
        }
    }


    private void AddOneReqToLView(RequestLine requestLine)
    {
        if (InvokeRequired) BeginInvoke((MethodInvoker)delegate { AddOneReqToLView(requestLine); });
        else
        {
            ListViewItem lvi = new ListViewItem(requestLine.RequestNr.ToString());
            lvi.SubItems.Add(requestLine.TableNr.ToString());
            lvi.SubItems.Add(_ps[requestLine.Prod].ToString());
            lvi.SubItems.Add(requestLine.Qtt.ToString());
            string state = StateToString(requestLine);
            lvi.SubItems.Add(state);
            listViewRequests.Items.Add(lvi);
            Console.WriteLine($"added to reqView: {requestLine.ToString()}");
        }
    }
    #endregion



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



    //TODO not needed?
    private void RoomProxy_rDEvent(RequestLine rl)
    {
        throw new NotImplementedException();
    }

    private void RoomProxy_rREvent(RequestLine rl)
    {

        switch (rl.RState)
        {
            case RequestState.Waiting:
                Console.WriteLine($"Adding new RequestLine.");
                AddRequest(rl);
                //TODO ADD ONE LINE TO THE VIEW OR REFRESH THE WHOLE VIEW?
                RefreshListViewRequests();
                break;
            case RequestState.Delivered:
                Console.WriteLine($"Removing new RequestLine.");
                RemoveRequest(rl);
                //TODO REMOVE ONE LINE OF THE VIEW OR REFRESH THE WHOLE VIEW?
                RefreshListViewRequests();
                //RemoveReqFromLView(requestLine);
                break;
            default:
                Console.WriteLine($"Updating new RequestLine.");
                //TODO UPDATE ONE LINE OF THE VIEW OR REFRESH THE WHOLE VIEW?
                //ChangeReqStateInLView(requestLine);
                RefreshListViewRequests();
                break;
        }

    }
    #endregion



    #region helperMethods
    private static string StateToString(RequestLine requestLine)
    {
        switch (requestLine.RState)
        {
            case RequestState.InProgress:
                return PTInProgress;
            case RequestState.Delivered:
                return PTDelivered;
            case RequestState.Ready:
                return PTReady;
            default:
                return PTWaiting;
        }
    }
    #endregion
}

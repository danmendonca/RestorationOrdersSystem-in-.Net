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
    private static String ENInProgress = "InProgress";
    private static String ENReady = "Ready";
    private static String ENDelivered = "Delivered";
    private static String ENWaiting = "Waiting";
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
        _roomProxy.rREvent += new RequestDelegate(RoomProxy_rREvent);
        //_roomProxy.rDEvent += new RequestDeliveredDelegate(RoomProxy_rDEvent);

        //reference to registerServer
        registerServer = (ISingleServer)RemoteNew.New(typeof(ISingleServer));

        //getting info from registerServer and update UI
        _ps = registerServer.GetProducts();
        _nrTables = registerServer.GetNrTables();
        ChangeProductListUI();
        ChangeTableListUI();

        //subscribe proxy to registerServer events
        RequestDelegate rrd = new RequestDelegate(_roomProxy.RepeaterRReady);
        registerServer.RequestEvent += rrd;

        _requestLines = _requestLines.Concat(registerServer.GetNonDeliveredRequests()).ToList();
        RefreshListViewRequests();

        comboBoxProduct.SelectedIndex = 0;
        comboBoxTable.SelectedIndex = 0;
        spinnerQuantity.Minimum = 1;
        spinnerQuantity.Value = 1;
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
            for (ushort i = 0; i < _nrTables; i++) comboBoxTable.Items.Add($"Table {i.ToString(),2}");
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
        }
    }

    private void RefreshAfterRequest()
    {
        if (InvokeRequired) BeginInvoke((MethodInvoker)delegate { RefreshAfterRequest(); });
        else
        {
            textBoxDescription.Text = "";
            spinnerQuantity.Value = 1;
        }
    }
    #endregion



    #region Button Handlers
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
            RefreshAfterRequest();
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
            int requestLineIndex = listViewRequests.SelectedIndices[0];
            RequestLine rl = _requestLines.ElementAt(requestLineIndex);

            if (rl.RState != RequestState.Ready)
                return;

            ushort requestNr = rl.RequestNr;
            int tblNr = rl.TableNr;
            registerServer.SetRequestDelivered(tblNr, requestNr);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception at RoomService:btnReqDelivered_Click \n" + ex.ToString());
        }
    }


    private void btnTableConsult_Click(object sender, EventArgs e)
    {
        try
        {
            if(!registerServer.ConsultTable((ushort)comboBoxTable.SelectedIndex))
                MessageBox.Show("This table has no requests to pay..", "Table Consult",
    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Client:btnAskBill_Click ===> \n" + ex.ToString());
        }
    }
    #endregion



    #region Event Handlers
    private void RoomProxy_rREvent(RequestLine rl)
    {

        switch (rl.RState)
        {
            case RequestState.Waiting:
                AddRequest(rl);
                RefreshListViewRequests();
                break;
            case RequestState.Delivered:
                RemoveRequest(rl);
                RefreshListViewRequests();
                break;
            default:
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
                return ENInProgress;
            case RequestState.Delivered:
                return ENDelivered;
            case RequestState.Ready:
                return ENReady;
            default:
                return ENWaiting;
        }
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegisterGUI
{
    public partial class RegisterGUI : Form
    {
        #region Statics
        private static int LISTVIEW_PRODUCT_INDEX = 0;
        private static int LISTVIEW_TOTAL_PRICE_INDEX = 3;
        private static int LISTVIEW_UNIT_PRICE_INDEX = 2;
        private static int LISTVIEW_QUANTITY_INDEX = 1;
        #endregion



        #region Attributes
        ISingleServer registerServer;
        RegisterGuiProxy rgProxy;
        #endregion



        #region Properties
        ushort NrOfTables { get; set; }
        List<Product> Products { get; set; }
        #endregion



        #region ClassMethods
        public RegisterGUI()
        {
            InitializeComponent();

            //subscribe proxy to server
            registerServer = (ISingleServer)RemoteNew.New(typeof(ISingleServer));

            //subscribe proxy event
            rgProxy = new RegisterGuiProxy();

            RequestDelegate selfRD = new RequestDelegate(RequestHandler);
            rgProxy.RequestEvent += selfRD;
            TablePaidDelegate selfTPD = new TablePaidDelegate(TablePaidHandler);
            rgProxy.TablePaidEvent += selfTPD;

            TablePaidDelegate proxyTPD = new TablePaidDelegate(rgProxy.TablePaidNotifier);
            registerServer.TablePaidEvent += proxyTPD;
            RequestDelegate proxyRD = new RequestDelegate(rgProxy.RequestsUpdateNotifier);
            registerServer.RequestEvent += proxyRD;

            NrOfTables = registerServer.GetNrTables();
            Products = registerServer.GetProducts();

            SetNrTablesComboBox();
        }
        #endregion



        #region RemoteDirectCalls
        List<RequestLine> GetTableRequests()
        {
            if (comboBoxTable.SelectedIndex >= 0)
                return registerServer.GetTableRLs(comboBoxTable.SelectedIndex);

            return null;
        }
        #endregion



        #region ViewUpdates
        private void SetNrTablesComboBox()
        {

            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { SetNrTablesComboBox(); });
            else
            {
                comboBoxTable.Items.Clear();
                for (ushort i = 0; i < NrOfTables; i++) comboBoxTable.Items.Add($"Table {i.ToString(),2}");
            }
        }


        private void ChangeTableRequestsView(List<RequestLine> rls)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { ChangeTableRequestsView(rls); });
            else
            {
                listViewTableReqs.Items.Clear();
                foreach (var requestLine in rls)
                {
                    Product prod = Products.ElementAt((int)requestLine.Prod);
                    float totalPrice = (float)(requestLine.Qtt * prod.Price);
                    ListViewItem lvi = new ListViewItem(prod.Name);
                    lvi.SubItems.Add(requestLine.Qtt.ToString());
                    lvi.SubItems.Add(prod.Price.ToString());
                    lvi.SubItems.Add(totalPrice.ToString());
                    listViewTableReqs.Items.Add(lvi);
                }
            }
        }
        #endregion



        #region Handlers
        private void btnSetTablePaid_Click(object sender, EventArgs e)
        {
            int selectedTable = comboBoxTable.SelectedIndex;
            registerServer.SetTablePaid(selectedTable);
        }

        private void comboBoxTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<RequestLine> rls = GetTableRequests();
            if (rls != null) ChangeTableRequestsView(rls);
        }

        private void RequestHandler(RequestLine rl)
        {
            if (rl.TableNr != comboBoxTable.SelectedIndex) return;

            List<RequestLine> rls = GetTableRequests();
            if (rls != null) ChangeTableRequestsView(rls);
        }

        private void TablePaidHandler(int tableNr)
        {
            if (tableNr != comboBoxTable.SelectedIndex) return;

            List<RequestLine> rls = GetTableRequests();
            if (rls != null) ChangeTableRequestsView(rls);
        }
        #endregion



        #region Overrides
        public override object InitializeLifetimeService()
        {
            return null;
        }
        #endregion
    }
}

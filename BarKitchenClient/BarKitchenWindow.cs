using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace BarKitchenClient
{
    public partial class BarKitchenWindow : Form
    {
        #region Attributes

        // Remote server interface
        ISingleServer RemoteServer;

        // Restaurant product list
        List<Product> RestaurantProducts;

        // Active request list
        List<RequestLine> ActiveRequestList;

        // BarKitchenEvent Repeater
        BarKitchenEventRepeater BarKitchenRepeater;

        // Delegate for loading listview
        private delegate void LoadListViewDelegate();

        // Delegate for adding row to listview
        private delegate ListViewItem AddListViewRowDelegate(ListViewItem lvItem);

        // Delegate for updating listview row state field 
        private delegate void UpdateRowStateDelegate(RequestLine rl);

        // Delegate for removing listview row
        private delegate void RemoveListViewRowDelegate(RequestLine rl);

        // List View Columns
        enum LvColumn { Request, Table, Product, Quantity, State, Service}

        // Service Type (Bar or Kitchen)
        private PreparationRoomID ServiceType { get; set; }

        #endregion

        public BarKitchenWindow()
        {
            RestaurantProducts = new List<Product>();
            ActiveRequestList = new List<RequestLine>();

            try
            {
                // Client configuration file
                RemotingConfiguration.Configure("BarKitchenClient.exe.config", false);

                // Remote Server object initialization
                RemoteServer = (ISingleServer)RemoteNew.New(typeof(ISingleServer));

                // Get Restaurant products
                RestaurantProducts = RemoteServer.GetProducts();

                // Windows Forms initialization
                InitializeComponent();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
                MessageBox.Show("Unable to connect remote server", "Remote Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        private void BarKitchenWindow_Load(object sender, EventArgs e)
        {
            
        }

        private void UpdateRequestState_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (listView1.SelectedItems.Count < 1) return;

            String tableNr = listView1.SelectedItems[0].SubItems[(int)LvColumn.Table].Text;
            String requestNr = listView1.SelectedItems[0].SubItems[(int)LvColumn.Request].Text;
           
            RemoteServer.UpdateRequestLineState(UInt16.Parse(tableNr), UInt16.Parse(requestNr));
        }

        private void startAppBtn_Click(object sender, EventArgs e)
        {
            startAppBtn.Enabled = false;
            stComboBox.Enabled = false;

            ServiceType = (PreparationRoomID)stComboBox.SelectedItem;
        
            ActiveRequestList = RemoteServer.GetActiveRequests(ServiceType);

            //Subscribe remote server events
            BarKitchenRepeater = new BarKitchenEventRepeater();
            BarKitchenRepeater.BarKitchenEvent += new BarKitchenDelegate(RefreshListView);
            RemoteServer.barKitchenEvent += new BarKitchenDelegate(BarKitchenRepeater.Repeater);
            
            LoadListViewDelegate llv = new LoadListViewDelegate(LoadListView);
            BeginInvoke(llv, new object[] { });

            updateStateBtn.Enabled = true;        
        }

        private void LoadListView()
        {
            foreach (RequestLine r in ActiveRequestList)
            {
                ListViewItem lvItem = new ListViewItem(new string[] { r.RequestNr.ToString(), r.TableNr.ToString(),
                    RestaurantProducts[r.Prod].Name, r.Qtt.ToString(), r.RState.ToString(), RestaurantProducts[r.Prod].PreparationSource.ToString() });
                listView1.Items.Add(lvItem);
            }
        }

        private void RefreshListView(RequestLine rl)
        {
            // TODO Get service type for function parameter

            RequestState rs = rl.RState;

            switch (rs)
            {
                case RequestState.Waiting:
                    ListViewItem lvi = CreateListViewItem(rl);
                    AddListViewRowDelegate addRow = new AddListViewRowDelegate(listView1.Items.Add);
                    BeginInvoke(addRow, new object[] { lvi });
                    break;
                case RequestState.InProgress:
                    UpdateRowStateDelegate updateState = new UpdateRowStateDelegate(UpdateRowAsync);
                    BeginInvoke(updateState, new object[] { rl });
                    break;
                case RequestState.Ready:
                    RemoveListViewRowDelegate rmRow = new RemoveListViewRowDelegate(RemoveRowAsync);
                    BeginInvoke(rmRow, new object[] { rl });
                    break;
                case RequestState.Delivered:
                    break;
                default:
                    break;
            }
        }

        private void UpdateRowAsync(RequestLine rl)
        {
            ListViewItem lvi = GetListViewItem(rl);
            if (lvi != null) lvi.SubItems[(int)LvColumn.State].Text = rl.RState.ToString(); 
        }
       
        private void RemoveRowAsync(RequestLine rl)
        {
            ListViewItem lvi = GetListViewItem(rl);
            if (lvi != null) listView1.Items.RemoveAt(lvi.Index);
        }

        // Gets the ListViewItem that matches the RequestLine parameter
        private ListViewItem GetListViewItem(RequestLine rl)
        {
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (Convert.ToInt16(lvi.SubItems[(int)LvColumn.Request].Text) == rl.RequestNr)
                {
                    return lvi;
                }
            }
            return null;
        }

        // Creates a ListViewItem with information from RequestLine parameter
        private ListViewItem CreateListViewItem(RequestLine rl)
        {
            ListViewItem lvItem = new ListViewItem(new string[] {
                rl.RequestNr.ToString(),
                rl.TableNr.ToString(),
                RestaurantProducts[rl.Prod].Name,
                rl.Qtt.ToString(),
                rl.RState.ToString(),
                RestaurantProducts[rl.Prod].PreparationSource.ToString()
            });

            return lvItem;
        }

    }

    class RemoteNew
    {
        private static Hashtable types = null;

        private static void InitTypeTable()
        {
            types = new Hashtable();

            foreach (WellKnownClientTypeEntry entry in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
            {
                types.Add(entry.ObjectType, entry);
            }
        }

        public static object New(Type type)
        {
            if (types == null)
            {
                InitTypeTable();
            }

            WellKnownClientTypeEntry entry = (WellKnownClientTypeEntry)types[type];

            if (entry == null)
            {
                throw new RemotingException("Type not found!");
            }

            return RemotingServices.Connect(type, entry.ObjectUrl);

        }

    }

}

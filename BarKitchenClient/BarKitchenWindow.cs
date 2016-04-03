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

        // Delegate for adding row to listview
        private delegate ListViewItem AddListViewRowDelegate(ListViewItem lvItem);

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

                // TODO Get service type for function parameter
                ServiceType = PreparationRoomID.Bar;
                ActiveRequestList = RemoteServer.GetActiveRequests(ServiceType);

                //Subscribe remote server events
                BarKitchenRepeater = new BarKitchenEventRepeater();
                BarKitchenRepeater.BarKitchenEvent += new BarKitchenDelegate(UpdateListView);
                RemoteServer.barKitchenEvent += new BarKitchenDelegate(BarKitchenRepeater.Repeater);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().FullName);
                Console.WriteLine(e.Message);
                Application.Exit();
            }

            // Windows Forms initialization
            InitializeComponent();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        private void BarKitchenWindow_Load(object sender, EventArgs e)
        {
            foreach (RequestLine r in ActiveRequestList)
            {
                ListViewItem lvItem = new ListViewItem(new string[] { r.RequestNr.ToString(), r.TableNr.ToString(),
                    RestaurantProducts[r.Prod].Name, r.Qtt.ToString(), r.RState.ToString(), RestaurantProducts[r.Prod].PreparationSource.ToString() });
                listView1.Items.Add(lvItem);
            }
        }

        private void UpdateRequestState_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (listView1.SelectedItems.Count < 1) return;

            String tableNr = listView1.SelectedItems[0].SubItems[(int)LvColumn.Table].Text;
            String requestNr = listView1.SelectedItems[0].SubItems[(int)LvColumn.Request].Text;
           
            RemoteServer.UpdateRequestLineState(UInt16.Parse(tableNr), UInt16.Parse(requestNr));
        }

        private void UpdateListView(RequestLine rl)
        {
            // TODO Get service type for function parameter

            RequestState rs = rl.RState;

            switch (rs)
            {
                case RequestState.Waiting:
                    AddRowListView(rl);
                    break;
                case RequestState.InProgress:
                    break;
                case RequestState.Ready:
                    break;
                case RequestState.Delivered:
                    break;
                default:
                    break;
            }
        }

        private void AddRowListView(RequestLine rl)
        {
            ListViewItem lvItem = new ListViewItem(new string[] { rl.RequestNr.ToString(), rl.TableNr.ToString(),
                    RestaurantProducts[rl.Prod].Name, rl.Qtt.ToString(), rl.RState.ToString(), RestaurantProducts[rl.Prod].PreparationSource.ToString() });
            AddListViewRowDelegate addRow = new AddListViewRowDelegate(listView1.Items.Add);
            BeginInvoke(addRow, new object[] { lvItem });
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

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
        ISingleServer remoteServer;

        // Restaurant product list
        List<Product> restaurantProducts;

        // Active request list
        List<RequestLine> activeRequestList;

        // BarKitchenEvent Repeater
        BarKitchenEventRepeater bkRepeater;

        // Delegate for adding row to listview
        private delegate ListViewItem AddListViewRowDelegate(ListViewItem lvItem);

        // List View Columns
        enum lvColumn { Request, Table, Product, Quantity, State, Service}

        // Service Type (Bar or Kitchen)
        private PreparationRoomID serviceType { get; set; }

        #endregion

        public BarKitchenWindow()
        {
            restaurantProducts = new List<Product>();
            activeRequestList = new List<RequestLine>();

            try
            {
                // Client configuration file
                RemotingConfiguration.Configure("BarKitchenClient.exe.config", false);

                // Remote Server object initialization
                remoteServer = (ISingleServer)RemoteNew.New(typeof(ISingleServer));

                // Get Restaurant products
                restaurantProducts = remoteServer.GetProducts();

                // TODO Get service type for function parameter
                serviceType = PreparationRoomID.Bar;
                activeRequestList = remoteServer.GetActiveRequests(serviceType);

                //Subscribe remote server events
                bkRepeater = new BarKitchenEventRepeater();
                bkRepeater.BarKitchenEvent += new BarKitchenDelegate(updateListView);
                remoteServer.barKitchenEvent += new BarKitchenDelegate(bkRepeater.Repeater);
                
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
            foreach (RequestLine r in activeRequestList)
            {
                ListViewItem lvItem = new ListViewItem(new string[] { r.RequestNr.ToString(), r.TableNr.ToString(),
                    restaurantProducts[r.Prod].Name, r.Qtt.ToString(), r.RState.ToString(), restaurantProducts[r.Prod].PreparationSource.ToString() });
                listView1.Items.Add(lvItem);
            }
        }

        private void updateRequestState_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (listView1.SelectedItems.Count < 1) return;

            String tableNr = listView1.SelectedItems[0].SubItems[(int)lvColumn.Table].Text;
            String requestNr = listView1.SelectedItems[0].SubItems[(int)lvColumn.Request].Text;
           
            remoteServer.UpdateRequestLineState(UInt16.Parse(tableNr), UInt16.Parse(requestNr));
        }

        private void updateListView(RequestLine rl)
        {
            // TODO Get service type for function parameter

            RequestState rs = rl.RState;

            switch (rs)
            {
                case RequestState.Waiting:
                    ListViewItem lvItem = new ListViewItem(new string[] { rl.RequestNr.ToString(), rl.TableNr.ToString(),
                    restaurantProducts[rl.Prod].Name, rl.Qtt.ToString(), rl.RState.ToString(), restaurantProducts[rl.Prod].PreparationSource.ToString() });
                    AddListViewRowDelegate addRow = new AddListViewRowDelegate(listView1.Items.Add);
                    BeginInvoke(addRow, new object[] { lvItem });
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

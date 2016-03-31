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

        // Restaurant tables list
        Table[] restaurantTables;

        // Restaurant product list
        List<Product> restaurantProducts;

        // Active request list
        List<RequestLine> activeRequestList;
        #endregion

        public BarKitchenWindow()
        {
            // Client configuration file
            RemotingConfiguration.Configure("BarKitchenClient.exe.config", false);

            try {
                // Remote Server object initialization
                remoteServer = (ISingleServer)RemoteNew.New(typeof(ISingleServer));
            } catch(RemotingException e)
            {
                Console.WriteLine(e.GetType().FullName);
                Console.WriteLine(e.Message);
            }

            // Windows Forms initialization
            InitializeComponent();
        }

        private void BarKitchenWindow_Load(object sender, EventArgs e)
        {
            // Get active requests from server and update listview on the app window
            updateRequestListView();
        }

        private void updateRequestState_Click(object sender, EventArgs e)
        {
            // TODO Prevent no row selected

            String requestNr = listView1.SelectedItems[0].Text;
            RequestLine rl = GetRequest(UInt16.Parse(requestNr));

            if(rl != null)
            {
                remoteServer.ChangeRequestState(rl);
                updateRequestListView();
            } else
            {
                Console.WriteLine("[BarKitchenApp] Unable to change request state");
            }
        }

        private void GetAllActiveRequests()
        {
            try
            {
                // Restaurant Tables
                restaurantTables = remoteServer.GetRestaurantTables();

                // Restaurant Products
                restaurantProducts = remoteServer.GetProducts();

                activeRequestList = new List<RequestLine>();

                foreach (Table t in restaurantTables)
                {
                    List<RequestLine> temp = t.getRequests();

                    foreach (RequestLine r in temp)
                    {
                        if (r.RState == RequestState.Waiting || r.RState == RequestState.InProgress)
                        {
                            activeRequestList.Add(r);
                        }
                    }

                    temp.Clear();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().FullName);
                Console.WriteLine(e.Message);
            }
            
        }

        private RequestLine GetRequest(ushort requestNr)
        {
            return activeRequestList.FirstOrDefault(r => r.RequestNr == requestNr);
        }

        private void updateRequestListView()
        {
            
            listView1.Items.Clear();
            GetAllActiveRequests();

            foreach (RequestLine r in activeRequestList)
            {
                ListViewItem lvItem = new ListViewItem(new string[] { r.RequestNr.ToString(), r.TableNr.ToString(),
                    restaurantProducts[r.Prod].Name, r.Qtt.ToString(), r.RState.ToString(), restaurantProducts[r.Prod].PreparationSource.ToString() });
                listView1.Items.Add(lvItem);
            }
        }

    }

    class RemoteNew
    {
        private static Hashtable types = null;

        private static void InitTypeTable()
        {
            types = new Hashtable();

            foreach(WellKnownClientTypeEntry entry in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
            {
                types.Add(entry.ObjectType, entry);
            }
        }

        public static object New(Type type)
        {
            if(types == null)
            {
                InitTypeTable();
            }

            WellKnownClientTypeEntry entry = (WellKnownClientTypeEntry) types[type];

            if(entry == null)
            {
                throw new RemotingException("Type not found!");
            }

            return RemotingServices.Connect(type, entry.ObjectUrl);

        }

    }

}

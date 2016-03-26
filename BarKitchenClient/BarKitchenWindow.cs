using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace BarKitchenClient
{
    public partial class BarKitchenWindow : Form
    {

        // Remote server interface
        ISingleServer remoteServer;

        public BarKitchenWindow()
        {
            // Client configuration file
            RemotingConfiguration.Configure("BarKitchenClient.exe.config", false);

            InitializeComponent();
            
            // Remote Server object initialization
            remoteServer = (ISingleServer)RemoteNew.New(typeof(ISingleServer));

            ushort nTables = remoteServer.GetNrTables();

            Console.WriteLine("Testing console...");
            Console.WriteLine("Nr Tables = {0}", nTables);
        }

        private void BarKitchenWindow_Load(object sender, EventArgs e)
        {
            
            ShowRequests();
        }

        private void ShowRequests()
        {
            ListViewItem item1 = new ListViewItem("Something");
            item1.SubItems.Add("SubItem1a");
            item1.SubItems.Add("SubItem1b");


            ListViewItem item2 = new ListViewItem("Something2");
            item2.SubItems.Add("SubItem2a");
            item2.SubItems.Add("SubItem2b");

            ListViewItem item3 = new ListViewItem("Something3");
            item3.SubItems.Add("SubItem3a");
            item3.SubItems.Add("SubItem3b");

            listView1.Items.AddRange(new ListViewItem[] { item1, item2, item3 });
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

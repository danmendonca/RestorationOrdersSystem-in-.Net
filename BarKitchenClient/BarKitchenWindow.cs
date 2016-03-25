using System;
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

            RemotingConfiguration.Configure("BarKitchenClient.exe.config", false);

            InitializeComponent();
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
}

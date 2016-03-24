using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.Remoting;

namespace BarKitchenClient
{
    static class BarKitchenClient
    {
        [STAThread]
        static void Main()
        {

            RemotingConfiguration.Configure("BarKitchenClient.exe.config", false);

            SingleServer ss = new SingleServer();
            
            
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BarKitchenWindow());
        }
        
    }

    
}
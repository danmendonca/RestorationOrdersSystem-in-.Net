using System;
using System.Runtime.Remoting;
using System.Windows.Forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        RemotingConfiguration.Configure("Client.exe.config", false);
        Window myWindow = new Window();
        Application.EnableVisualStyles();
        Application.Run(myWindow);
    }
}
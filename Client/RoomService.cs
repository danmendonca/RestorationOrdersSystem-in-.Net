using System;
using System.Runtime.Remoting;
using System.Windows.Forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        RemotingConfiguration.Configure("Client.exe.config", false);
        RoomWindow myWindow = new RoomWindow();
        Application.EnableVisualStyles();
        Application.Run(myWindow);
    }
}
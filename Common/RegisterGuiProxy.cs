using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class RegisterGuiProxy : MarshalByRefObject
{
    public event NewRequestDelegate EventNewRequest;
    public event TablePaidDelegate TablePaidEvent;


    public void NewRequestHandler(RequestLine rl)
    {
        if (EventNewRequest == null)
            return;

        Delegate[] invkList = TablePaidEvent.GetInvocationList();
        foreach (NewRequestDelegate handler in invkList)
        {
            //Console.WriteLine("I'm a new thread! Sending RequestReady messages");
            new Thread(() =>
            {
                try
                {
                    handler(rl);
                }
                catch (Exception)
                {
                    EventNewRequest -= handler;
                }
            }).Start();
        }
    }

    public void TablePaidNotifier(int table)
    {
        if (TablePaidEvent == null)
            return;

        Delegate[] invkList = TablePaidEvent.GetInvocationList();
        foreach (TablePaidDelegate handler in invkList)
        {
            //Console.WriteLine("I'm a new thread! Sending RequestReady messages");
            new Thread(() =>
            {
                try
                {
                    handler(table);
                }
                catch (Exception)
                {
                    TablePaidEvent -= handler;
                }
            }).Start();
        }
    }

    public override object InitializeLifetimeService()
    {
        return null;
    }
}

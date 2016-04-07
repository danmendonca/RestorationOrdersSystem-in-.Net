using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class RegisterGuiProxy : MarshalByRefObject
{
    public event TablePaidDelegate TablePaidEvent;
    public event RequestDelegate RequestEvent;

    public void TablePaidNotifier(int table)
    {
        if (TablePaidEvent == null)
            return;

        Delegate[] invkList = TablePaidEvent.GetInvocationList();
        foreach (TablePaidDelegate handler in invkList)
        {
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

    public void RequestsUpdateNotifier(RequestLine rl)
    {
        if (RequestEvent == null)
            return;

        Delegate[] invkList = RequestEvent.GetInvocationList();
        foreach (RequestDelegate handler in invkList)
        {
            new Thread(() =>
            {
                try
                {
                    handler(rl);
                }
                catch (Exception)
                {
                    RequestEvent -= handler;
                }
            }).Start();
        }
    }

    public override object InitializeLifetimeService()
    {
        return null;
    }
}

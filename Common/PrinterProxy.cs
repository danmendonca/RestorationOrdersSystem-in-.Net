using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class PrinterProxy : MarshalByRefObject
{
    public event InvoiceDelegate InvoiceEvent;
    public event InvoiceDelegate ConsultTableEvent;

    public void InvoiceHandler(List<RequestLine> tableRequests)
    {
        if (InvoiceEvent == null)
            return;

        Delegate[] invkList = InvoiceEvent.GetInvocationList();
        foreach (InvoiceDelegate handler in invkList)
        {
            new Thread(() =>
            {
                try
                {
                    handler(tableRequests);
                }
                catch (Exception)
                {
                    InvoiceEvent -= handler;
                }
            }).Start();
        }
    }

    public void ConsultTableHandler(List<RequestLine> tableRequests)
    {
        if (ConsultTableEvent == null)
            return;

        Delegate[] invkList = ConsultTableEvent.GetInvocationList();
        foreach (InvoiceDelegate handler in invkList)
        {
            new Thread(() =>
            {
                try
                {
                    handler(tableRequests);
                }
                catch (Exception)
                {
                    InvoiceEvent -= handler;
                }
            }).Start();
        }
    }

    public override object InitializeLifetimeService()
    {
        return null;
    }
}

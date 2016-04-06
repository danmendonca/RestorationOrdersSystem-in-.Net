using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class PrinterProxy : MarshalByRefObject
{
    public event InvoiceDelegate InvoiceEvent;

    public void TableInvoiceHandler(List<RequestLine> tableRequests)
    {
        if (InvoiceEvent == null)
            return;

        Delegate[] invkList = InvoiceEvent.GetInvocationList();
        foreach (InvoiceDelegate handler in invkList)
        {
            //Console.WriteLine("I'm a new thread! Sending RequestReady messages");
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
}

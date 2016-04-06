using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Printer
{
    List<RequestLine> invoiceLines;
    List<Product> products;
    ISingleServer registerServer;
    PrinterProxy proxy;

    #region initialization        
    public Printer()
    {
        invoiceLines = new List<RequestLine>();
        registerServer = (ISingleServer)RemoteNew.New(typeof(ISingleServer));
        proxy = new PrinterProxy();
        products = registerServer.GetProducts();

        InvoiceDelegate proxyID = new InvoiceDelegate(InvoiceHandler);
        InvoiceDelegate serverID = new InvoiceDelegate(proxy.TableInvoiceHandler);

        proxy.InvoiceEvent += proxyID;
        registerServer.InvoiceEvent += serverID;
    }

    #endregion

    void InvoiceHandler(List<RequestLine> lRl)
    {
        Console.Clear();
        int line = 1;
        float total = 0;
        foreach (var rl in lRl)
        {
            Product p = products.ElementAt(rl.Prod);
            float lineTotal = rl.Qtt * p.Price;
            Console.WriteLine($"{line,3} {p.Name,15} {rl.Qtt,3} {lineTotal,4}€");
            total += lineTotal;
            line++;
        }
        Console.WriteLine($"Total: {total,21}€");
    }
}

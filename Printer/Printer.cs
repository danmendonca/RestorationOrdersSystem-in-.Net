using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Printer
{
    static string CULTURE_NAME = "en-GB";


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

        InvoiceDelegate selfID = new InvoiceDelegate(InvoiceHandler);
        InvoiceDelegate proxyID = new InvoiceDelegate(proxy.InvoiceHandler);
        proxy.InvoiceEvent += selfID;
        registerServer.InvoiceEvent += proxyID;


        InvoiceDelegate selfCTD = new InvoiceDelegate(ConsultTableHandler);
        InvoiceDelegate proxyCTD = new InvoiceDelegate(proxy.ConsultTableHandler);
        proxy.ConsultTableEvent += selfCTD;
        registerServer.ConsultTableEvent += proxyCTD;
    }

    #endregion

    void InvoiceHandler(List<RequestLine> lRl)
    {
        Console.Clear();
        int line = 1;
        float total = 0;
        DateTime current = DateTime.Now;
        var culture = new CultureInfo(CULTURE_NAME);
        string invoiceTime = $"Invoice date: {current.ToString(culture)}";

        Console.WriteLine("RestBar Company\n\n");

        String header = String.Format("{0,4} {1,15} {2,8} {3,8}", "line", "product", "Qtt", "total");
        Console.WriteLine(header);
        foreach (var rl in lRl)
        {
            Product p = products.ElementAt(rl.Prod);
            float lineTotal = rl.Qtt * p.Price;
            Console.WriteLine($"{line,4} {p.Name,15} {rl.Qtt,8} {lineTotal,8}$");
            total += lineTotal;
            line++;
        }
        Console.WriteLine($"Total: {total,31}$");

        Console.WriteLine(); Console.WriteLine();
        Console.WriteLine($"{invoiceTime,39}");
    }

    void ConsultTableHandler(List<RequestLine> lRl)
    {
        if (lRl == null || lRl.Count < 1) return;

        int tableNr = lRl.ElementAt(0).TableNr;

        Console.Clear();
        int line = 1;
        float total = 0;
        DateTime current = DateTime.Now;

        Console.WriteLine($"Table nr:{tableNr} - Requests Consult");
        foreach (var rl in lRl)
        {
            Product p = products.ElementAt(rl.Prod);
            float lineTotal = rl.Qtt * p.Price;
            Console.WriteLine($"{line,3} {p.Name,15} {rl.Qtt,3} {lineTotal,4}$");
            total += lineTotal;
            line++;
        }
        Console.WriteLine($"Total: {total,21}$");
    }
}

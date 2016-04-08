using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

static class Server
{
    [STAThread]
    static void Main(string[] args)
    {
        RemotingConfiguration.Configure("Server.exe.config", false);
        Console.WriteLine("[Server]: Press Return to terminate.");
        Console.ReadLine();
    }
}

public class SingleServer : MarshalByRefObject, ISingleServer
{
    #region Statics
    private static ushort NR_OF_TABLES = 8;
    #endregion



    #region attributes
    private List<List<RequestLine>> bills = new List<List<RequestLine>>();
    private List<Product> products = new List<Product>();
    #endregion



    #region Properties
    public List<Table> Tables { get; private set; } = new List<Table>();
    ushort NrTables { get; set; }
    ushort NrRequests { get; set; }
    #endregion



    #region events
    public event RequestDelegate RequestEvent;
    public event BarKitchenDelegate barKitchenEvent;
    public event TablePaidDelegate TablePaidEvent;
    public event InvoiceDelegate InvoiceEvent;
    #endregion



    #region classMethods
    public SingleServer()
    {
        CreateProducts();
        NrTables = NR_OF_TABLES;
        CreateTables();
        NrRequests = 0;
    }

    private void CreateTables()
    {
        //tables = new Table[NrTables];
        for (ushort i = 0; i < NrTables; i++) Tables.Add(new Table(i));
    }

    private void CreateProducts()
    {
        if (products.Count > 0)
            return;

        Product p1 = new Product("Francesinha", 9.5f, PreparationRoomID.Kitchen);
        products.Add(p1);
        Product p2 = new Product("Pica-Pau", 7.0f, PreparationRoomID.Kitchen);
        products.Add(p2);
        Product p3 = new Product("Sopa", 2.5f, PreparationRoomID.Kitchen);
        products.Add(p3);
        Product p4 = new Product("Tosta Mista", 1.5f, PreparationRoomID.Bar);
        products.Add(p4);
        Product p5 = new Product("Torrada", 0.80f, PreparationRoomID.Bar);
        products.Add(p5);
        Product p6 = new Product("Coca-Cola", 1.0f, PreparationRoomID.Bar);
        products.Add(p6);
        Product p7 = new Product("RedBull", 1.7f, PreparationRoomID.Bar);
        products.Add(p7);
    }


    private void TablePaymentNotifier(int table)
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

    private void TableInvoiceNotifier(List<RequestLine> tablesRequests)
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
                    handler(tablesRequests);
                }
                catch (Exception)
                {
                    InvoiceEvent -= handler;
                }
            }).Start();
        }
    }


    public void RequestNotifier(RequestLine rl)
    {
        if (RequestEvent == null)
        {
            return;
        }

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
    #endregion



    #region Interface Implementation
    void ISingleServer.MakeRequest(RequestLine aRl)
    {
        
        if (aRl.TableNr > NrTables || Tables.ElementAt(aRl.TableNr).TblState != TableStateID.Available)
            return;

        RequestLine rl = new RequestLine(aRl);
        rl.RequestNr = NrRequests++;
        Tables.ElementAt(rl.TableNr).insertNewRequest(rl);

        RequestNotifier(rl);
        NotifyBarKitchen(rl);
    }

    public ushort GetNrTables()
    {
        return NrTables;
    }

    public List<RequestLine> GetNonDeliveredRequests()
    {
        List<RequestLine> nonDliveredRequests = new List<RequestLine>();
        foreach (var item in Tables)
        {
            foreach (var rl in item.getRequests())
            {
                if (rl.RState != RequestState.Delivered)
                    nonDliveredRequests.Add(rl);
            }
        }
        return nonDliveredRequests;
    }

    List<Table> ISingleServer.GetTables()
    {
        return Tables;
    }

    List<RequestLine> ISingleServer.GetTableRLs(int selectedIndex)
    {
        return Tables.ElementAt(selectedIndex).getRequests();
    }

    List<Product> ISingleServer.GetProducts()
    {
        return products;
    }


    bool ISingleServer.RequestBill(ushort tableNr)
    {
        if (tableNr > NrTables || Tables.ElementAt(tableNr).TblState != TableStateID.Available)
            return false;

        Tables.ElementAt(tableNr).changeState();
        bills.Add(Tables.ElementAt(tableNr).getRequests());

        return true;
    }

    public void SetTablePaid(int t)
    {
        bills.Add(Tables.ElementAt(t).getRequests());
        Tables.ElementAt(t).changeState();

        TableInvoiceNotifier(Tables.ElementAt(t).getRequests());

        Tables.ElementAt(t).ClearRequests();
        TablePaymentNotifier(t);
    }

    void ISingleServer.SetRequestDelivered(int tblNr, ushort rNumber)
    {
        foreach (RequestLine reqL in Tables.ElementAt(tblNr).getRequests())
        {
            if (reqL.RequestNr != rNumber) continue;

            reqL.RState = RequestState.Delivered;
            RequestNotifier(reqL);
            break;
        }


    }
    #endregion



    #region Bar Kitchen Methods Definition

    private RequestLine GetRequest(int tableNr, int requestNr)
    {
        return Tables.ElementAt(tableNr).getRequests().FirstOrDefault(r => r.RequestNr == requestNr);
    }

    // From all table requests returns requests with waiting or in progress status
    List<RequestLine> ISingleServer.GetActiveRequests(PreparationRoomID service)
    {
        List<RequestLine> activeRequestList = new List<RequestLine>();

        foreach (Table t in Tables)
        {
            List<RequestLine> temp = t.getRequests();

            foreach (RequestLine r in temp)
            {
                if ((r.RState == RequestState.Waiting || r.RState == RequestState.InProgress) && (products[r.Prod].PreparationSource == service))
                {
                    activeRequestList.Add(r);
                }
            }

        }

        return activeRequestList;
    }

    void ISingleServer.UpdateRequestLineState(int tableNr, int requestNr)
    {
        RequestLine rl = GetRequest(tableNr, requestNr);

        if (rl != null)
        {
            rl.changeState();
            NotifyBarKitchen(rl);

            RequestNotifier(rl);

        }
        else
        {
        }
    }

    private void NotifyBarKitchen(RequestLine rl)
    {
        if (barKitchenEvent != null)
        {
            Delegate[] invkList = barKitchenEvent.GetInvocationList();

            foreach (BarKitchenDelegate handler in invkList)
            {
                new Thread(() =>
                {
                    try
                    {
                        handler(rl);
                    }
                    catch (Exception)
                    {
                        barKitchenEvent -= handler;
                    }
                }).Start();
            }
        }
    }

    #endregion



    #region Overrides
    public override object InitializeLifetimeService()
    {
        return null;
    }
    #endregion

}


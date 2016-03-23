using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Threading;

class Server
{
    static void Main(string[] args)
    {
        RemotingConfiguration.Configure("Server.exe.config", false);
        Console.WriteLine("[Server]: Press Return to terminate.");
        Console.ReadLine();
    }
}

public class SingleServer : MarshalByRefObject, ISingleServer
{
    private static ushort NR_OF_TABLES = 8;
    Hashtable table = new Hashtable();
    private List<Product> products = new List<Product>();
    ushort NrTables { get; set; }
    ushort NrRequests { get; set; }
    Table[] tables;
    List<List<RequestLine>> bills = new List<List<RequestLine>>();

    public event RequestReadyDelegate requestReadyEvent;
    public event BarRequestDelegate barRequestEvent;
    public event RestaurantDelegate restaurantRequestEvent;

    public SingleServer()
    {
        CreateProducts();
        NrTables = NR_OF_TABLES;
        CreateTables();
        NrRequests = 0;
    }

    private void CreateTables()
    {
        tables = new Table[NrTables];
        for (ushort i = 0; i < NrTables; i++) tables[(int)i] = new Table(i);
    }

    public void ChangeRequestState(RequestLine rl)
    {
        if (requestReadyEvent == null)
            return;

        Delegate[] invkList = requestReadyEvent.GetInvocationList();
        foreach (RequestReadyDelegate handler in invkList)
        {
            new Thread(() =>
            {
                try
                {
                    handler(rl);
                }
                catch (Exception)
                {
                    requestReadyEvent -= handler;
                }
            }).Start();
        }

        throw new NotImplementedException();
    }

    public void ClientAddress(string address)    //TODO guid and address are not necessary as we are using event subscribers
    {
        IRoomService rem = (IRoomService)RemotingServices.Connect(typeof(IRoomService), address);
        rem.SetProducts(products);
        rem.SetNrTables(NrTables);
    }

    public void MakeRequest(ushort tableNr, ushort p, ushort qtty, string dsc)
    {
        if (tableNr > NrTables || tables[tableNr].TblState != TableStateID.Available)
            return;

        RequestLine rl = new RequestLine(NrRequests++, p, qtty, tableNr, dsc);
        tables[tableNr].insertNewRequest(rl);

        Console.WriteLine($"Added new request to table {rl.TableNr} for {rl.Qtt} of {products[p]}");
    }

    public bool RequestBill(ushort t)
    {
        if (t > NrTables || tables[t].TblState != TableStateID.Available)
            return false;

        tables[t].changeState();
        bills.Add(tables[t].getRequests());

        return true;
    }

    private void CreateProducts()
    {
        if (products.Count > 0)
            return;

        Product p1 = new Product("Francesinha", 9.5f, PreparationRoomID.Restaurant);
        products.Add(p1);
        Product p2 = new Product("Pica-Pau", 7.0f, PreparationRoomID.Restaurant);
        products.Add(p2);
        Product p3 = new Product("Sopa", 2.5f, PreparationRoomID.Restaurant);
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

    public void PayTable(ushort t)
    {
        if (t > NrTables || tables[t].TblState != TableStateID.Paying)
            return;

        bills.Add(tables[t].getRequests());
        tables[t].changeState();
        tables[t].ClearRequests();
    }
}

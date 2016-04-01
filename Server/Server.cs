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

    public void PayTable(ushort t)
    {
        if (t > NrTables || tables[t].TblState != TableStateID.Paying)
            return;

        bills.Add(tables[t].getRequests());
        tables[t].changeState();
        tables[t].ClearRequests();
    }

    void ISingleServer.MakeRequest(RequestLine rl)
    {
        Console.WriteLine($"Added new request to table {rl.TableNr} for {rl.Qtt} of {products[rl.Prod]}");
        if (rl.TableNr > NrTables || tables[rl.TableNr].TblState != TableStateID.Available)
            return;
        rl.RequestNr = NrRequests++;
        tables[rl.TableNr].insertNewRequest(rl);

        //testing purposes while no restaurant/bar implementation
        ChangeRequestState(rl);
        /*
        List<RequestLine> tableRls = tables[rl.TableNr].getRequests();
        if (tableRls.Count > 1)
        {
            int previous = tableRls.Count - 2;
            tables[rl.TableNr].changeRequestState(tableRls[previous].RequestNr);
            ChangeRequestState(tableRls[previous]);
        }
        */

    }

    public void ChangeRequestState(RequestLine rl)
    {
        if (requestReadyEvent == null)
        {
            Console.WriteLine("No subscribers ");
            return;
        }

        Delegate[] invkList = requestReadyEvent.GetInvocationList();
        foreach (RequestReadyDelegate handler in invkList)
        {
            Console.WriteLine("I'm a new thread! Sending RequestReady messages");
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
    }

    ushort ISingleServer.GetNrTables()
    {
        return NrTables;
    }

    List<Product> ISingleServer.GetProducts()
    {
        return products;
    }

    bool ISingleServer.RequestBill(ushort tableNr)
    {
        if (tableNr > NrTables || tables[tableNr].TblState != TableStateID.Available)
            return false;

        tables[tableNr].changeState();
        bills.Add(tables[tableNr].getRequests());

        return true;
    }

    void ISingleServer.SetRequestDelivered(int tblNr, ushort rNumber)
    {
        foreach (RequestLine reqL in tables[tblNr].getRequests())
        {
            if (reqL.RequestNr != rNumber) continue;
            Console.WriteLine($"Request nr: {reqL.RequestNr} delivered");
            reqL.RState = RequestState.Delivered;
            ChangeRequestState(reqL);
            break;
        }


    }

    List<RequestLine> ISingleServer.GetActiveRequests(PreparationRoomID service)
    {
        List<RequestLine> activeRequestList = new List<RequestLine>();

        foreach (Table t in tables)
        {
            List<RequestLine> temp = t.getRequests();

            foreach (RequestLine r in temp)
            {
                if (r.RState == RequestState.Waiting || r.RState == RequestState.InProgress)
                {
                    activeRequestList.Add(r);
                }
            }

            temp.Clear();
        }

        return activeRequestList;
    }

}

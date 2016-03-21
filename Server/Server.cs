using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;

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
    private static short NR_OF_TABLES = 8;
    Hashtable table = new Hashtable();
    private List<Product> products = new List<Product>();
    short NrTables { get; set; }
    short NrRequests { get; set; }
    Table[] tables;


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
        for (short i = 0; i < NrTables; i++) tables[(int)i] = new Table(i);
    }

    public void ChangeRequestState(RequestLine rl)
    {
        throw new NotImplementedException();
    }

    public void ClientAddress(Guid guid, string address)
    {

        if (!table.ContainsKey(guid))
            table.Add(guid, address);

        IRoomService rem = (IRoomService)RemotingServices.Connect(typeof(IRoomService), address);
        rem.SetProducts(products);
        rem.SetNrTables(NrTables);
        Console.WriteLine("[SingleServer]: Registered " + address);
    }

    public void MakeRequest(short tableNr, short p, short qtty, string dsc)
    {
        if (tableNr > NrTables || tables[tableNr].TblState != TableStateID.Available)
            return;

        RequestLine rl = new RequestLine(NrRequests++, p, qtty, tableNr, dsc);
        tables[tableNr].insertNewRequest(rl);

        Console.WriteLine($"Added new request to table {rl.TableNr} for {rl.Qtt} of {products[p]}");
    }

    public bool RequestBill(short t)
    {
        throw new NotImplementedException();
    }

    public void SomeCall(Guid guid)
    {
        if (table.ContainsKey(guid))
        {
            IRoomService rem = (IRoomService)RemotingServices.Connect(typeof(IRoomService), (string)table[guid]); //reference to the client remote obj
            Console.WriteLine("[SingleServer]: Obtained the client remote object");
            rem.SomeMessage("Server calling Client");
        }
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
}

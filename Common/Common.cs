using System;
using System.Collections.Generic;

[Serializable]
public enum PreparationRoomID { Bar, Restaurant };
[Serializable]
public enum TableStateID { Available, Paying };
[Serializable]
public enum RequestState { Waiting, InProgress, Ready, Delivered };

[Serializable]
public class Product
{
    public string Name { get; protected set; }
    public float Price { get; protected set; }
    PreparationRoomID PreparationSource { get; set; }

    public Product(string name, float price, PreparationRoomID PreparationSource)
    {
        Name = name;
        Price = price;
        this.PreparationSource = PreparationSource;
    }

    public override string ToString()
    {
        return $"{Name} {Price,3}€";
    }
}

[Serializable]

public class RequestLine : MarshalByRefObject
{
    public ushort Prod { get; protected set; }
    public int Qtt { get; protected set; }
    public ushort RequestNr { get; set; }
    public string Description { get; private set; }
    public RequestState RState { get; set; }
    public int TableNr { get; set; }

    public RequestLine(ushort requestNr, ushort prodIndex, int qtt, ushort tblNr, string desc)
    {
        Prod = prodIndex;
        Qtt = qtt;
        TableNr = tblNr;
        RequestNr = requestNr;

        if (desc != null)
            Description = desc;
        else Description = "";

        RState = RequestState.Waiting;
    }

    public bool changeState()
    {
        switch (RState)
        {
            case RequestState.Waiting:
                RState = RequestState.InProgress;
                return true;
            case RequestState.InProgress:
                RState = RequestState.Ready;
                return true;
            default:
                return false;
        }
    }

    public override string ToString()
    {
        return $"{Prod.ToString(),10} {Qtt,3} {TableNr,2}";
    }
}

[Serializable]
public class Table
{
    public ushort TableNr { get; private set; }
    public TableStateID TblState { get; private set; }
    protected List<RequestLine> requests = new List<RequestLine>();

    public Table(ushort tblNr)
    {
        TblState = TableStateID.Available;
        TableNr = tblNr;
    }


    public bool insertNewRequest(RequestLine rq)
    {
        if (TableStateID.Paying == TblState)
            return false;

        requests.Add(rq);
        return true;
    }

    public void changeState()
    {
        TblState = (TblState == TableStateID.Available) ? TableStateID.Paying : TableStateID.Available;
    }

    public override string ToString()
    {
        return TableNr.ToString();
    }

    public List<RequestLine> getRequests()
    {
        return this.requests;
    }

    public void ClearRequests()
    {
        this.requests = new List<RequestLine>();
    }

    public void changeRequestState(ushort rNr)
    {
        foreach(RequestLine rl in requests)
        {
            if (rl.RequestNr != rNr)
                continue;
            rl.changeState();
            break;
        }
    }
}

#region delegates
public delegate void RequestReadyDelegate(RequestLine rl);
public delegate void BarRequestDelegate(RequestLine rl);
public delegate void RestaurantDelegate(RequestLine rl);
public delegate void RequestDeliveredDelegate(RequestLine rl);


public delegate void NrTablesDelegate(ushort n);
public delegate void ProductListDelegate(List<Product> lp);
#endregion


#region Interfaces
public interface ISingleServer
{
    event RequestReadyDelegate requestReadyEvent;
    event BarRequestDelegate barRequestEvent;
    event RestaurantDelegate restaurantRequestEvent;
    void ChangeRequestState(RequestLine rl);
    void MakeRequest(RequestLine rl);
    ushort GetNrTables();
    List<Product> GetProducts();
    bool RequestBill(ushort tableNr);
    void SetRequestDelivered(int tblNr, ushort rNumber);
}

public interface IRoomService
{
    void requestNotification(RequestLine rl);
}

#endregion
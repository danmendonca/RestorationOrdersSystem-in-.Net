using System;
using System.Collections.Generic;

public enum PreparationRoomID { Bar, Restaurant };
public enum TableStateID { Available, Paying };
public enum RequestState { Waiting, InProgress, Ready };

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
    public short Prod { get; protected set; }
    public int Qtt { get; protected set; }
    public short RequestNr { get; set; }
    public string Description { get; private set; }
    public RequestState RState { get; set; }
    public int TableNr { get; set; }

    public RequestLine(short requestNr, short prodIndex, int qtt, short tblNr, string desc)
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


public class Table
{
    public short TableNr { get; private set; }
    public TableStateID TblState { get; private set; }
    protected List<RequestLine> requests = new List<RequestLine>();

    public Table(Int16 tblNr)
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
}

#region Interfaces
public interface ISingleServer
{
    void ClientAddress(Guid guid, string address);
    void SomeCall(Guid guid);
    void MakeRequest(short tableNr, short p, short qtty, String dsc);
    bool RequestBill(short tableNr);
    void ChangeRequestState(RequestLine rl);
}

public interface IRoomService
{
    void SomeMessage(string message);
    void SetProducts(List<Product> ps);
    void SetNrTables(short nrTbls);
    void requestNotification(RequestLine rl);
}

#endregion
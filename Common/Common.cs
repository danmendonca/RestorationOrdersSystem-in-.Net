using System;
using System.Collections.Generic;



#region Enums
[Serializable]
public enum PreparationRoomID { Bar, Kitchen };
[Serializable]
public enum TableStateID { Available, Paying };
[Serializable]
public enum RequestState { Waiting, InProgress, Ready, Delivered };
#endregion



#region classes
[Serializable]
public class Product
{
    #region Properties
    public string Name { get; protected set; }
    public float Price { get; protected set; }
    public PreparationRoomID PreparationSource { get; set; }
    #endregion

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

    #region attributes
    private RequestState _requestState;
    #endregion



    #region Properties
    public ushort Prod { get; protected set; }
    public int Qtt { get; protected set; }
    public ushort RequestNr { get; set; }
    public string Description { get; private set; }
    public RequestState RState
    {
        get { return _requestState; }
        set
        {
            switch (value)
            {
                case RequestState.Waiting:
                    _requestState = value;
                    break;
                case RequestState.InProgress:
                    if (RState == RequestState.Waiting)
                        _requestState = value;
                    break;
                case RequestState.Ready:
                    if (RState != RequestState.Delivered)
                        _requestState = value;
                    break;
                case RequestState.Delivered:
                    _requestState = value;
                    break;
                default:
                    break;
            }
        }
    }
    public int TableNr { get; set; }
    #endregion



    #region overrides
    public override bool Equals(object obj)
    {
        // Check for null values and compare run-time types.
        if (obj == null || GetType() != obj.GetType())
            return false;

        return (this.RequestNr == ((RequestLine)obj).RequestNr) ? true : false;
    }


    public override int GetHashCode()
    {
        return Convert.ToInt32(RequestNr);
    }


    public override string ToString()
    {
        return $"{Prod.ToString(),10} {Qtt,3} {TableNr,2}";
    }
    #endregion



    #region constructor
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
    public RequestLine(RequestLine rl)
    {
        Prod = rl.Prod;
        Qtt = rl.Qtt;
        TableNr = rl.TableNr;
        RequestNr = rl.RequestNr;

        if (rl.Description != null)
            Description = rl.Description;
        else Description = "";

        RState = RequestState.Waiting;
    }
    #endregion

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
    }

[Serializable]
public class Table
{
    #region properties

    public ushort TableNr { get; private set; }
    public TableStateID TblState { get; private set; }

    #endregion



    #region attributes

    protected List<RequestLine> requests = new List<RequestLine>();

    #endregion



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
        foreach (RequestLine rl in requests)
        {
            if (rl.RequestNr != rNr)
                continue;
            rl.changeState();
            break;
        }
    }

}
#endregion



#region Room Service Delegates
public delegate void RequestReadyDelegate(RequestLine rl);
public delegate void RequestDeliveredDelegate(RequestLine rl);
public delegate void NrTablesDelegate(ushort n);
public delegate void ProductListDelegate(List<Product> lp);
#endregion



#region Bar Kitchen Delegates

public delegate void BarKitchenDelegate(RequestLine rl);

public class BarKitchenEventRepeater : MarshalByRefObject
{
    public event BarKitchenDelegate BarKitchenEvent;

    public override object InitializeLifetimeService()
    {
        return null;
    }

    public void Repeater(RequestLine rl)
    {
        if (BarKitchenEvent != null)
            BarKitchenEvent(rl);
    }
}

#endregion



#region Interfaces
public interface ISingleServer
{
    #region events
    event RequestReadyDelegate requestReadyEvent;
    event BarKitchenDelegate barKitchenEvent;
    #endregion

    bool RequestBill(ushort tableNr);
    List<RequestLine> GetActiveRequests(PreparationRoomID service);
    List<Product> GetProducts();
    ushort GetNrTables();
    void ChangeRequestState(RequestLine rl);
    void MakeRequest(RequestLine rl);
    void SetRequestDelivered(int tblNr, ushort rNumber);
    void UpdateRequestLineState(int tableNr, int requestNr);

}


public interface IRoomService
{
    void requestNotification(RequestLine rl);
}

#endregion
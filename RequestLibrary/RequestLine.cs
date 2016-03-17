using System;

namespace RequestLibrary
{
    public class RequestLine
    {
        public Product Prod { get; protected set; }
        public int Qtt { get; protected set; }
        public int RequestNr { get; set; }
        public string Description { get; private set; }
        public RequestState RState{get;set;}
        public int TableNr { get; set;}

        public RequestLine(Product prod, int qtt, Int16 tblNr, string desc)
        {
            Prod = prod;
            Qtt = qtt;
            TableNr = tblNr;
            RequestNr = -1;

            if (desc != null)
                Description = desc;
            else Description = "";

            RState = RequestState.Waiting;
        }

        public bool changeState()
        {
            switch(RState)
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
            return $"{Prod.ToString(), 10} {Qtt, 3} {TableNr, 2}";
        }
    }
}
using System;
using System.Collections.Generic;

namespace RequestLibrary
{
    public class Table
    {
        public Int16 TableNr { get; private set; }
        public TableStateID TblState {get; private set;}
        protected List<RequestLine> requests;
        Product product;

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
}

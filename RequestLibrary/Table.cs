using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    class Table
    {
        public Int16 TableNr { get; private set; }
        public TableStateID TblState;
        List<RequestLine> requests;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    class Table
    {

        public enum state { available, inPayment };

        public Int16 TableNr { get; private set; }
        public int TblState;
        List<RequestLine> requests;
        Product product;

        public Table(Int16 tblNr)
        {
            TblState = (int) state.available;
            TableNr = tblNr;
        }


        public bool insertNewRequest(RequestLine rq)
        {
            if ((int)state.inPayment == TblState)
                return false;

            requests.Add(rq);
            return true;
        }

        public void changeState()
        {
            TblState = (TblState == (int)state.available) ? (int)state.inPayment : (int)state.available;
        }

        public override string ToString()
        {
            return TableNr.ToString();
        }
    }
}

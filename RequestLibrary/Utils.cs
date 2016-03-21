using System;
using System.Collections.Generic;

namespace RequestLibrary
{
    public enum PreparationRoomID { Bar, Restaurant };
    public enum TableStateID { Available, Paying };
    public enum RequestState { Waiting, InProgress, Ready };

    public interface IRegister
    {
        bool MakeRequest(short tableNr, Product p, Int16 qtty, String dsc);
        bool RequestBill(short t);
        void ChangeRequestState(RequestLine rl);
        List<Product> GetProducts();
        short GetNrTables();
        void ClientAddress(string address);
    }


    public interface IRequestPreparation
    {
        void receiveRequest(RequestLine rl);
    }

    public interface IRoom
    {
        void requestNotification(RequestLine rl);
    }
}
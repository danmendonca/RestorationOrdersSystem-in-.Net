using System;
using System.Threading;

public class RoomProxy : MarshalByRefObject, IRoomService
{
    public event RequestDeliveredDelegate rDEvent;
    public event RequestReadyDelegate rREvent;

    public void RepeaterRReady(RequestLine rl)
    {
        if (rREvent == null)
        {
            Console.WriteLine("RoomProxy: No subscribers");
            return;
        }

        Delegate[] invkList = rREvent.GetInvocationList();
        foreach (RequestReadyDelegate handler in invkList)
        {//TODO Will the event really have more than one subscriber?
            new Thread(() =>
            {
                try
                {
                    handler(rl);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.StackTrace);
                    rREvent -= handler;
                }
            }).Start();
        }
    }

    public void RepeaterRDelivered(RequestLine rl)
    {
        if (rDEvent == null)
        {
            Console.WriteLine("No subscribers ");
            return;
        }

        Delegate[] invkList = rDEvent.GetInvocationList();
        foreach (RequestReadyDelegate handler in invkList)
        {
            new Thread(() =>
            {
                try
                {
                    handler(rl);
                }
                catch (Exception)
                {
                    rREvent -= handler;
                }
            }).Start();
        }
    }

    public override object InitializeLifetimeService()
    {
        return null;
    }

    public void requestNotification(RequestLine rl)
    {
        throw new NotImplementedException();
    }
}

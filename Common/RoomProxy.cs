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
            Console.WriteLine("Repeater: No subscribers");
            return;
        }

        Delegate[] invkList = rREvent.GetInvocationList();
        foreach (RequestReadyDelegate handler in invkList)
        {
            Console.WriteLine("I'm a new thread! Sending RequestReady messages");
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
            Console.WriteLine("I'm a new thread! Sending RequestReady messages");
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

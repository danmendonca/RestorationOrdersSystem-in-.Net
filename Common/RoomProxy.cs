using System;
using System.Threading;

public class RoomProxy : MarshalByRefObject, IRoomService
{
    public event RequestDelegate rREvent;

    public void RepeaterRReady(RequestLine rl)
    {
        if (rREvent == null)
        {
            return;
        }

        Delegate[] invkList = rREvent.GetInvocationList();
        foreach (RequestDelegate handler in invkList)
        {
            new Thread(() =>
            {
                try
                {
                    handler(rl);
                }
                catch (Exception exception)
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

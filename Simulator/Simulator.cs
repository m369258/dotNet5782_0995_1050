namespace Simulator;
public static  class Simulator
{
    private static event EventHandler<EventStatusArgs>? reportStart;
    private static event EventHandler<EventStatusArgs>? reportEnd;
    private static event EventHandler? reportEndSim;

    private static BlApi.IBl bl = BlApi.Factory.Get();
    volatile private static bool isActive;

    public static void Deactive() => isActive = false;

    public static void Active()
    {
        Random rn = new Random();
        new Thread(() =>
            {
                isActive = true;
                while (isActive)
                {
                    int? oldId = bl.order.GetOldestOrder();
                    if (oldId != null)
                    {
                        BO.Order boOrder = bl.order.GetOrderDetails((int)oldId);
                        int delay = rn.Next(3, 11);
                        DateTime now = DateTime.Now;
                        DateTime time = now + new TimeSpan(delay * 1000);
                        BO.OrderStatus willl = (BO.OrderStatus)(((int)boOrder.status) + 1);
                        EventStatusArgs args = new EventStatusArgs()
                        {
                            OrderId = (int)oldId,
                            start = now,
                            finish = time,
                            now = (BO.OrderStatus)boOrder.status,
                            will= willl
                        };
                        reportStart?.Invoke(null, args);
                        Thread.Sleep(delay * 1000);
                        EventStatusArgs args1 = new EventStatusArgs()
                        {
                            OrderId = (int)oldId
                        };
                            //reportEnd?.Invoke(null, args1);
                        bl.order.UpdateStatus((int)oldId);
                    }
                    Thread.Sleep(1000);
                }
                reportEndSim?.Invoke(null,EventArgs.Empty);
            }).Start();
    }

    public static void ReportStart(EventHandler<EventStatusArgs> ev) => reportStart += ev;

    public static void DereportStart(EventHandler<EventStatusArgs> ev) => reportStart -= ev;

    public static void ReportEnd(EventHandler<EventStatusArgs> ev) => reportEnd += ev;
    public static void DereportEnd(EventHandler<EventStatusArgs> ev) => reportEnd -= ev;

    public static void ReportEndSim(EventHandler ev) => reportEndSim += ev;
    public static void DereportEndSim(EventHandler ev) => reportEndSim -= ev;

}

public class EventStatusArgs:EventArgs
{
    public int OrderId { get; set; }
    public DateTime start { get; set; }
    public DateTime finish { get; set; }
    public BO.OrderStatus now { get; set; }
    public BO.OrderStatus will { get; set; }
}
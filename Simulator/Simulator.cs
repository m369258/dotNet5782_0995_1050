namespace Simulator;
public static  class Simulator
{
    /// האם לעשות כאן ציבורי או לעשות פעולות שדרכם נרשמים

    //private static event EventHandler RreportStart;
    //private static event EventHandler ReportEnd;
    //private static event EventHandler ReportEndSim;

    public static event EventHandler reportStart;
    public static event EventHandler reportEnd;
    public static event EventHandler reportEndSim;

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
                    int? oldId = bl.order.GetOldestOrder();//ליצור ב BL
                    if (oldId != null)
                    {
                        BO.Order boOrder = bl.order.GetOrderDetails((int)oldId);
                        int delay = rn.Next(3, 11);
                        DateTime time = DateTime.Now + new TimeSpan(delay * 1000);
                        //ReportStart();//האם זה נכון??????????????? לא ידעתי מה לשים בכל ה NEW
                        Thread.Sleep(delay * 1000);
                        //ReportEnd();
                        bl.order.UpdateStatus((int)oldId);//ליצור ב BL
                    }
                    Thread.Sleep(1000);
                }
                    //ReportEndSim();
            }).Start();
    }

}

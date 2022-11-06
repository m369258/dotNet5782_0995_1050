namespace Dal;

internal static class DataSource
{
    static Random rand = new Random(DateTime.Now.Millisecond); //המחלקה של סטטי שדה
    static int num = rand.Next();

    //static internal Order
}

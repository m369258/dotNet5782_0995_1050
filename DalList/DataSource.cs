using Do;
namespace Dal;

internal static class DataSource
{
    //Declaration of a random variable
    static Random rand = new Random(DateTime.Now.Millisecond);
    static int num = rand.Next();

    //A constant variable that maintains the size of the arrays
    private const int sizeOfProdArr = 50;
    private const int sizeOfOrderArr = 100;
    private const int sizeOfOrderItemArr = 200;

    //Declaration + assignment of arrays of entities that are consumed
    static internal Product[] products = new Product[sizeOfProdArr];
    static internal Order[] orders = new Order[sizeOfOrderArr];
    static internal OrderItem[] orderItems = new OrderItem[sizeOfOrderItemArr];


    /// <summary>
    /// An internal class for handling the automatic identifiers and saving the number of existing objects from each actual entity
    /// </summary>
    internal static class Config
    {
        //Variables for saving the amount of objects from each actual entity
        static internal int indexProduct = 0;
        static internal int indexOrder = 0;
        static internal int indexOrderItem = 0;

        //Saving the entities' automatic identifiers
        static int automaticOrder = 1000;
        static int automaticOrderItem = 1000;

        public static int AutomaticOrder
        {
            get { return ++automaticOrder; }
        }
        public static int AutomaticOrderItem
        {
            get { return ++automaticOrderItem; }
        }

        static Config() => orders[0].ID = 1001;
    }


    /// <summary>
    /// static constructive action
    /// </summary>
    static DataSource()
    {
        s_Initialize();
    }


    /// <summary>
    /// The method calls the methods of adding objects to the entity arrays
    /// </summary>
    private static void s_Initialize()
    {
        createProducts();
        createOrders();
        createOrderItems();
    }


    /// <summary>
    /// This action initializes the products details
    /// </summary>
    private static void createProducts()
    {
        string[] productNames = { "מארז 12 קאפקייקס בינוני ליום הולדת", "בייבי בלו קייק", "קנדי קייק קטנה", "מיניקייק קרמל", "מארז גדול של מקרונים COOL BLUE", "מארז קטן של מקרונים PRIDE", "מארז גדול של מקרונים LOVE", "מארז קטן של מקרונים UNICORN", "מארז גדול של מקרונים CHOCOHOLIC", "בלונדיז", "נשיקות מרנג", "בלון לידת בת", "בלון לידת בן", "עוגת יום הולדת", "עוגת אוראו" };
        int[] categories = { 1, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 5, 5, 2, 2 };
        float[] prices = { 158, 285, 285, 125, 220, 85, 220, 85, 220, 42, 22, 28, 28, 200, 285 };
        for (int i = 0; i < 15; i++)
        {
            products[i].ID = (i + 1) * 1000000;
            products[i].Name = productNames[i];
            products[i].Category = (Category)categories[i];
            products[i].Price = prices[i];
            if (i <= productNames.Length * 0.05)
                products[i].InStock = 0;
            else
                products[i].InStock = i * 100;
            Config.indexProduct++;
        }
    }

    /// <summary>
    /// This action resets the order details
    /// </summary>
    private static void createOrders()
    {
        string[] customerNames = { "שירה נוסבכר", "מיכל גרינבוים", "זיסי לוי", "אורית כץ", "מירי ויזל", "מימי רוט", "חני כהן", "ציפורה אולמן", "נועה לובין", " שני פורייס", " דבי רוזנברג", "נחמי לב", "תמר פריימן", "משה שטיינמץ ", "הילה איצקוביץ ", " דניאל וייס", " יהונתן בלחדב", "נעם ברקוביץ", " שולמית גוגיג", " מאיר רוט" };

        string[] customerAddresses = { "בעל התניא 20 בני ברק", "גולומב 5 בני ברק", "יצחק אלחנן 2 תל-אביב-יפו", "גורדון 22 בני ברק", "שדרות ירושלים 66 רמת גן", "אבן גבירול 8 בני ברק", "רשי 5 בני ברק", "שבזי 2 בני ברק", "חרמון 3 רעננה", "הבנים 77 הוד השרון", "הרי גולן 113 הרצליה", "שבט נפתלי 1 אשדוד", "רחבת מבצע ארז 3 באר שבע", "גנתון 43 גינתון", "חברון 13 תל אביב - יפו", "כפר הס 9931 כפר הס", "דוד רמז 5 לוד", "עין ורד 159 עין ורד", "דקר 5 בני ברק", "ירושלים 11 בני ברק" };
        for (int i = 0; i < 20; i++)
        {
            orders[i].ID = Config.AutomaticOrder;
            orders[i].CustomerName = customerNames[i];
            orders[i].CustomerEmail = customerNames[i] + "@gmail.com";
            orders[i].CustomerAddress = customerAddresses[i];
            orders[i].OrderDate = DateTime.MinValue + new TimeSpan(rand.Next(2001, 2022) * 365, 0, 0, 0);

            //About 80% of the orders will have a delivery date that must be after the order creation time
            if (i <= 0.8 * 20)
                orders[i].ShipDate = orders[i].OrderDate + new TimeSpan(2, 0, 0, 0);
            else
                orders[i].ShipDate = orders[i].OrderDate;
            //About 60% of orders sent will have a delivery date
            if (i <= 0.6 * 20)
                orders[i].DeliveryDate = orders[i].ShipDate + new TimeSpan(3, 0, 0, 0);
            else
                orders[i].DeliveryDate = orders[i].ShipDate;
            Config.indexOrder++;
        }
    }

    /// <summary>
    /// This action initializes the details of the products in the order
    /// </summary>
    private static void createOrderItems()
    {
        int cOrderItems = 0;//Lists the details of all orders that have joined
        //For each order adds between one and four items
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < rand.Next(1, 5); j++)
            {
                orderItems[cOrderItems].ID = Config.AutomaticOrderItem;
                orderItems[cOrderItems].OrderId = i + 1001;
                orderItems[cOrderItems].ProductId = products[rand.Next(1, Config.indexProduct)].ID;
                orderItems[cOrderItems].Price = findPrice(orderItems[i].ProductId);
                orderItems[cOrderItems].Amount = i + rand.Next(20, 100);
                cOrderItems++;
                Config.indexOrderItem++;
            }
            //In the event that 40 items were not added to all the orders,
            //we will perform another partial round so that at the end of the function there will be at least order details
            if (i == 19 && cOrderItems < 40)
                i = i - cOrderItems;
        }
    }

    /// <summary>
    /// This operation imports the price of the product according to the product ID
    /// </summary>
    /// <param name="productId">id product</param>
    /// <returns>The price of the requested product in any other case will return -1</returns>
    private static double findPrice(int productId)
    {
        for (int i = 0; i < Config.indexProduct; i++)
        {
            if (productId == products[i].ID)
                return products[i].Price;
        }
        return -1;
    }
}
using Do;
namespace Dal;

internal static class DataSource
{
    //Declaration of a random variable
    static Random rand = new Random(DateTime.Now.Millisecond);
    static int num = rand.Next();

    //Declaration + assignment of arrays of entities that are consumed
    static internal Product[] products = new Product[50];
    static internal List<Order> orders;
    static internal OrderItem[] orderItems = new OrderItem[200];

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
        static int automaticOrder = 0;
        static int automaticOrderItem = 0;

        public static int AutomaticOrder
        {
            get { return ++automaticOrder; }
        }
        public static int AutomaticOrderItem
        {
            get { return ++automaticOrderItem; }
        }

        static Config() => products[0] = new Product();

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

        Order myOrder = new Order();
        for (int i = 0; i < 20; i++)
        {
            myOrder.ID = Config.AutomaticOrder;
            myOrder.CustomerName = customerNames[i];
            myOrder.CustomerEmail = customerNames[i] + "@gmail.com";
            myOrder.CustomerAddress = customerAddresses[i];
            myOrder.OrderDate = DateTime.MinValue + new TimeSpan(rand.Next(2001, 2022) * 365, 0, 0, 0);
            if (i <= 0.8 * 20)
                myOrder.DeliveryDate = orders[i].OrderDate + new TimeSpan(3, 0, 0, 0);
            if (i <= 0.6 * 20)
                myOrder.ShipDate = orders[i].DeliveryDate + new TimeSpan(2, 0, 0, 0);
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
                orderItems[cOrderItems].OrderId = i;
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

    /// <summary>
    /// This operation adds a order 
    /// </summary>
    /// <param name="order">Order to add</param>
    internal static void Add(Order order)
    {
        int ind = Config.indexOrder++;
        orders[ind] = order;
    }

    /// <summary>
    /// This operation adds a product if its ID number is not in the system
    /// </summary>
    /// <param name="product">Product to add</param>
    /// <exception cref="Exception">If the ID number exists, an error will be thrown</exception>
    internal static void Add(Product product)
    {
        int i;
        //The loop checks if there is a product with the requested ID number, if so it will throw an error
        for (i = 0; i < Config.indexProduct && products[i].ID != product.ID; i++) ;
        if (i != Config.indexProduct)
        {
            throw new Exception("this product is exsist");
        }
        int ind = Config.indexProduct++;
        products[ind] = product;
    }

    /// <summary>
    /// This action adds an order item
    /// </summary>
    /// <param name="orderItem">Order item to be added</param>
    /// <exception cref="Exception">Throw an error if the product or order does not exist</exception>
    internal static void Add(OrderItem orderItem)
    {
        int i;
        //Checking whether the product ID exists in any other case will throw an error
        for (i = 0; i < Config.indexProduct && products[i].ID != orderItem.ProductId; i++) ;
        if (i == Config.indexProduct)
        {
            throw new Exception("this product is exsist");
        }

        //Checking if the order ID exists in any other case will throw an error
        for (i = 0; i < Config.indexOrder && orders[i].ID != orderItem.OrderId; i++) ;
        if (i == Config.indexOrder)
        {
            throw new Exception("this order is exsist");
        }

        //Adding the order item to the database and updating the actual quantity
        int ind = Config.indexOrderItem++;
        orderItems[ind] = orderItem;
    }


}

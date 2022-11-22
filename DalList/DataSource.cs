using Do;
namespace Dal;

internal static class DataSource
{
    //Declaration of a random variable
    static Random rand = new Random(DateTime.Now.Millisecond);
    static int num = rand.Next();

    //Declaration + assignment of arrays of entities that are consumed
    static internal List<Product> products;
    static internal List<Order> orders;
    static internal List <OrderItem> orderItems;

    /// <summary>
    /// An internal class for handling the automatic identifiers and saving the number of existing objects from each actual entity
    /// </summary>
    internal static class Config
    {

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
        Product product = new Product();
        string[] productNames = { "מארז 12 קאפקייקס בינוני ליום הולדת", "בייבי בלו קייק", "קנדי קייק קטנה", "מיניקייק קרמל", "מארז גדול של מקרונים COOL BLUE", "מארז קטן של מקרונים PRIDE", "מארז גדול של מקרונים LOVE", "מארז קטן של מקרונים UNICORN", "מארז גדול של מקרונים CHOCOHOLIC", "בלונדיז", "נשיקות מרנג", "בלון לידת בת", "בלון לידת בן", "עוגת יום הולדת", "עוגת אוראו" };
        int[] categories = { 1, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 5, 5, 2, 2 };
        float[] prices = { 158, 285, 285, 125, 220, 85, 220, 85, 220, 42, 22, 28, 28, 200, 285 };
        for (int i = 0; i < 15; i++)
        {
            product.ID = (i + 1) * 1000000;
            product.Name = productNames[i];
            product.Category = (Category)categories[i];
            product.Price = prices[i];
            if (i <= productNames.Length * 0.05)
                product.InStock = 0;
            else
                product.InStock = i * 100;
            products.Add(product);
        }
    }

    /// <summary>
    /// This action resets the order details
    /// </summary>
    private static void createOrders()
    {
        string[] customerNames = { "shira nussbacher", "michal grinboim", "zisi levi", " orit catz", "miri vizel ", "mimi rot", " chani cohen", " zipora adler", "noa lubin", "shani pories", "debi rozenberg", "nechami lev", "tammar praiman", "moshe shtaitemetz", "hila shtainemetz", "daniel weiss", "jonatan mualem", "noam levkovitch", "shulamit gugig", "meir rot" };

        string[] customerAddresses = { "Baal Atania 20 Bnei Brak", " ", "יצחק אלחנן 2 תל-אביב-יפו", "גורדון 22 בני ברק", "שדרות ירושלים 66 רמת גן", "אבן גבירול 8 בני ברק", "רשי 5 בני ברק", "שבזי 2 בני ברק", "חרמון 3 רעננה", "הבנים 77 הוד השרון", "הרי גולן 113 הרצליה", "שבט נפתלי 1 אשדוד", "רחבת מבצע ארז 3 באר שבע", "גנתון 43 גינתון", "חברון 13 תל אביב - יפו", "כפר הס 9931 כפר הס", "דוד רמז 5 לוד", "עין ורד 159 עין ורד", "דקר 5 בני ברק", "ירושלים 11 בני ברק" };

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
        }
    }

    /// <summary>
    /// This action initializes the details of the products in the order
    /// </summary>
    private static void createOrderItems()
    {
        OrderItem orderItem = new OrderItem();
        int cOrderItems = 0;//Lists the details of all orders that have joined
        //For each order adds between one and four items
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < rand.Next(1, 5); j++)
            {
                orderItem.ID = Config.AutomaticOrderItem;
                orderItem.OrderId = i;
                orderItem.ProductId = products[rand.Next(1, products.Count)].ID;
                orderItem.Price = findPrice(orderItems[i].ProductId);
                orderItem.Amount = i + rand.Next(20, 100);
                cOrderItems++;
                orderItems.Add(orderItem);
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
        
        Product p = products.First();
       for(int i=0;i<products.Count;i++)
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
        
        orders.Add( order);
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
       
        for (i = 0; i < products.Count && products[i].ID != product.ID; i++) ;
        if (i != products.Count)
        {
            throw new Exception("this product is exsist");
        }
        products.Add( product);
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
        for (i = 0; i < products.Count && products[i].ID != orderItem.ProductId; i++) ;
        if (i == products.Count)
        {
            throw new Exception("this product is exsist");
        }

        //Checking if the order ID exists in any other case will throw an error
        for (i = 0; i < orders.Count && orders[i].ID != orderItem.OrderId; i++) ;
        if (i == orders.Count)
        {
            throw new Exception("this order is exsist");
        }

        //Adding the order item to the database and updating the actual quantity
        orderItems.Add(orderItem);
    }


}

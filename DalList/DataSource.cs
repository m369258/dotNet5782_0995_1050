﻿using Do;
namespace Dal;

internal static class DataSource
{
    //Declaration of a random variable
    static Random rand = new Random(DateTime.Now.Millisecond);
    static int num = rand.Next();

    //Declaration + assignment of arrays of entities that are consumed
    internal static List<Product?> products = new List<Product?>();
    internal static List<Order?> orders = new List<Order?>();
    internal static List<OrderItem?> orderItems = new List<OrderItem?>();
    internal static List<Users?> users = new List<Users?>();


    /// <summary>
    /// An internal class for handling the automatic identifiers and saving the number of existing objects from each actual entity
    /// </summary>
    internal static class Config
    {
        //Saving the entities' automatic identifiers
        static int automaticOrder = 1000;
        static int automaticOrderItem = 1000;
        static int automaticUsers = 1000;

        public static int AutomaticOrder
        {
            get { return ++automaticOrder; }
        }
        public static int AutomaticOrderItem
        {
            get { return ++automaticOrderItem; }
        }
        public static int AutomaticUsers
        {
            get { return ++automaticUsers; }
        }

        static Config() => orders.Count();
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
        creatUsers();
    }


    /// <summary>
    /// This action initializes the products details
    /// </summary>
    private static void createProducts()
    {
        string[] productNames = { "מארז 12 קאפקייקס בינוני ליום הולדת", "בייבי בלו קייק", "קנדי קייק קטנה", "מיניקייק קרמל", "מארז גדול של מקרונים COOL BLUE", "מארז קטן של מקרונים PRIDE", "מארז גדול של מקרונים LOVE", "מארז קטן של מקרונים UNICORN", "מארז גדול של מקרונים CHOCOHOLIC", "בלונדיז", "נשיקות מרנג", "בלון לידת בת", "בלון לידת בן", "עוגת יום הולדת", "עוגת אוראו" };
        int[] categories = { 1, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 5, 5, 2, 2 };
        float[] prices = { 158, 285, 285, 125, 220, 85, 220, 85, 220, 42, 22, 28, 28, 200, 285 };
        string[] images = { "oreo-cake-416x416.png", "IMG-1818-1-416x416.png", "IMG_3070-416x416.png", "IMG_2722-2-324x324.png", "IMG_2564-2-324x324.png", "heightCake.png", "happyBirthday.png", "bigCake.png", "12_medium_birthday_-copy-2-416x416.png", "IMG-9167-416x416.png", "love_12-copy-2-416x416.png", "printed-cake-pink-416x416.png", "IMG-1818-1-416x416.png", "IMG-9167-416x416.png", "IMG-9162-416x416.png" };
        for (int i = 0; i < 15; i++)
        {
            Product product = new Product();
            product.ID = (i + 1) * 1000000;
            product.Name = productNames[i];
            product.Category = (Category)categories[i];
            product.Price = prices[i];
            if (i <= productNames.Length * 0.05)
                product.InStock = 0;
            else
                product.InStock = i * 100;
            product.Img = images[i];
            products.Add(product);
        }
    }

    private static void creatUsers()
    {
        string[] usersName = { "shira6557", "michal.grinboim", "זיסי לוי", "אורית כץ", "מירי ויזל", "מימי רוט", "חני כהן", "ציפורה אולמן", "נועה לובין", " שני פורייס", " דבי רוזנברג", "נחמי לב", "תמר פריימן", "משה שטיינמץ ", "הילה איצקוביץ ", " דניאל וייס", " יהונתן בלחדב", "נעם ברקוביץ", " שולמית גוגיג", " מאיר רוט" };
        string[] usersEmail = { "shira6557@gmail.com", "michal.grinboim@gmail.com", "זיסי לוי", "אורית כץ", "מירי ויזל", "מימי רוט", "חני כהן", "ציפורה אולמן", "נועה לובין", " שני פורייס", " דבי רוזנברג", "נחמי לב", "תמר פריימן", "משה שטיינמץ ", "הילה איצקוביץ ", " דניאל וייס", " יהונתן בלחדב", "נעם ברקוביץ", " שולמית גוגיג", " מאיר רוט" };
        string[] usersAddress = { "בעל התניא 20 בני ברק", "גולומב 5 בני ברק", "יצחק אלחנן 2 תל-אביב-יפו", "גורדון 22 בני ברק", "שדרות ירושלים 66 רמת גן", "אבן גבירול 8 בני ברק", "רשי 5 בני ברק", "שבזי 2 בני ברק", "חרמון 3 רעננה", "הבנים 77 הוד השרון", "הרי גולן 113 הרצליה", "שבט נפתלי 1 אשדוד", "רחבת מבצע ארז 3 באר שבע", "גנתון 43 גינתון", "חברון 13 תל אביב - יפו", "כפר הס 9931 כפר הס", "דוד רמז 5 לוד", "עין ורד 159 עין ורד", "דקר 5 בני ברק", "ירושלים 11 בני ברק" };

        for (int i = 0; i < 15; i++)
        {
            Users user = new Users();
            user.ID = Config.AutomaticUsers;
            user.Name = usersName[i];
            user.Address = usersAddress[i];
            user.Email = usersEmail[i] ;
            user.Password =( i * 12356).ToString();
            user.TypeOfUser = (Do.TypeOfUser)0;
            users.Add(user);
        }
    }

    /// <summary>
    /// This action resets the order details
    /// </summary>
    private static void createOrders()
    {
        string[] customerNames = { "shira6557@gmail.com", "michall@gmail.com", "זיסי לוי", "אורית כץ", "מירי ויזל", "מימי רוט", "חני כהן", "ציפורה אולמן", "נועה לובין", " שני פורייס", " דבי רוזנברג", "נחמי לב", "תמר פריימן", "משה שטיינמץ ", "הילה איצקוביץ ", " דניאל וייס", " יהונתן בלחדב", "נעם ברקוביץ", " שולמית גוגיג", " מאיר רוט" };

        string[] customerAddresses = { "בעל התניא 20 בני ברק", "גולומב 5 בני ברק", "יצחק אלחנן 2 תל-אביב-יפו", "גורדון 22 בני ברק", "שדרות ירושלים 66 רמת גן", "אבן גבירול 8 בני ברק", "רשי 5 בני ברק", "שבזי 2 בני ברק", "חרמון 3 רעננה", "הבנים 77 הוד השרון", "הרי גולן 113 הרצליה", "שבט נפתלי 1 אשדוד", "רחבת מבצע ארז 3 באר שבע", "גנתון 43 גינתון", "חברון 13 תל אביב - יפו", "כפר הס 9931 כפר הס", "דוד רמז 5 לוד", "עין ורד 159 עין ורד", "דקר 5 בני ברק", "ירושלים 11 בני ברק" };
        for (int i = 0; i < 20; i++)
        {
            Order order = new Order();
            order.ID = Config.AutomaticOrder;
            order.CustomerName = customerNames[i];
            order.CustomerEmail = customerNames[i];
            order.CustomerAddress = customerAddresses[i];
            //order.OrderDate = DateTime.MinValue + new TimeSpan(rand.Next(2001, 2022) * 365, 0, 0, 0);

            order.OrderDate=DateTime.Now;
            order.ShipDate = null;
            order.DeliveryDate = null;

            //About 80% of the orders will have a delivery date that must be after the order creation time
            //if (i <= 0.8 * 20)
            //    order.ShipDate = order.OrderDate + new TimeSpan(2, 0, 0, 0);
            //else
            //    order.ShipDate = order.OrderDate;
            ////About 60% of orders sent will have a delivery date
            //if (i <= 0.6 * 20)
            //    order.DeliveryDate = order.ShipDate + new TimeSpan(3, 0, 0, 0);
            //else
            //    order.DeliveryDate = order.ShipDate;
            orders.Add(order);
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
                OrderItem orderItem = new OrderItem();
                orderItem.ID = Config.AutomaticOrderItem;
                orderItem.OrderId = i + 1001;
                orderItem.ProductId = products[rand.Next(1, products.Count)]?.ID??throw new Exception();
                orderItem.Price = findPrice(orderItem.ProductId);
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
        for (int i = 0; i < products.Count; i++)
        {
            if (productId == products[i]?.ID)
                return products[i]?.Price ?? throw new Exception();
        }
        return -1;
    }
}
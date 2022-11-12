using Do;
using System.Net.NetworkInformation;

namespace Dal;

internal static class DataSource
{

    static Random rand = new Random(DateTime.Now.Millisecond); //המחלקה של סטטי שדה
    static int num = rand.Next();

    static internal Product[] products = new Product[50];
    static internal Order[] orders = new Order[100];
    static internal OrderItem[] orderItems = new OrderItem[200];

    internal static class Config
    {
        static internal int indexProduct = 0;
        static internal int indexOrder = 0;
        static internal int indexOrderItem = 0;

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
        string[] productNames = { "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p10", "p11", "p12", "p13", "p14", "p15" };
        for (int i = 0; i < 15; i++)
        {
            products[i] = new Product();
            products[i].ID = (i + 1) * 1000000;//??
            products[i].Name = productNames[i];
            products[i].Category = (Category)rand.Next(5);
            products[i].Price = (i + 1) * 10;
            if (i <= productNames.Length * 0.05)
                products[i].InStock = 0;
            else
                products[i].InStock = i * 100;
        }
    }

    /// <summary>
    /// This action resets the order details
    /// </summary>
    private static void createOrders()
    {
        string[] orderNames = { "o1", "o2", "o3", "o4", "o5", "o6", "o7", "o8", "o9", "o10", "o11", "o12", "o13", "o14", "o15", "o16", "o17", "o18", "o19", "o20" };
        string[] customerAddresses = { "a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8", "a9", "a10", "a12", "a13", "a14", "a15", "a16", "a17", "a18", "a19", "a20" };
        for (int i = 0; i < 20; i++)
        {
            orders[i] = new Order();
            orders[i].ID = Config.AutomaticOrder;
            orders[i].CustomerName = orderNames[i];
            orders[i].CustomerEmail = orderNames[i] + "@gmail.com";//??האם צריך לבדוק את המקרה שאולי ל שתי אנשים הם בעלי אותו שם
            orders[i].CustomerAddress = customerAddresses[i];
            orders[i].OrderDate = DateTime.MinValue + new TimeSpan(rand.Next(2001, 2022) * 365, 0, 0, 0);
            if (i <= 0.8 * 20)
                orders[i].DeliveryDate = orders[i].OrderDate + new TimeSpan(3, 0, 0, 0);
            if (i <= 0.6 * 20)
                orders[i].ShipDate = orders[i].DeliveryDate + new TimeSpan(2, 0, 0, 0);
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
                orderItems[i] = new OrderItem();
                orderItems[i].ID = Config.AutomaticOrderItem;// i + i * j;//!!לבדוק
                orderItems[i].OrderId = i;
                orderItems[i].ProductId = rand.Next(15);
                orderItems[i].Price = ff(orderItems[i].ProductId);//??
                orderItems[i].Amount = i + rand.Next(20, 100);
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
    /// 
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    private static double ff(int productId)
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
        for (i = 0; i < Config.indexProduct && products[i].ID != product.ID ; i++);
        if(i==Config.indexProduct)
        {
            throw new Exception("this product is exsist");
        }
        int ind = Config.indexProduct++;
        products[ind] = product;
    }

    internal static void Add(OrderItem orderItem)
    {
        int i;
        for (i = 0; i < Config.indexProduct && products[i].ID != orderItem.ProductId; i++) ;
        if(i==Config.indexProduct)
        {
            throw new Exception("this product is exsist");
        }

        for (i = 0; i < Config.indexOrder && orders[i].ID != orderItem.OrderId; i++) ;
        if (i == Config.indexOrder)
        {
            throw new Exception("this order is exsist");
        }
        int ind = Config.indexOrderItem++;
        orderItems[ind] = orderItem;
    }



}

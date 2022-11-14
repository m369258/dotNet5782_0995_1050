using Do;
using Dal;

enum MainMenu { Exist = 0, Product, Order, OrderItem }
enum Options { Add = 1, Get, GetAll, Update, Delete, GetByIDOrder, GetByIDOrderAndIDProduct }
class OurProgram
{
    /// <summary>
    /// A class variable for each entity and entity
    /// </summary>
    private static DalOrder dalOrder = new DalOrder();
    private static DalProduct dalProduct = new DalProduct();
    private static DalOrderItem dalOrderItem = new DalOrderItem();
    static void Main()
    {
        MainMenu choice;
        //Receiving a voluntary action number to perform
        Console.WriteLine(@"please choose one of the following:
0: exit
1: prodeuct
2: order
3: order-item");
        choice = (MainMenu)int.Parse(Console.ReadLine());
        //As long as 0 was not pressed to exit
        while (choice != 0)
        {
            switch (choice)
            {
                case MainMenu.Product:
                    submenuOfProduct();
                    break;
                case MainMenu.Order:
                    submenuOfOrder();
                    break;
                case MainMenu.OrderItem:
                    submenuOfOrderItem();
                    break;
                default:
                    break;
            }
            Console.WriteLine(@"please choose one of the following:
0: exit
1: prodeuct
2: order
3: order-item");
            choice = (MainMenu)int.Parse(Console.ReadLine());
        }
    }

    private static void submenuOfProduct()
    {
        Options choice;
        //Print the checklist for the entity
        Console.WriteLine(@"enter your choice:
1: add an order
2: get an order by ID
3: get all orders 
4: update an order
5: delete an order ");
        //Accepting the user's choice
        if (!Options.TryParse(Console.ReadLine(), out choice)) throw new Exception("choice is in valid");

        int idProduct;
        Product p;

        try
        {
            switch (choice)
            {
                case Options.Add:
                    Console.WriteLine("Adding a product");
                    Console.Write("enter idProduct with 6 numbers:");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    p = InputProduct();
                    p.ID = idProduct;
                    Console.WriteLine(dalProduct.Add(p));
                    Console.WriteLine("Product whose number:{0} has been successfully added", idProduct);
                    break;

                case Options.Get:
                    Console.WriteLine("Receiving a number by the ID");
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    Console.WriteLine(dalProduct.Get(idProduct));
                    break;

                case Options.GetAll:
                    Product[] products = dalProduct.GetAllProducts();
                    foreach (Product myProduct in products)
                    {
                        Console.WriteLine(myProduct);
                    }
                    break;

                case Options.Update:
                    Console.WriteLine("Product update:");
                    Console.WriteLine("enter id product:");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    Console.WriteLine("The requested product before the change");
                    Console.WriteLine(dalProduct.Get(idProduct));
                    p = InputProduct();
                    p.ID = idProduct;
                    //will update the product only if all the details have been verified
                    if (p.ID != null && p.Name != null && p.Category != null && p.InStock != null && p.Price != null)
                    {
                        dalProduct.Update(p);
                        Console.WriteLine("The requested product after the change");
                        Console.WriteLine(dalProduct.Get(idProduct));
                    }
                    break;

                case Options.Delete:
                    Console.WriteLine("Product to be deleted");
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    dalProduct.Delete(idProduct);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    /// <summary>
    /// This action performs input from the user details about the product
    /// </summary>
    /// <returns>Returns a product with the details entered by the user</returns>
    /// <exception cref="Exception">If one of the inputs was wrong</exception>
    private static Product InputProduct()
    {
        int instock;
        double price;
        string name;
        Category category;
        Console.WriteLine("enter name, category, price, instock of product");
        name = Console.ReadLine();
        if (!Category.TryParse(Console.ReadLine(), out category)) throw new Exception("category is in valid");
        if (!double.TryParse(Console.ReadLine(), out price)) throw new Exception("price is in valid");
        if (!int.TryParse(Console.ReadLine(), out instock)) throw new Exception("inStock is in valid");
        Product p = new Product();
        p.Name = name;
        p.Category = category;
        p.Price = price;
        p.InStock = instock;

        return p;
    }

    /// <summary>
    /// The method is a sub-menu for an order, you can choose to add an object, 
    /// display an object, display a list of orders, update and delete an order.
    /// </summary>
    private static void submenuOfOrder()
    {
        Options choice;
        //Print the checklist for the entity
        Console.WriteLine(@"enter your choice:
a: add an order
b: get an order by ID
c: delete an order
d: update an order
e: get all orders");
        //Accepting the user's choice
        if (!Options.TryParse(Console.ReadLine(), out choice)) throw new Exception("choice is invalid");
        int idOrder;
        Order myOrder;

        try
        {
            //Checking what action the user wants to run
            switch (choice)
            {
                case Options.Add:
                    //A call to an action that receives data for an order object
                    myOrder = InputOrder();
                    //A call to action that adds an order to the system
                    Console.WriteLine(dalOrder.Add(myOrder));
                    break;

                case Options.Get:
                    //Receipt of order ID number
                    Console.WriteLine("enter the id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    Console.WriteLine(dalOrder.Get(idOrder));
                    break;

                case Options.GetAll:
                    Order[] orders = dalOrder.GetAllOrders();
                    foreach (Order currOrder in orders)
                    {
                        Console.WriteLine(currOrder);
                    }
                    break;

                case Options.Update:
                    Console.WriteLine("enter id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    Console.WriteLine("before: ");
                    Console.WriteLine(dalOrder.Get(idOrder));
                    myOrder = InputOrder();
                    myOrder.ID = idOrder;
                    //In the event that all order details have not been provided, this order will not be updated.
                    if (myOrder.ID != null && myOrder.CustomerName != null && myOrder.CustomerEmail != null && myOrder.CustomerAddress != null &&
                        myOrder.OrderDate != null && myOrder.DeliveryDate != null && myOrder.ShipDate != null)
                    {
                        dalOrder.Update(myOrder);
                        Console.WriteLine("after: ");
                        Console.WriteLine(dalOrder.Get(idOrder));
                    }
                    break;

                case Options.Delete:
                    Console.WriteLine("enter the id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    dalOrder.Delete(idOrder);
                    break;
            }
        }
        //In case of any error an appropriate error will be thrown
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    /// <summary>
    /// Receiving order details from the user
    /// </summary>
    /// <returns>order with details from the user</returns>
    /// <exception cref="Exception">In case incorrect details were provided</exception>
    private static Order InputOrder()
    {
        Order order = new Order();
        string customerName, customerEmail, customerAddress;
        DateTime orderDate, deliveryDate, shipDate;

        //Receiving details from the user
        Console.WriteLine("enter customer name, customer email, customer address, order date , delivery date, ship date");
        customerName = Console.ReadLine();
        customerEmail = Console.ReadLine();
        customerAddress = Console.ReadLine();
        if (!DateTime.TryParse(Console.ReadLine(), out orderDate)) throw new Exception("order date is invalid");
        if (!DateTime.TryParse(Console.ReadLine(), out deliveryDate)) throw new Exception("delivery date is invalid");
        if (!DateTime.TryParse(Console.ReadLine(), out shipDate)) throw new Exception("shipDate is invalid");

        //Put the values received from the user into an order object
        order.CustomerName = customerName;
        order.CustomerEmail = customerEmail;
        order.CustomerAddress = customerAddress;
        order.OrderDate = orderDate;
        order.DeliveryDate = deliveryDate;
        order.ShipDate = shipDate;

        return order;
    }

    private static void submenuOfOrderItem()
    {
        string choice;
        //Print the checklist for the entity
        Console.WriteLine(@"enter your choice:
a: add an order
b: get an order by ID
c: delete an order
d: update an order
e: get all orders
f: get all the products of a specific order
g. get a specific itemOrder of a specific order and a specific product");
        //Accepting the user's choice
        choice = Console.ReadLine();
        int idOrderItem, idOrder;
        OrderItem orderItem;
        //
        try
        {
            switch (choice)
            {

                case "a":
                    {
                        orderItem = InputOrderItem();
                        Console.WriteLine(dalOrderItem.Add(orderItem));
                    }
                    break;

                case "b":
                    Console.WriteLine("enter the id orderItem");
                    idOrderItem = int.Parse(Console.ReadLine());
                    Console.WriteLine(dalOrderItem.Get(idOrderItem));
                    break;
                case "c":
                    OrderItem[] orderItems = dalOrderItem.GetAllOrderItems();
                    foreach (OrderItem currOrderItems in orderItems)
                    {
                        Console.WriteLine(currOrderItems);
                    }
                    break;

                case "d":
                    Console.WriteLine("enter id orderItem");
                    idOrderItem = int.Parse(Console.ReadLine());
                    Console.WriteLine("before: ");
                    Console.WriteLine(dalOrderItem.Get(idOrderItem));
                    orderItem = InputOrderItem();
                    orderItem.ID = idOrderItem;
                    if (orderItem.ID != null && orderItem.OrderId != null && orderItem.ProductId != null && orderItem.Price != null && orderItem.Amount != null)
                    {
                        dalOrderItem.Update(orderItem);
                        Console.WriteLine("after: ");
                        Console.WriteLine(dalOrderItem.Get(idOrderItem));
                    }

                    break;
                case "e":
                    Console.WriteLine("enter the id idOrderItem");
                    idOrderItem = int.Parse(Console.ReadLine());
                    dalOrderItem.Delete(idOrderItem);
                    break;
                case "f":
                    Console.WriteLine("enter the id order");
                    idOrder = int.Parse(Console.ReadLine());
                    OrderItem[] myOrderItems = dalOrderItem.GetByIdOrder(idOrder);
                    foreach (OrderItem currOrderItems in myOrderItems)
                    {
                        Console.WriteLine(currOrderItems);
                    }
                    break;
                case "g":
                    Console.WriteLine("enter the id order and id product");
                    idOrder = int.Parse(Console.ReadLine());
                    int idProduct = int.Parse(Console.ReadLine());
                    Console.WriteLine(dalOrderItem.Get(idOrder, idProduct));
                    break;


            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    private static OrderItem InputOrderItem()
    {
        int orderId, productId, amount;
        double price;

        Console.WriteLine("enter ProductId,orderId, price, Amount of orderItem");
        productId = int.Parse(Console.ReadLine());
        orderId = int.Parse(Console.ReadLine());
        price = double.Parse(Console.ReadLine());
        amount = int.Parse(Console.ReadLine());

        OrderItem ordIm = new OrderItem();
        ordIm.ProductId = productId;
        ordIm.Price = price;
        ordIm.Amount = amount;
        ordIm.OrderId = orderId;
        return ordIm;
    }
}

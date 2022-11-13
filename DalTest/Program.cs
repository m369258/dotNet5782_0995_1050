using Do;
using Dal;

class OurProgram
{
    private DalOrder dalOrder = new DalOrder();
    private DalProduct dalProduct = new DalProduct();
    private DalOrderItem dalOrderItem = new DalOrderItem();
    static void Main()
    {
        int choice;
        Console.WriteLine("enter your chioce: ");
        choice = int.Parse(Console.ReadLine());
        while (choice != 0)
        {

        }
    }

    private void submenuOfProduct()
    {
        string choice;
        //Print the checklist for the entity
        Console.WriteLine("enter your choice");
        Console.WriteLine("a. Option to add an object to an entity's list\r\n" +
            "b. Object display option by ID\r\n" +
            "c. Entity list view option\r\n" +
            "d. Option to update object data\r\n" +
            "e. Option to delete an object from an entity's list"
            );
        //Accepting the user's choice
        choice = Console.ReadLine();
        int idProduct;
        Product p;
        //
        try
        {
            switch (choice)
            {
                case "a":
                    {
                        p = InputProduct();
                        Console.WriteLine(dalProduct.Add(p));
                    }
                    break;

                case "b":
                    Console.WriteLine("enter the id product");
                    idProduct = int.Parse(Console.ReadLine());

                    Console.WriteLine(dalProduct.Get(idProduct));
                    break;
                case "c":
                    Product[] products = dalProduct.GetAllProducts();
                    for (int i = 0; i < products.Length; i++)
                    {
                        Console.WriteLine(products[i]);
                    }
                    break;
                case "d":
                    p = InputProduct();
                    dalProduct.Update(p);
                    break;
                case "e":
                    Console.WriteLine("enter the id product");
                    idProduct = int.Parse(Console.ReadLine());
                    dalProduct.Delete(idProduct);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private Product InputProduct()
    {
        int id, instock;
        double price;
        string name;
        Category category;
        Console.WriteLine("enter id, name, category, price, instock of product");
        id = int.Parse(Console.ReadLine());
        name = Console.ReadLine();
        category = (Category)int.Parse(Console.ReadLine());
        price = double.Parse(Console.ReadLine());
        instock = int.Parse(Console.ReadLine());

        Product p = new Product();
        p.ID = id;
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
    private void submenuOfOrder()
    {
        string choice;
        //Print the checklist for the entity
        Console.WriteLine("enter your choice");
        Console.WriteLine("a. Option to add an object to an entity's list\r\n" +
            "b. Object display option by ID\r\n" +
            "c. Entity list view option\r\n" +
            "d. Option to update object data\r\n" +
            "e. Option to delete an object from an entity's list"
            );
        //Accepting the user's choice
        choice = Console.ReadLine();

        int idOrder;
        Order myOrder;

        try
        {
            //
            switch (choice)
            {
                case "a":
                    {
                        myOrder = InputOrder();
                        Console.WriteLine(dalOrder.Add(myOrder));
                    }
                    break;

                case "b":
                    Console.WriteLine("enter the id order");
                    idOrder = int.Parse(Console.ReadLine());

                    Console.WriteLine(dalProduct.Get(idOrder));
                    break;
                case "c":
                    Order[] orders = dalOrder.GetAllOrders();
                    for (int i = 0; i < orders.Length; i++)
                    {
                        Console.WriteLine(orders[i]);
                    }
                    break;
                case "d":
                    myOrder = InputOrder();
                    dalOrder.Update(myOrder);
                    break;
                case "e":
                    Console.WriteLine("enter the id product");
                    idOrder = int.Parse(Console.ReadLine());
                    dalProduct.Delete(idOrder);
                    break;
            }
        }
        //In case of any error an appropriate error will be thrown
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private Order InputOrder()
    {
        Order order = new Order();
        int id;
        string customerName, customerEmail, customerAddress;
        DateTime orderDate, deliveryDate, shipDate;

        Console.WriteLine("enter id, customer name, customer email, customer address, order date , delivery date, ship date");
        id = int.Parse(Console.ReadLine());
        customerName = Console.ReadLine();
        customerEmail = Console.ReadLine();
        customerAddress = Console.ReadLine();
        orderDate = DateTime.Parse(Console.ReadLine());
        deliveryDate = DateTime.Parse(Console.ReadLine());
        shipDate = DateTime.Parse(Console.ReadLine());

        order.ID = id;
        order.CustomerName = customerName;
        order.CustomerEmail = customerEmail;
        order.CustomerAddress = customerAddress;
        order.OrderDate = orderDate;
        order.DeliveryDate = deliveryDate;
        order.ShipDate = shipDate;

        return order;
    }

    private void submenuOfOrderItem()
    {
        string choice;
        //Print the checklist for the entity
        Console.WriteLine("enter your choice:");
        Console.WriteLine("a. Option to add an object to an entity's list\r\n" +
            "b. Object display option by ID\r\n" +
            "c. Entity list view option\r\n" +
            "d. Option to update object data\r\n" +
            "e. Option to delete an object from an entity's list\r\n" +
             "f. Brings all the details of a specific order\r\n" +
            "g. Brings a specific item of a specific order"
            );
        //Accepting the user's choice
        choice = Console.ReadLine();
        int idOrderItem,idOrder;
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
                    for (int i = 0; i < orderItems.Length; i++)
                    {
                        Console.WriteLine(orderItems[i]);
                    }
                    break;

                case "d":
                    orderItem = InputOrderItem();
                    dalOrderItem.Update(orderItem);
                    break;
                case "e":
                    Console.WriteLine("enter the id idOrderItem");
                    idOrderItem = int.Parse(Console.ReadLine());
                    dalOrderItem.Delete(idOrderItem);
                    break;
                case "f":
                    Console.WriteLine("enter the id order");
                     idOrder = int.Parse(Console.ReadLine());
                    Console.WriteLine(dalOrderItem.GetByIdOrder(idOrder));

                    break;
                case "g":
                    Console.WriteLine("enter the id order and id product");
                    idOrder = int.Parse(Console.ReadLine());
                    int idProduct=int.Parse(Console.ReadLine());
                    Console.WriteLine(dalOrderItem.Get(idOrder, idProduct));
                    break;


            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    private OrderItem InputOrderItem()
    {
        int orderId, productId, amount;
        double price;
      
        Console.WriteLine("enter OrderId, ProductId, price, Amount of orderItem");
        orderId = int.Parse(Console.ReadLine());
        productId = int.Parse(Console.ReadLine());

        price = double.Parse(Console.ReadLine());
        amount = int.Parse(Console.ReadLine());

        OrderItem ordIm = new OrderItem();
        ordIm.OrderId = orderId;
        ordIm.ProductId = productId;
        ordIm.Price = price;
        ordIm.Amount = amount;

        return ordIm;
    }
}

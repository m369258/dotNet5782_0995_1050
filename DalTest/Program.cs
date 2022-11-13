﻿using Do;
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
            switch (choice)
            {
                case 1:
                   submenuOfProduct();
                    break;
                case 2:
                    submenuOfOrder();
                    break;
                case 3:
                    submenuOfOrderItem();
                    break;
                default:
                    break;
            }
        }
    }

    private static void submenuOfProduct()
    {
        string choice;
        DalProduct dalP = new DalProduct();
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
                    p = InputProduct();
                    Console.WriteLine(dalP.Add(p));
                    break;

                case "b":
                    Console.WriteLine("enter the id product");
                    idProduct = int.Parse(Console.ReadLine());

                    Console.WriteLine(dalP.Get(idProduct));
                    break;
                case "c":
                    Product[] products = dalP.GetAllProducts();
                    foreach (Product myProduct in products)
                    {
                        Console.WriteLine(myProduct);
                    }
                    break;
                case "d":
                    Console.WriteLine("enter id product");
                    idProduct = int.Parse(Console.ReadLine());
                    Console.WriteLine("before: ");
                    Console.WriteLine(dalP.Get(idProduct));
                    p = InputProduct();
                    p.ID = idProduct;
                    if (p.ID != null && p.Name != null && p.Category != null && p.InStock != null && p.Price != null)
                    {
                        dalP.Update(p);
                        Console.WriteLine("after: ");
                        Console.WriteLine(dalP.Get(idProduct));
                    }
                    break;
                case "e":
                    Console.WriteLine("enter the id product");
                    idProduct = int.Parse(Console.ReadLine());
                    dalP.Delete(idProduct);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static Product InputProduct()
    {
        int instock;
        double price;
        string name;
        Category category;
        Console.WriteLine("enter name, category, price, instock of product");
        name = Console.ReadLine();
        category = (Category)int.Parse(Console.ReadLine());
        price = double.Parse(Console.ReadLine());
        instock = int.Parse(Console.ReadLine());

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
        DalOrder dalO = new DalOrder();
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
            //Checking what action the user wants to run
            switch (choice)
            {
                case "a":
                    {
                        //A call to an action that receives data for an order object
                        myOrder = InputOrder();
                        //A call to action that adds an order to the system
                        Console.WriteLine(dalO.Add(myOrder));
                    }
                    break;

                case "b":
                    //Receipt of order ID number
                    Console.WriteLine("enter the id order");
                    idOrder = int.Parse(Console.ReadLine());

                    Console.WriteLine(dalO.Get(idOrder));
                    break;
                case "c":
                    Order[] orders = dalO.GetAllOrders();
                    foreach (Order currOrder in orders)
                    {
                        Console.WriteLine(currOrder);
                    }
                    break;
                case "d":
                    Console.WriteLine("enter id order");
                    idOrder = int.Parse(Console.ReadLine());
                    Console.WriteLine("before: ");
                    Console.WriteLine(dalO.Get(idOrder));
                    myOrder = InputOrder();
                    myOrder.ID = idOrder;
                    if (myOrder.ID != null && myOrder.CustomerName != null && myOrder.CustomerEmail != null && myOrder.CustomerAddress != null &&
                        myOrder.OrderDate != null && myOrder.DeliveryDate != null && myOrder.ShipDate != null)
                    {
                        dalO.Update(myOrder);
                        Console.WriteLine("after: ");
                        Console.WriteLine(dalO.Get(idOrder));
                    }
                    break;
                case "e":
                    Console.WriteLine("enter the id product");
                    idOrder = int.Parse(Console.ReadLine());
                    dalO.Delete(idOrder);
                    break;
            }
        }
        //In case of any error an appropriate error will be thrown
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static Order InputOrder()
    {
        Order order = new Order();
        string customerName, customerEmail, customerAddress;
        DateTime orderDate, deliveryDate, shipDate;

        Console.WriteLine("enter customer name, customer email, customer address, order date , delivery date, ship date");
        customerName = Console.ReadLine();
        customerEmail = Console.ReadLine();
        customerAddress = Console.ReadLine();
        orderDate = DateTime.Parse(Console.ReadLine());
        deliveryDate = DateTime.Parse(Console.ReadLine());
        shipDate = DateTime.Parse(Console.ReadLine());

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
        DalOrderItem daloI = new DalOrderItem();
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
                        Console.WriteLine(daloI.Add(orderItem));
                    }
                    break;

                case "b":
                    Console.WriteLine("enter the id orderItem");
                    idOrderItem = int.Parse(Console.ReadLine());
                    Console.WriteLine(daloI.Get(idOrderItem));
                    break;
                case "c":
                    OrderItem[] orderItems = daloI.GetAllOrderItems();
                    foreach (OrderItem currOrderItems in orderItems)
                    {
                        Console.WriteLine(currOrderItems);
                    }
                    break;

                case "d":
                    Console.WriteLine("enter id orderItem");
                    idOrderItem = int.Parse(Console.ReadLine());
                    Console.WriteLine("before: ");
                    Console.WriteLine(daloI.Get(idOrderItem));
                    orderItem = InputOrderItem();
                    orderItem.ID = idOrderItem;
                    if (orderItem.ID != null && orderItem.OrderId != null && orderItem.ProductId != null && orderItem.Price != null && orderItem.Amount != null)
                    {
                        daloI.Update(orderItem);
                        Console.WriteLine("after: ");
                        Console.WriteLine(daloI.Get(idOrderItem));
                    }

                    break;
                case "e":
                    Console.WriteLine("enter the id idOrderItem");
                    idOrderItem = int.Parse(Console.ReadLine());
                    daloI.Delete(idOrderItem);
                    break;
                case "f":
                    Console.WriteLine("enter the id order");
                    idOrder = int.Parse(Console.ReadLine());
                    Console.WriteLine(daloI.GetByIdOrder(idOrder));

                    break;
                case "g":
                    Console.WriteLine("enter the id order and id product");
                    idOrder = int.Parse(Console.ReadLine());
                    int idProduct = int.Parse(Console.ReadLine());
                    Console.WriteLine(daloI.Get(idOrder, idProduct));
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

        Console.WriteLine("enter ProductId, price, Amount of orderItem");
        productId = int.Parse(Console.ReadLine());

        price = double.Parse(Console.ReadLine());
        amount = int.Parse(Console.ReadLine());

        OrderItem ordIm = new OrderItem();
        ordIm.ProductId = productId;
        ordIm.Price = price;
        ordIm.Amount = amount;

        return ordIm;
    }
}

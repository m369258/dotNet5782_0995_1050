﻿using Do;
using Dal;
using DalApi;
enum MainMenu { Exist = 0, Product, Order, OrderItem }
enum Options { Add = 1, Get, GetAll, Update, Delete, GetByIDOrder, GetByIDOrderAndIDProduct }
class OurProgram
{
  static DalList1 myDalList=new DalList1();
    ///// <summary>
    ///// A class variable for each entity and entity
    ///// </summary>
    //private static DalOrder dalOrder = new DalOrder();
    //private static DalProduct dalProduct = new DalProduct();
    //private static DalOrderItem dalOrderItem = new DalOrderItem();
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
1: add an product
2: get a product by ID
3: gethhhhhhhhhhhhhhhhhh all products 
4: update an product
5: delete an product ");
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
                    Console.WriteLine(myDalList.product.Add(p));
                    Console.WriteLine("Product whose number:{0} has been successfully added", idProduct);
                    break;

                case Options.Get:
                    Console.WriteLine("Receiving a number by the ID");
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    Console.WriteLine(myDalList.product.Get(idProduct));
                    break;

                case Options.GetAll:
                    IEnumerable<Product> products = myDalList.product.GetAll();
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
                    Console.WriteLine(myDalList.product.Get(idProduct));
                    p = InputProduct();
                    p.ID = idProduct;
                    //will update the product only if all the details have been verified
                    if (p.ID != null && p.Name != null && p.Category != null && p.InStock != null && p.Price != null)
                    {
                        myDalList.product.Update(p);
                        Console.WriteLine("The requested product after the change");
                        Console.WriteLine(myDalList.product.Get(idProduct));
                    }
                    break;

                case Options.Delete:
                    Console.WriteLine("Product to be deleted");
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    myDalList.product.Delete(idProduct);
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
        Console.WriteLine(@"enter name,
category- 
for cupcakes insert 1
for cakes insert 2
for macarons insert 3
for sweets insert 4
for ballons insert 5,
price, 
instock of product");
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
1: add an order
2: get an order by ID
3: get all orders
4: update an order
5: delete an order");
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
                    Console.WriteLine(myDalList.order.Add(myOrder));
                    break;

                case Options.Get:
                    //Receipt of order ID number
                    Console.WriteLine("enter the id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    Console.WriteLine(myDalList.order.Get(idOrder));
                    break;

                case Options.GetAll:
                    IEnumerable<Order> orders = myDalList.order.GetAll();
                    foreach (Order currOrder in orders)
                    {
                        Console.WriteLine(currOrder);
                    }
                    break;

                case Options.Update:
                    Console.WriteLine("enter id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    Console.WriteLine("before: ");
                    Console.WriteLine(myDalList.order.Get(idOrder));
                    myOrder = InputOrder();
                    myOrder.ID = idOrder;
                    //In the event that all order details have not been provided, this order will not be updated.
                    if (myOrder.ID != null && myOrder.CustomerName != null && myOrder.CustomerEmail != null && myOrder.CustomerAddress != null &&
                        myOrder.OrderDate != null && myOrder.DeliveryDate != null && myOrder.ShipDate != null)
                    {
                        myDalList.order.Update(myOrder);
                        Console.WriteLine("after: ");
                        Console.WriteLine(myDalList.order.Get(idOrder));
                    }
                    break;

                case Options.Delete:
                    Console.WriteLine("enter the id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    myDalList.order.Delete(idOrder);
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
    /// <summary>
    /// Sub menu for order details
    /// </summary>
    /// <exception cref="Exception">If one of the details entered by the user is incorrect, an error will be thrown</exception>
    private static void submenuOfOrderItem()
    {
        Options choice;
        //Print the checklist for the entity
        Console.WriteLine(@"enter your choice:
1: add an orderItem
2: get an orderItem by ID
3: get all orderItems
4: update an orderItem
5: delete an orderItem
6: get all the products of a specific order
7. get a specific orderItem of a specific order and a specific product");
        //Accepting the user's choice
        if (!Options.TryParse(Console.ReadLine(), out choice)) throw new Exception("choice is in valid");

        int idOrderItem, idOrder;
        OrderItem orderItem;

        try
        {
            switch (choice)
            {

                case Options.Add:
                    {
                        Console.WriteLine("Item on order to be added");
                        orderItem = InputOrderItem();
                        Console.WriteLine(myDalList.orderItems.Add(orderItem));
                        Console.WriteLine("Order item whose number {0} has been successfully added", orderItem.ID);
                    }
                    break;

                case Options.Get:
                    Console.WriteLine("Item display on order according to the user's request");
                    Console.WriteLine("enter the id orderItem");
                    if (!int.TryParse(Console.ReadLine(), out idOrderItem)) throw new Exception("idOrderItem is in valid");
                    Console.WriteLine(myDalList.orderItems.Get(idOrderItem));
                    break;

                case Options.GetAll:
                    Console.WriteLine("Displaying all existing order details");
                    IEnumerable<OrderItem> orderItems = myDalList.orderItems.GetAll();
                    //Printing all existing order details
                    foreach (OrderItem currOrderItems in orderItems)
                    {
                        Console.WriteLine(currOrderItems);
                    }
                    break;

                case Options.Update:
                    Console.WriteLine("Order item update requested");
                    Console.WriteLine("enter id orderItem");
                    if (!int.TryParse(Console.ReadLine(), out idOrderItem)) throw new Exception("idOrderItem is in valid");
                    Console.WriteLine("Item on order before change");
                    Console.WriteLine(myDalList.orderItems.Get(idOrderItem));
                    orderItem = InputOrderItem();
                    orderItem.ID = idOrderItem;
                    //Update an item in the order only if all the details have been entered by the user
                    if (orderItem.ID != null && orderItem.OrderId != null && orderItem.ProductId != null && orderItem.Price != null && orderItem.Amount != null)
                    {
                        myDalList.orderItems.Update(orderItem);
                        Console.WriteLine("Item on order after the change");
                        Console.WriteLine(myDalList.orderItems.Get(idOrderItem));
                    }
                    break;

                case Options.Delete:
                    Console.WriteLine("Deletion of an item in a requested order");
                    Console.WriteLine("enter the id idOrderItem");
                    if (!int.TryParse(Console.ReadLine(), out idOrderItem)) throw new Exception("idOrderItem is in valid");
                    myDalList.orderItems.Delete(idOrderItem);
                    Console.WriteLine("An item in the order whose number {0} has been successfully deleted", idOrderItem);
                    break;

                case Options.GetByIDOrder:
                    Console.WriteLine("Display all products of a specific order");
                    Console.WriteLine("enter the id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("idOrder is in valid");
                  IEnumerable<OrderItem> myOrderItems = myDalList.orderItems.GetByIdOrder(idOrder);
                    Console.WriteLine("Printing all products of a specific order");
                    foreach (OrderItem currOrderItems in myOrderItems)
                    {
                        Console.WriteLine(currOrderItems);
                    }
                    break;

                case Options.GetByIDOrderAndIDProduct:
                    Console.WriteLine("Displaying a specific item in a specific order");
                    Console.WriteLine("enter the id order and id product");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("idOrder is in valid");
                    int idProduct;
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    Console.WriteLine(myDalList.orderItems.Get(idOrder, idProduct));
                    break;

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    /// <summary>
    /// Receiving details of a product ordered by the user
    /// </summary>
    /// <returns>Returns an item in the order with the data captured from the user</returns>
    /// <exception cref="Exception">If one of the receptions was not successful</exception>
    private static OrderItem InputOrderItem()
    {
        int orderId, productId, amount;
        double price;
        Console.WriteLine("enter ProductId,orderId, price, Amount of orderItem");
        if (!int.TryParse(Console.ReadLine(), out productId)) throw new Exception("productId is in valid");
        if (!int.TryParse(Console.ReadLine(), out orderId)) throw new Exception("orderId is in valid");
        if (!double.TryParse(Console.ReadLine(), out price)) throw new Exception("price is in valid");
        if (!int.TryParse(Console.ReadLine(), out amount)) throw new Exception("amount is in valid");
        OrderItem ordIm = new OrderItem();
        ordIm.ProductId = productId;
        ordIm.Price = price;
        ordIm.Amount = amount;
        ordIm.OrderId = orderId;
        return ordIm;
    }
}

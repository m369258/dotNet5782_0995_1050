using BlApi;
using BO;

namespace BlTest;

internal class Program
{
    private static IBl myBL = new Bl();
    enum MainMenu { Exist = 0, Product, Order, Cart }
    enum OptionsOfProducts { Add = 1, Get, GetAll, Update, Delete, GetByIDAndCart }
    enum OptionsOfOrders { GetListOfOrders = 1, OrderShippingUpdate, GetOrderDetails, OrderDeliveryUpdate, OrderTracking }
    enum OptionsOfCarts { Add = 1, Update, MakeAnOrder }

    /// <summary>
    /// A private variable that stores the current shopping cart
    /// </summary>
    private static BO.Cart boCart = new BO.Cart()
    {
        CustomerName = "shira nussbacher",
        CustomerAddress = "baal atania 20 bb",
        CustomerEmail = "shira6557@gmail.com",
        items = new List<BO.OrderItem>(),
        TotalPrice = 0,
    };

    /// <summary>
    /// A main program that manages user requests
    /// </summary>
    static void Main(string[] args)
    {
        MainMenu choice;
        //Receiving a voluntary action number to perform
        Console.WriteLine(@"please choose one of the following:
0: exit
1: prodeuct
2: order
3: cart");
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
                case MainMenu.Cart:
                    submenuOfCart();
                    break;
                default:
                    break;
            }
            Console.WriteLine(@"please choose one of the following:
0: exit
1: prodeuct
2: order
3: cart");
            choice = (MainMenu)int.Parse(Console.ReadLine());
        }
    }
    private static void submenuOfProduct()
    {
        OptionsOfProducts choice;
        //Print the checklist for the entity
        Console.WriteLine(@"enter your choice:
1: add an product
2: get a product by ID
3: get all products 
4: update an product
5: delete an product 
6:get a product by ID and Cart");
        //Accepting the user's choice
        if (!OptionsOfProducts.TryParse(Console.ReadLine(), out choice)) throw new InvalidInputException("choice is in valid");

        //Declaration of a logical layer item variable
        int idProduct;
        BO.Product p = new BO.Product();

        try
        {
            switch (choice)
            {
                case OptionsOfProducts.Add:
                    Console.WriteLine("Adding a product");
                    Console.Write("enter idProduct with 6 numbers:");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new InvalidInputException("idProduct is in valid");
                    //Calling a function that receives item details from the user
                    p = InputProduct();
                    p.ID = idProduct;
                    myBL.product.AddProduct(p);
                    Console.WriteLine("Product whose number:{0} has been successfully added", idProduct);
                    break;

                case OptionsOfProducts.Get:
                    Console.WriteLine("Receiving a number by the ID");
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new InvalidInputException("idProduct is in valid");
                    Console.WriteLine(myBL.product.GetProduct(idProduct));
                    break;

                case OptionsOfProducts.GetAll:
                    //Requests all items logged and printed
                    IEnumerable<BO.ProductForList> products = myBL.product.GetListOfProducts();
                    foreach (var myProduct in products)
                    {
                        Console.WriteLine(myProduct);
                    }
                    break;

                case OptionsOfProducts.Update:
                    Console.WriteLine("Product update:");
                    Console.WriteLine("enter id product:");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new InvalidInputException("idProduct is in valid");
                    Console.WriteLine("The requested product before the change");
                    Console.WriteLine(myBL.product.GetProduct(idProduct));
                    //Calling a function that receives item details from the user
                    p = InputProduct();
                    p.ID = idProduct;
                    myBL.product.UpDateProduct(p);
                    Console.WriteLine("The requested product after the change");
                    Console.WriteLine(myBL.product.GetProduct(idProduct));
                    break;

                case OptionsOfProducts.Delete:
                    Console.WriteLine("Product to be deleted");
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new InvalidInputException("idProduct is in valid");
                    myBL.product.DeleteProduct(idProduct);
                    break;

                case OptionsOfProducts.GetByIDAndCart:
                    Console.WriteLine("Requests an item by item ID and basket");
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new InvalidInputException("idProduct is in valid");
                    Console.WriteLine(myBL.product.GetProduct(idProduct, boCart));
                    break;

            }
        }
        catch (InvalidInputException invalidEx){Console.WriteLine(invalidEx);}
        catch (InvalidArgumentException invalidArg) { Console.WriteLine(invalidArg); }
        catch(InternalErrorException internalEx) { Console.WriteLine(internalEx); }
        catch (Exception ex){Console.WriteLine(ex.Message);}
    }

    /// <summary>
    /// Receives the item data from the user
    /// </summary>
    /// <returns>Receives the item data from the user</returns>
    /// <exception cref="Exception">In case of incorrect input</exception>
    private static BO.Product InputProduct()
    {
        int instock;
        double price;
        string name;
        BO.Category category;
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
        if (!BO.Category.TryParse(Console.ReadLine(), out category)) throw new InvalidInputException("category is in valid");
        if (!double.TryParse(Console.ReadLine(), out price)) throw new InvalidInputException("price is in valid");
        if (!int.TryParse(Console.ReadLine(), out instock)) throw new InvalidInputException("inStock is in valid");
        BO.Product p = new BO.Product();
        p.Name = name;
        p.Category = category;
        p.Price = price;
        p.InStock = instock;
        return p;
    }

    /// <summary>
    /// A sub-menu of the order - which performs actions according to the user's request
    /// </summary>
    /// <exception cref="InvalidInputException">An appropriate error will be thrown if necessary</exception>
    private static void submenuOfOrder()
    {
        OptionsOfOrders choice;
        //Print the checklist for the entity
        Console.WriteLine(@"enter your choice:
1: get list of orders
2: order shipping update
3: get order details
4: order delivery update
5: order tracking");
        //Accepting the user's choice
        if (!OptionsOfOrders.TryParse(Console.ReadLine(), out choice)) throw new InvalidInputException("choice is invalid");
        int idOrder;
        BO.Order myOrder;

        try
        {
            BO.Order boOrder;
            //Checking what action the user wants to run
            switch (choice)
            {
                //Order list request
                case OptionsOfOrders.GetListOfOrders:
                    //A call to action that returns the list of orders
                    IEnumerable<BO.OrderForList> orderForLists = myBL.order.GetListOfOrders();
                    //Printing the order for list
                    foreach (BO.OrderForList currOrderForList in orderForLists)
                    {
                        Console.WriteLine(currOrderForList);
                    }
                    break;

              

                //Order shipping update
                case OptionsOfOrders.OrderShippingUpdate:
                    Console.WriteLine("enter id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new InvalidInputException("id product is invalid");
                    Console.WriteLine("before: ");
                    Console.WriteLine(myBL.order.GetOrderDetails(idOrder));
                    boOrder = myBL.order.OrderShippingUpdate(idOrder);
                    Console.WriteLine("after: ");
                    Console.WriteLine(boOrder);
                    //Console.WriteLine(myBL.order.GetOrderDetails(idOrder));?????????????????????????????????????
                    break;

                //Order details request
                case OptionsOfOrders.GetOrderDetails:
                    //Receipt of order ID number
                    Console.WriteLine("enter the id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new InvalidInputException("id product is invalid");
                    //A call to action that returns the order details
                    Console.WriteLine(myBL.order.GetOrderDetails(idOrder));
                    break; 

                //Order delivery update
                case OptionsOfOrders.OrderDeliveryUpdate:
                    Console.WriteLine("enter id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new InvalidInputException("id product is invalid");
                    Console.WriteLine("before: ");
                    Console.WriteLine(myBL.order.GetOrderDetails(idOrder));
                    boOrder = myBL.order.OrderDeliveryUpdate(idOrder);
                    Console.WriteLine("after: ");
                    Console.WriteLine(myBL.order.GetOrderDetails(idOrder));
                    break;

                //Order Tracking
                case OptionsOfOrders.OrderTracking:
                    Console.WriteLine("enter id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new InvalidInputException("id product is invalid");
                    Console.WriteLine(myBL.order.OrderTracking(idOrder));
                    break;
            }
        }
        //In case of any error an appropriate error will be thrown
        catch (InvalidInputException invalidEx) { Console.WriteLine(invalidEx); }
        catch (InvalidArgumentException invalidArg) { Console.WriteLine(invalidArg); }
        catch (InternalErrorException internalEx) { Console.WriteLine(internalEx); }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
    }

    /// <summary>
    /// A sub-menu of the shopping basket - which performs actions according to the user's request
    /// </summary>
    /// <exception cref="InvalidInputException">An appropriate error will be thrown if necessary</exception>
    private static void submenuOfCart()
    {
        OptionsOfCarts choice;
        //Print the checklist for the entity
        Console.WriteLine(@"enter your choice:
1: Adding a product to the shopping cart
2: update a cart
3: make an order
");
        //Accepting the user's choice
        if (!OptionsOfCarts.TryParse(Console.ReadLine(), out choice)) throw new InvalidInputException("choice is invalid");

        try
        {
            int idProduct;
            int newQuantity;
            //Checking what action the user wants to run
            switch (choice)
            {
                //in case of adding product to the cart
                case OptionsOfCarts.Add:
                    //the user will insert id product
                    Console.WriteLine("Adding a product to the cart");
                    Console.Write("enter idProduct with 6 numbers:");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new InvalidInputException("idProduct is in valid");
                    //A call to action that adds a product to the shopping cart
                    Console.WriteLine(myBL.cart.Add(boCart, idProduct));
                    break;

                //Shopping cart update
                case OptionsOfCarts.Update:
                    //Receives a new product and quantity from the user and updates the shopping basket accordingly.
                    Console.WriteLine("enter id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new InvalidInputException("id product is invalid");
                    Console.WriteLine("enter new quientity");
                    if (!int.TryParse(Console.ReadLine(), out newQuantity)) throw new InvalidInputException("quantity is invalid");
                    //A call to the function that updates the shopping cart.
                    Console.WriteLine(myBL.cart.Update(boCart, idProduct, newQuantity));
                    Console.WriteLine("the cart updated");
                    break;

                //make an order
                case OptionsOfCarts.MakeAnOrder:
                    //A call to action that executes the order
                    myBL.cart.MakeAnOrder(boCart);
                    Console.WriteLine("the order made");
                    break;
            }
        }
        //In case of any error an appropriate error will be thrown
        catch (InvalidInputException invalidEx) { Console.WriteLine(invalidEx); }
        catch (InvalidArgumentException invalidArg) { Console.WriteLine(invalidArg); }
        catch (InternalErrorException internalEx) { Console.WriteLine(internalEx); }
        catch (NotEnoughInStockException notEnoughInStock) { Console.WriteLine(notEnoughInStock); }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
    }
}
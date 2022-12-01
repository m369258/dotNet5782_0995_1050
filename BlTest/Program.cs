using BlApi;
namespace BlTest;

internal class Program
{
    private static IBl myBL = new Bl();

    enum MainMenu { Exist = 0, Product, Order, Cart }
    enum OptionsOfProducts { Add = 1, Get, GetAll, Update, Delete, GetByIDAndCart }
    enum OptionsOfOrders { GetListOfOrders = 1, OrderShippingUpdate, GetOrderDetails, OrderDeliveryUpdate, OrderTracking }
    enum OptionsOfCarts { Add = 1, Update, MakeAnOrder }

    private static BO.Cart boCart = new BO.Cart()
    {
        CustomerName = "shira nussbacher",
        CustomerAddress = "baal atania 20 bb",
        CustomerEmail = "shira6557@gmail.com",
        items = new List<BO.OrderItem>(),
        TotalPrice = 0,
    };

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
        if (!OptionsOfProducts.TryParse(Console.ReadLine(), out choice)) throw new Exception("choice is in valid");

        int idProduct;
        BO.Product p = new BO.Product();

        try
        {
            switch (choice)
            {
                case OptionsOfProducts.Add:
                    Console.WriteLine("Adding a product");
                    Console.Write("enter idProduct with 6 numbers:");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    p = InputProduct();
                    p.ID = idProduct;
                    myBL.product.AddProduct(p);
                    Console.WriteLine("Product whose number:{0} has been successfully added", idProduct);
                    break;

                case OptionsOfProducts.Get:
                    Console.WriteLine("Receiving a number by the ID");
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    Console.WriteLine(myBL.product.GetProduct(idProduct));
                    break;

                case OptionsOfProducts.GetAll:
                    IEnumerable<BO.ProductForList> products = myBL.product.GetListOfProducts();
                    foreach (var myProduct in products)
                    {
                        Console.WriteLine(myProduct);
                    }
                    break;

                case OptionsOfProducts.Update:
                    Console.WriteLine("Product update:");
                    Console.WriteLine("enter id product:");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    Console.WriteLine("The requested product before the change");
                    Console.WriteLine(myBL.product.GetProduct(idProduct));
                    p = InputProduct();
                    p.ID = idProduct;
                    //will update the product only if all the details have been verified
                    myBL.product.UpDateProduct(p);
                    Console.WriteLine("The requested product after the change");
                    Console.WriteLine(myBL.product.GetProduct(idProduct));
                    break;

                case OptionsOfProducts.Delete:
                    Console.WriteLine("Product to be deleted");
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    myBL.product.DeleteProduct(idProduct);
                    break;

                case OptionsOfProducts.GetByIDAndCart:
                    Console.WriteLine("enter the id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    BO.Cart boCart = InputCart();
                    Console.WriteLine(myBL.product.GetProduct(idProduct, boCart));
                    break;

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
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
        if (!BO.Category.TryParse(Console.ReadLine(), out category)) throw new Exception("category is in valid");
        if (!double.TryParse(Console.ReadLine(), out price)) throw new Exception("price is in valid");
        if (!int.TryParse(Console.ReadLine(), out instock)) throw new Exception("inStock is in valid");
        BO.Product p = new BO.Product();
        p.Name = name;
        p.Category = category;
        p.Price = price;
        p.InStock = instock;
        return p;
    }
    private static BO.Cart InputCart()
    {
        string CustomerName, CustomerEmail, CustomerAddress;
        List<BO.OrderItem> items = new List<BO.OrderItem>();
        BO.OrderItem boOrderItem = new BO.OrderItem();
        double price;
        Console.WriteLine(@"enter CustomerName,
enter CustomerEmail,
enter CustomerAddress,
add order items till enter 0,
enter price");
        CustomerName = Console.ReadLine();
        CustomerEmail = Console.ReadLine();
        CustomerAddress = Console.ReadLine();

        Console.WriteLine("enter 1 to add OrderItem and 0 to stop adding:");
        int addOrderItems = int.Parse(Console.ReadLine());
        while (addOrderItems != 0)
        {
            items.Add(InputOrderItem());
            Console.WriteLine("enter 1 to add OrderItem and 0 to stop adding:");
            addOrderItems = int.Parse(Console.ReadLine());
        }
        if (!double.TryParse(Console.ReadLine(), out price)) throw new Exception("price is in valid");
        BO.Cart p = new BO.Cart
        {
            CustomerName = CustomerName,
            CustomerEmail = CustomerEmail,
            CustomerAddress = CustomerAddress,
            items = items,
            TotalPrice = price
        };
        return p;
    }
    private static BO.OrderItem InputOrderItem()
    {
        int idProduct, quantityPerItem;
        string nameProduct;
        double productPrice, totalPrice;
        Console.WriteLine(@"enter idProduct,
enter nameProduct,
enter productPrice,
enter quantityPerItem,
enter totalPrice");
        if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("id is in valid");

        nameProduct = Console.ReadLine();
        if (!double.TryParse(Console.ReadLine(), out productPrice)) throw new Exception("productPrice is in valid");
        if (!int.TryParse(Console.ReadLine(), out quantityPerItem)) throw new Exception("quantityPerItem is in valid");
        if (!double.TryParse(Console.ReadLine(), out totalPrice)) throw new Exception("totalPrice is in valid");

        BO.OrderItem boOrderItem = new BO.OrderItem
        {
            ProductId = idProduct,
            QuantityPerItem = quantityPerItem,
            NameProduct = nameProduct,
            productPrice = productPrice,
            TotalPrice = totalPrice
        };
        return boOrderItem;
    }
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
        if (!OptionsOfOrders.TryParse(Console.ReadLine(), out choice)) throw new Exception("choice is invalid");
        int idOrder;
        BO.Order myOrder;

        try
        {
            BO.Order boOrder;
            //Checking what action the user wants to run
            switch (choice)
            {

                case OptionsOfOrders.GetListOfOrders:
                    IEnumerable<BO.OrderForList> orderForLists = myBL.order.GetListOfOrders();
                    foreach (BO.OrderForList currOrderForList in orderForLists)
                    {
                        Console.WriteLine(currOrderForList);
                    }
                    break;
                //עדכון שילוח הזמנה
                case OptionsOfOrders.OrderShippingUpdate:
                    Console.WriteLine("enter id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    Console.WriteLine("before: ");
                    Console.WriteLine(myBL.order.GetOrderDetails(idOrder));
                    boOrder = myBL.order.OrderShippingUpdate(idOrder);
                    Console.WriteLine("after: ");
                    Console.WriteLine(myBL.order.GetOrderDetails(idOrder));
                    break;

                case OptionsOfOrders.GetOrderDetails:
                    //Receipt of order ID number
                    Console.WriteLine("enter the id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    Console.WriteLine(myBL.order.GetOrderDetails(idOrder));
                    break;
                //עדכון אספק, הזמנה
                case OptionsOfOrders.OrderDeliveryUpdate:
                    Console.WriteLine("enter id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    Console.WriteLine("before: ");
                    Console.WriteLine(myBL.order.GetOrderDetails(idOrder));
                    boOrder = myBL.order.OrderDeliveryUpdate(idOrder);
                    Console.WriteLine("after: ");
                    Console.WriteLine(myBL.order.GetOrderDetails(idOrder));
                    break;

                case OptionsOfOrders.OrderTracking:
                    Console.WriteLine("enter id order");
                    if (!int.TryParse(Console.ReadLine(), out idOrder)) throw new Exception("id product is invalid");
                    Console.WriteLine(myBL.order.OrderTracking(idOrder));
                    //Console.WriteLine(myBL.order.order(idOrder));
                    break;
            }
        }
        //In case of any error an appropriate error will be thrown
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
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
        if (!OptionsOfCarts.TryParse(Console.ReadLine(), out choice)) throw new Exception("choice is invalid");

        try
        {
            int idProduct;
            int newQuantity;
            //Checking what action the user wants to run
            switch (choice)
            {
                case OptionsOfCarts.Add:
                    Console.WriteLine("Adding a product to the cart");
                    Console.Write("enter idProduct with 6 numbers:");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("idProduct is in valid");
                    Console.WriteLine(myBL.cart.Add(boCart, idProduct));
                    break;


                case OptionsOfCarts.Update:
                    Console.WriteLine("enter id product");
                    if (!int.TryParse(Console.ReadLine(), out idProduct)) throw new Exception("id product is invalid");
                    Console.WriteLine("enter new quientity");
                    if (!int.TryParse(Console.ReadLine(), out newQuantity)) throw new Exception("quantity is invalid");
                    Console.WriteLine(myBL.cart.Update(boCart, idProduct, newQuantity));
                    Console.WriteLine("the cart updated");
                    break;

                case OptionsOfCarts.MakeAnOrder:
                    myBL.cart.MakeAnOrder(boCart);
                    Console.WriteLine("the order made");
                    break;
            }
        }
        //In case of any error an appropriate error will be thrown
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

}
using BlApi;
using Dal;
using DalApi;

namespace BlTest;

internal class Program
{
    private static IBl myBL = new Bl();

    enum MainMenu { Exist = 0, Product, Order, Cart }
    enum OptionsOfProducts { Add = 1, Get, GetAll, Update, Delete, GetByIDOrder, GetByIDOrderAndIDProduct }
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
5: delete an product ");
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
                    Console.WriteLine(myBL.product.Get(idProduct));
                    break;

                case OptionsOfProducts.GetAll:
                    IEnumerable<BO.Product> products = myBL.product.GetAll();
                    foreach (BO.Product myProduct in products)
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
                    myDalList.product.Update(p);
                    Console.WriteLine("The requested product after the change");
                    Console.WriteLine(myDalList.product.Get(idProduct));
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

    private static void submenuOfOrder()
    {

    }

    private static void submenuOfCart()
    {

    }


}
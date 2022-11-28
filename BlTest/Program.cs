using BlApi;
using Dal;
using DalApi;

namespace BlTest;

internal class Program
{
    enum MainMenu { Exist = 0, Product, Order, Cart }
    enum Options { Add = 1, Get, GetAll, Update, Delete, GetByIDOrder, GetByIDOrderAndIDProduct }
    static void Main(string[] args)
    {
            //private static IBl myBL;

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
3: get all products 
4: update an product
5: delete an product ");
        //Accepting the user's choice
        if (!Options.TryParse(Console.ReadLine(), out choice)) throw new Exception("choice is in valid");

        int idProduct;
       BO. Product p = new BO.Product();

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

    private static void submenuOfOrder()
    {

    }

    private static void submenuOfCart()
    {

    }


}
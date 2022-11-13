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
                        p = Input();
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
                    p = Input();
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

    private Product Input()
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
    private void submenuOfOrder()
    {
        //Print the checklist for the entity

    }

    private void submenuOfOrderItem()
    {
        //Print the checklist for the entity

    }
}

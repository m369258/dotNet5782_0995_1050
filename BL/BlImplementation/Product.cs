﻿namespace BlImplementation;

internal class Product : BlApi.IProduct
{
    // DalApi.IDal dalProduct = new Dal.DalList1();
    public DalApi.IDal myDal { get; set; }
    IEnumerable<BO.ProductForList> BlApi.IProduct.GetListOfProducts()
    {
        List<BO.ProductForList> ListProducts = new List<BO.ProductForList>();
        foreach (var item in myDal.product.GetAll())
        {
            BO.ProductForList product = new BO.ProductForList
            {
                ID = item.ID,
                Name = item.Name,
                Price = item.Price,
                category = (BO.Category)(item.Category)
            };
            ListProducts.Add(product);
        }
        return ListProducts;
    }

    public BO.Product GetProduct(int idProduct)
    {
        if (idProduct > 0)
        {
            try
            {
                BO.Product product = new BO.Product();
                Do.Product p1 = myDal.product.Get(idProduct);
                product.ID = p1.ID;
                product.Name = p1.Name;
                product.Price = p1.Price;
                product.InStock = p1.InStock;
                product.Category = (BO.Category)(p1.Category);
                return product;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        else
            throw new BO.IncorrectIndex("Negative ID");
    }


    public BO.ProductItem GetProduct(int idProduct, BO.Cart cart)
    {
        if (idProduct > 0)
        {
            try
            {
                Do.Product p1 = myDal.product.Get(idProduct);
                BO.ProductItem product = new BO.ProductItem { 
                ProductID = p1.ID,
                    Price = p1.Price,
                    Name = p1.Name,
                    Category = (BO.Category)(p1.Category),
                    InStock = p1.InStock > 0 ? true : false,
                    Amount = p1.InStock };
                return product;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        else
            throw new BO.IncorrectIndex("Negative ID");
    }

    public void AddProduct(BO.Product product)
    {
        Do.Product p1 = new Do.Product {
            ID = product.ID > 0 ? product.ID : throw new BO.IncorrectIndex(""),
        Name = product.Name != null ? product.Name : throw new BO.InvalidField("Empty name"),
        Price = product.Price >= 0 ? product.Price : throw new BO.InvalidField("Negative price"),
        InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidField("Unfavorable inStock"),
        Category = (Do.Category)(product.Category)};
        try
        {
            myDal.product.Add(p1);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }


    public void DeleteProduct(int idProduct)
    {
        IEnumerable<Do.Order> orders=new List<Do.Order>();
        IEnumerable<Do.OrderItem> orderItems = new List<Do.OrderItem>();
        try
        {
            orders = myDal.order.GetAll();
            foreach (var order in orders)
            {
                orderItems = myDal.orderItems.GetByIdOrder(order.ID);
                foreach (var item in orderItems)
                {

                    if (item.ProductId == idProduct)
                        throw new BO.CannotPerformThisOperation("A used product cannot be deleted");
                }

            }

            myDal.order.Delete(idProduct);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public void UpDateProduct(BO.Product product)
    {
        try
        {
            Do.Product p = myDal.product.Get(product.ID);
            p.ID = product.ID > 0 ? product.ID : throw new BO.IncorrectIndex("Negative ID");
            p.Name = product.Name != null ? product.Name : throw new BO.InvalidField("Empty name");
            p.Price = product.Price >= 0 ? product.Price : throw new BO.InvalidField("Negative price");
            p.InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidField("Unfavorable inStock");
            p.Category = (Do.Category)(product.Category);
            myDal.product.Update(p);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    }

﻿using BO;
using Do;

namespace BlImplementation;
internal class Product : BlApi.IProduct
{
    DalApi.IDal myDal = new Dal.DalList();
    public delegate bool conditionFunction(Product entity);
    IEnumerable<BO.ProductForList> BlApi.IProduct.GetListOfProducts()
    {

        //Request from the data layer of all products
        IEnumerable<Do.Product?> doProducts = myDal.product.GetAll();

        //
        return doProducts.Select(item => new BO.ProductForList()
        {
            ID = item?.ID ?? throw new Exception(),
            Name = item?.Name ?? throw new Exception(),
            Price = item?.Price ?? throw new Exception(),
            category = (BO.Category)(item?.Category)
        });
    }

    public BO.Product GetProduct(int idProduct)
    {
        //Checking whether the ID is negative
        if (idProduct > 0)
        {
            //A product request based on the data layer identifier, if the information has not arrived, will thrthrow an error
            Do.Product? p1;
            try { p1 = myDal.product.Get(idProduct); }
            catch (Do.DalDoesNotExistException ex)
            {
                throw new BO.InternalErrorException("this id doesnt exsist", ex);
            }

            //Build a layer product based on the data
            BO.Product? product = new BO.Product()
            {
                ID = p1?.ID ?? throw new Exception(),
                Name = p1?.Name,
                Price = (double)(p1?.Price),
                InStock = p1?.InStock ?? throw new Exception(),
                Category = (BO.Category)(p1?.Category),
            };

            return product;
        }
        else throw new BO.InvalidArgumentException("Negative ID");
    }


    public BO.ProductItem GetProduct(int idProduct, BO.Cart cart)
    {

        //Checking whether the ID is negative
        if (idProduct > 0)
        {
            bool isExsist = false;
            BO.OrderItem boOrderItem = new BO.OrderItem();

            //A product request based on the data layer identifier, if the information has not arrived, will throw an error
            Do.Product? p1;
            try { p1 = myDal.product.Get(idProduct); }
            catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

            //Going through all the items in the basket and saving the desired item
            for (int i = 0; i < cart.items.Count && !isExsist; i++)
            {
                if (cart.items[i].ProductId == idProduct)
                {
                    boOrderItem = cart.items[i];
                    isExsist = true;
                }
            }

            //If the item does not exist, a temporary error will be thrown
            if (!isExsist)
                throw new BO.InternalErrorException("The requested product does not exist in the shopping cart");

            if (p1 == null)
                throw new BO.InternalErrorException("not found");
            Do.Product p2= (Do.Product)p1;
            //Building a logical product based on the data and returning it
            BO.ProductItem product = new BO.ProductItem
            {
                ProductID = ((int)p2.ID),
                Name = p2.Name,
                Price = p2.Price ,
                Category = (BO.Category)(p2.Category),
                InStock = p1?.InStock > 0 ? true : false,
                Amount = boOrderItem.QuantityPerItem
            };
            return product;
        }
        else throw new BO.InvalidArgumentException("Negative ID");
    }

    public void AddProduct(BO.Product product)
    {
        //Building a data product based on a logical product+Validity checks and appropriate error throwing
        Do.Product p1 = new Do.Product
        {
            ID = product.ID > 0 ? product.ID : throw new BO.InvalidArgumentException("Negative ID"),
            Name = product.Name != null ? product.Name : throw new BO.InvalidArgumentException("Empty name"),
            Price = product.Price >= 0 ? product.Price : throw new BO.InvalidArgumentException("Negative price"),
            InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidArgumentException("Unfavorable inStock"),
            Category = (Do.Category)(product.Category)
        };

        //Adding the data layer product
        myDal.product.Add(p1);
    }


    public void DeleteProduct(int idProduct)
    {
        //Request from the data layer of all orders
        IEnumerable<Do.Order?> orders = myDal.order.GetAll();

        bool isExsistProduct = orders.Any(currenOrder => myDal.orderItems.GetByIdOrder(currenOrder?.ID).Any(item => item?.ProductId == idProduct));
        //A request for each order's details according to the identifier if the product intended for deletion exists in one of the orders will throw an exception
        //IEnumerable<Do.OrderItem?>  orderItems = orders.Select(currenOrder => myDal.orderItems.GetByIdOrder(currenOrder?.ID).FirstOrDefault(item => item?.ProductId == idProduct));
        if (!isExsistProduct)
            throw new BO.InternalErrorException("A used product cannot be deleted");
        //foreach (var order in orders)
        //{
        //    orderItems = myDal.orderItems.GetByIdOrder(order.ID);
        //    foreach (var item in orderItems)
        //    {
        //        if (item.ProductId == idProduct)
        //            throw new BO.InternalErrorException("A used product cannot be deleted");
        //    }
        //}
        //A request to delete a product according to the identifier and the product does not exist in the data will throw an error
        try { myDal.product.Delete(idProduct); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("A non-existent product cannot be deleted", ex); }

    }

    public void UpDateProduct(BO.Product? product)
    {
        //A product request based on the data layer identifier, if the information has not arrived, will throw an error
        Do.Product? p;
        try { p = myDal.product.Get(product?.ID ?? throw new Exception()); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

        //Updating the data layer product according to the received data in case of incorrect data will throw an error
        p.ID = product.ID > 0 ? product.ID : throw new BO.InvalidArgumentException("Negative ID"); ;
        p.Name = product.Name != null ? product.Name : throw new BO.InvalidArgumentException("Empty name");
        p.Price = product.Price >= 0 ? product.Price : throw new BO.InvalidArgumentException("Negative price");
        p.InStock = product.InStock > 0 ? product.InStock : throw new BO.InvalidArgumentException("Unfavorable inStock");
        p.Category = (Do.Category)(product.Category);
        myDal.product.Update(p);

    }
}

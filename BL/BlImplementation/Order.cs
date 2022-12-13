﻿using BO;
using Do;

namespace BlImplementation;

internal class Order : BlApi.IOrder
{
    //Request access to the data layer
    DalApi.IDal myDal = new Dal.DalList();
    public IEnumerable<BO.OrderForList> GetListOfOrders()
    {
        double price;

        IEnumerable<Do.OrderItem?> myDOOrderItems;//List of order details
        List<BO.OrderForList> boListOrders = new List<BO.OrderForList>();//Order list
        //Request a list of orders from the data layer
        IEnumerable<Do.Order?> doOrders = myDal.order.GetAll();

        //Build an order list of the OrderForList type (logical entity) based on the database


        foreach (var item in doOrders)
        {
            price = 0;
            myDOOrderItems = myDal.orderItems.GetByIdOrder(item?.ID??throw new Exception());

            //Calculating the price of items in the product in order to arrive at the total price

            foreach (var it in myDOOrderItems)
            {
                price += it?.Price * it?.Amount ?? throw new Exception();
            }

            //Build an order list of the OrderForList type on the database
            BO.OrderForList order = new BO.OrderForList
            {
                OrderID = item?.ID ?? throw new Exception(),
                CustomerName = item?.CustomerName,
                status = (BO.OrderStatus)((item?.DeliveryDate != null && item?.ShipDate != null) ? 3 : (item?.ShipDate != null) ? 2 : 1),
                AmountForItems = myDOOrderItems.Count(),
                TotalPrice = price
            };
            boListOrders.Add(order);
        }

        return boListOrders;
    }

    public BO.Order GetOrderDetails(int idOrder)
    {
        double price = 0.0;
        IEnumerable<Do.OrderItem?> doOrderItems = new List<Do.OrderItem?>();
        List<BO.OrderItem> ListOrderItems = new List<BO.OrderItem>();

        //A check that identifies the order is not negative
        if (idOrder > 0)
        {
            //A order request based on the data layer identifier, if the information has not arrived, will throw an error
            Do.Order doOrder;
            try { doOrder = myDal.order.Get(idOrder); }
            catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

            //Request any order details according to its ID
            doOrderItems = myDal.orderItems.GetByIdOrder(idOrder);
            foreach (var item in doOrderItems)
            {
                //A product request based on the data layer identifier, if the information has not arrived, will throw an error
                Do.Product doProduct;
                try { doProduct = myDal.product.Get(item?.ProductId??throw new Exception()); }
                catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

                //Building an item in a logical order, accumulating the price of all the items together and adding it to a list of items in a logical order
                BO.OrderItem boOrderItem = new BO.OrderItem
                {
                    ID = item?.ID??throw new Exception(),
                    ProductId = item?.ProductId??throw new Exception(),
                    NameProduct = doProduct.Name,
                    productPrice = doProduct.Price,
                    QuantityPerItem = item?.Amount??throw new Exception(),
                    TotalPrice = doProduct.Price * item?.Amount??throw new Exception(),
                };
                price += boOrderItem.TotalPrice;
                ListOrderItems.Add(boOrderItem);
            }
            //Building a logical order based on the data and returning it
            BO.Order order = new BO.Order()
            {
                ID = doOrder.ID,
                CustomerName = doOrder.CustomerName,
                status = (BO.OrderStatus)((doOrder.DeliveryDate != null && doOrder.ShipDate != null) ? 3 : (doOrder.ShipDate != null) ? 2 : 1),
                CustomerAddress = doOrder.CustomerAddress,
                CustomerEmail = doOrder.CustomerEmail,
                DeliveryDate = doOrder.DeliveryDate,
                ShipDate = doOrder.ShipDate,
                PaymentDate = doOrder.OrderDate,
                items = ListOrderItems,
                totalPrice = price,
            };
            return order;

        }
        else throw new BO.InvalidArgumentException("Negative ID");
    }

    public BO.Order OrderDeliveryUpdate(int idOrder)
    {
        IEnumerable<Do.OrderItem?> doOrderItems = new List<Do.OrderItem?>();
        List<BO.OrderItem> boOrderItems = new List<BO.OrderItem>();
        double price = 0.0;

        //A order request based on the data layer identifier, if the information has not arrived, will throw an error
        Do.Order doOrder;
        try { doOrder = myDal.order.Get(idOrder); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

        //In the event that the order was sent and not delivered - updating its delivery to now, in any other case appropriate exceptions will be thrown
        if (doOrder.ShipDate != null)
            throw new Exception("The order has already been delivered");
        if (doOrder.DeliveryDate == null)
            throw new Exception("The order has not been sent yet");
        doOrder.ShipDate = DateTime.Now;

        //Updating the status of the order in case the order does not exist in the data will throw an exception
        try { myDal.order.Update(doOrder); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("It is not possible to update the order does not exist", ex); }

        //Request any order details by ID
        doOrderItems = myDal.orderItems.GetByIdOrder(idOrder);

        foreach (var item in doOrderItems)
        {
            //A product request based on the data layer identifier, if the information has not arrived, will throw an error
            Do.Product doProduct;
            try { doProduct = myDal.product.Get(item?.ProductId??-1); }
            catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

            //Building an item in a logical order, accumulating the price of all the items together and adding it to a list of items in a logical order
            BO.OrderItem boOrderItem = new BO.OrderItem
            {
                ID = item?.ID??throw new Exception(),
                ProductId = item?.ProductId??throw new Exception(),
                NameProduct = doProduct.Name,
                productPrice = doProduct.Price,
                QuantityPerItem = item?.Amount??throw new Exception(),
                TotalPrice = doProduct.Price * item?.Amount??throw new Exception()
            };
            price += boOrderItem.TotalPrice;
            boOrderItems.Add(boOrderItem);
        }

        //Building a logical order based on the data and returning it
        BO.Order order = new BO.Order
        {
            ID = idOrder,
            CustomerName = doOrder.CustomerName,
            CustomerAddress = doOrder.CustomerAddress,
            CustomerEmail = doOrder.CustomerEmail,
            status = (BO.OrderStatus)((doOrder.DeliveryDate != null && doOrder.ShipDate != null) ? 3 : (doOrder.ShipDate != null) ? 2 : 1),
            DeliveryDate = doOrder.DeliveryDate,
            ShipDate = doOrder.ShipDate,
            PaymentDate = doOrder.OrderDate,
            items = boOrderItems,
            totalPrice = price,
        };
        return order;
    }

    public BO.OrderTracking OrderTracking(int idOrder)
    {
        //A order request based on the data layer identifier, if the information has not arrived, will throw an error
        Do.Order doOrder = new Do.Order();
        try { doOrder = myDal.order.Get(idOrder); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this is order is not exsist", ex); }

        //Creating an order tracking object in the logical layer according to the data
        BO.OrderTracking myOrderTracking = new BO.OrderTracking();
        myOrderTracking.Tracking = new List<Tuple<DateTime?, string?>?>();
        myOrderTracking.Tracking?.Add(new Tuple<DateTime?, string?>(doOrder.OrderDate, "The order has been confirmed"));
        myOrderTracking.ID = idOrder;
        myOrderTracking.Status = (BO.OrderStatus)((doOrder.DeliveryDate != null && doOrder.ShipDate != null) ? 3 : (doOrder.ShipDate != null) ? 2 : 1);
        if (myOrderTracking.Status == OrderStatus.OrderSend|| myOrderTracking.Status == OrderStatus.OrderProvided)
        {
            myOrderTracking.Tracking.Add(new Tuple<DateTime?, string?>(doOrder.DeliveryDate, "The order was sent"));
        }
         if (myOrderTracking.Status == OrderStatus.OrderProvided)
        {
            myOrderTracking.Tracking.Add(new Tuple<DateTime?, string?>(doOrder.DeliveryDate, "The order was delivered"));
        }
        return myOrderTracking;
    }

    //public BO.Order UpdateOrder()


    public BO.Order OrderShippingUpdate(int orderId)
    {
        Do.Order doOrder = new Do.Order();
        //Check if an order exists (in data layer).
        try { doOrder = myDal.order.Get(orderId); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("There is no product with this id", ex); }

        if (doOrder.DeliveryDate != null)
            throw new InternalErrorException("The order has already been sent");
        //Check if an order exists (in the data layer) and has not yet been sent
       else 
        {
            //Updating the shipping date in the order in both a data entity and a logical entity
            doOrder.DeliveryDate = DateTime.Now;
            myDal.order.Update(doOrder);

            //in order to copy to the BO list
            IEnumerable<Do.OrderItem?> doItems = new List<Do.OrderItem?>();
            doItems = myDal.orderItems.GetByIdOrder(doOrder.ID);

            double price = 0;
            //creating the logical layer
            List<BO.OrderItem> boItems = new List<BO.OrderItem>();
            //Goes over the list of order details from the data layer and created a list of order details from the logical layer
            foreach (var item in doItems)
            {
                Do.Product myDoProduct;
                try { myDoProduct = myDal.product.Get(item?.ProductId??-1); }
                catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id product is not exsist", ex); }
                BO.OrderItem boOrderItem = new BO.OrderItem()
                {
                    ID = item?.ID??throw new Exception(),
                    ProductId = item?.ProductId??throw new Exception(),
                    NameProduct = myDoProduct.Name,
                    productPrice = myDoProduct.Price,
                    QuantityPerItem = item?.Amount??throw new Exception(),
                    TotalPrice = myDoProduct.Price * item?.Amount??throw new Exception()
                };
                price += boOrderItem.TotalPrice;
                boItems.Add(boOrderItem);
            }

            //Creating an order from the logical extent of an order based on the data
            BO.Order boOrder = new BO.Order()
            {
                CustomerName = doOrder.CustomerName,
                CustomerEmail = doOrder.CustomerEmail,
                CustomerAddress = doOrder.CustomerAddress,
                DeliveryDate = doOrder.DeliveryDate,
                ShipDate = doOrder.ShipDate,
                PaymentDate = doOrder.OrderDate,
                status = OrderStatus.OrderSend,
                items = boItems,
                totalPrice = price
            };
            return boOrder;
        }
        return null;
    }

}



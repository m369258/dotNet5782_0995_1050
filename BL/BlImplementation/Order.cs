﻿using BlApi;
using BO;
using System.Runtime.CompilerServices;
namespace BlImplementation;

internal class Order : BlApi.IOrder
{
    //Request access to the data layer
    DalApi.IDal myDal = DalApi.Factory.Get();

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderForList> GetListOfOrders()
    {
        List<BO.OrderForList> boListOrders = new List<BO.OrderForList>();//Order list

        //Request a list of orders from the data layer
        IEnumerable<Do.Order?> doOrders = myDal.order.GetAll();

        //Build an order list of the OrderForList type (logical entity) based on the database
        boListOrders = doOrders.Select(item => createBoOrderFromDoOrder(item)).ToList();
        return boListOrders;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<BO.OrderTracking?> OrdersOfUsers(string? email)
    {
        IEnumerable<BO.OrderTracking> orderTrackings;
        IEnumerable<Do.Order?> doUserOrders;
        IEnumerable<int?> ordersIds;

        if (email == null)
            doUserOrders = myDal.order.GetAll();
        else
            doUserOrders = myDal.order.GetAll(item => item?.CustomerEmail == email);

        ordersIds = doUserOrders.Select(item => item?.ID);
        orderTrackings = ordersIds.Select(item => OrderTracking(item ?? throw new BO.InternalErrorException("problem with idOrder")));

        return orderTrackings;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order GetOrderDetails(int idOrder)
    {
        double price = 0.0;
        IEnumerable<Do.OrderItem?> doOrderItems;
        List<BO.OrderItem> ListOrderItems = new List<OrderItem>();
        //A check that identifies the order is not negative
        if (idOrder > 0)
        {
            //A order request based on the data layer identifier, if the information has not arrived, will throw an error
            Do.Order doOrder;
            try { doOrder = myDal.order.Get(item => item?.ID == idOrder); }
            catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

            //Request any order details according to its ID
            doOrderItems = myDal.orderItems.GetAll(item => item?.OrderId == idOrder);

            //Constructs a list of items in the order of a logical layer
            ListOrderItems = doOrderItems.Select(item => this.buildingOrderItem((Do.OrderItem)(item!), ref price)).ToList();

            //Building a logical order based on the data and returning it
            BO.Order boOrder = new BO.Order();
            doOrder.CopyBetweenEnriries(boOrder);
            boOrder.PaymentDate = doOrder.OrderDate;
            //boOrder.status = (BO.OrderStatus)((doOrder.DeliveryDate != null && doOrder.ShipDate != null) ? 3 : (doOrder.DeliveryDate != null) ? 2 : 1);
            boOrder.status = OrderStatus.OrderConfirmed;
            //Order status check
            if (doOrder.DeliveryDate != null && doOrder.ShipDate == null)
                boOrder.status = OrderStatus.OrderSend;

            else if (doOrder.ShipDate != null)
                boOrder.status = OrderStatus.OrderProvided;



            //boOrder.status = (BO.OrderStatus)((doOrder.DeliveryDate != null && doOrder.ShipDate != null) ? 3 : (doOrder.ShipDate != null) ? 2 : 1);
            boOrder.PaymentDate = doOrder.OrderDate;
            boOrder.items = ListOrderItems;
            boOrder.totalPrice = price;
            return boOrder;
        }
        else throw new BO.InvalidArgumentException("Negative ID");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order OrderDeliveryUpdate(int idOrder)
    {
        IEnumerable<Do.OrderItem?> doOrderItems = new List<Do.OrderItem?>();
        List<BO.OrderItem> boOrderItems = new List<BO.OrderItem>();
        double price = 0.0;

        //A order request based on the data layer identifier, if the information has not arrived, will throw an error
        Do.Order doOrder;
        try { doOrder = myDal.order.Get(item => item?.ID == idOrder); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("There is no order with this id", ex); }

        //In the event that the order was sent and not delivered - updating its delivery to now, in any other case appropriate exceptions will be thrown
        if (doOrder.ShipDate != null)
            throw new BO.InternalErrorException("The order has already been delivered");
        if (doOrder.DeliveryDate == null)
            throw new BO.InternalErrorException("The order has not been sent yet");
        doOrder.ShipDate = DateTime.Now;

        //Updating the status of the order in case the order does not exist in the data will throw an exception
        try { myDal.order.Update(doOrder); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("It is not possible to update the order does not exist", ex); }

        //Request any order details by ID
        doOrderItems = myDal.orderItems.GetAll(item => item?.OrderId == idOrder);

        //Constructs a list of items in the order of a logical layer
        boOrderItems = doOrderItems.Select(item => this.buildingOrderItem((Do.OrderItem)(item!), ref price)).ToList();

        //Building a logical order based on the data and returning it
        BO.Order order = new BO.Order();
        doOrder.CopyBetweenEnriries(order);
        order.ID = idOrder;
        order.status = OrderStatus.OrderProvided;
        order.PaymentDate = doOrder.OrderDate;
        order.items = boOrderItems;
        order.totalPrice = price;
        return order;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.OrderTracking OrderTracking(int idOrder)
    {
        //A order request based on the data layer identifier, if the information has not arrived, will throw an error
        Do.Order doOrder = new Do.Order();
        try { doOrder = myDal.order.Get(item => item?.ID == idOrder); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this is order is not exsist", ex); }

        //Creating an order tracking object in the logical layer according to the data
        BO.OrderTracking myOrderTracking = new BO.OrderTracking();
        myOrderTracking.Tracking = new List<Tuple<DateTime?, string?>?>();
        myOrderTracking.Tracking?.Add(new Tuple<DateTime?, string?>(doOrder.OrderDate, "The order has been confirmed"));
        myOrderTracking.ID = idOrder;
        myOrderTracking.Status = OrderStatus.OrderConfirmed;

        //Order status check
        if (doOrder.DeliveryDate != null)
        {
            myOrderTracking.Status = OrderStatus.OrderSend;
            myOrderTracking.Tracking?.Add(new Tuple<DateTime?, string?>(doOrder.DeliveryDate, "The order was sent"));
        }
        if (doOrder.ShipDate != null)
        {
            myOrderTracking.Status = OrderStatus.OrderProvided;
            myOrderTracking.Tracking?.Add(new Tuple<DateTime?, string?>(doOrder.DeliveryDate, "The order was delivered"));
        }
        return myOrderTracking;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public BO.Order OrderShippingUpdate(int orderId)
    {
        double price = 0.0;
        //Check if an order exists (in data layer).
        Do.Order doOrder = new Do.Order();
        try { doOrder = myDal.order.Get(item => item?.ID == orderId); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("There is no order with this id", ex); }

        //If the order has been sent, an exception will be thrown
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
            doItems = myDal.orderItems.GetAll(item => item?.OrderId == doOrder.ID);

            //Constructs a list of items in the order of a logical layer
            List<BO.OrderItem> boItems = doItems.Select(item => this.buildingOrderItem((Do.OrderItem)(item!), ref price)).ToList();

            //Creating an order from the logical extent of an order based on the data
            BO.Order boOrder = new BO.Order();
            //doOrder.CopyBetweenEnriries(boOrder);
            boOrder.ID = doOrder.ID;
            boOrder.CustomerName = doOrder.CustomerName;
            boOrder.CustomerAddress = doOrder.CustomerAddress;
            boOrder.CustomerEmail = doOrder.CustomerEmail;
            boOrder.PaymentDate = doOrder.OrderDate;
            boOrder.status = OrderStatus.OrderSend;
            boOrder.DeliveryDate = doOrder.DeliveryDate;
            boOrder.PaymentDate = doOrder.OrderDate;
            boOrder.ShipDate = doOrder.ShipDate;
            boOrder.items = boItems;
            boOrder.totalPrice = price;
            return boOrder;
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int? GetOldestOrder()
    {
        IEnumerable<Do.Order?> doOrders = myDal.order.GetAll();
        DateTime? dateTime = DateTime.Now;
        int? id = null;
        foreach (var ord in doOrders)
        {
            if (ord?.OrderDate != null && ord?.OrderDate < dateTime && ord?.ShipDate == null && ord?.DeliveryDate == null)
            {
                dateTime = ord?.OrderDate;
                id = ord?.ID;
            }
            else if (ord?.ShipDate == null && ord?.DeliveryDate != null && ord?.DeliveryDate < dateTime)
            {
                dateTime = ord?.DeliveryDate;
                id = ord?.ID;
            }
        }
        return id;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void UpdateStatus(int id)
    {
        Do.Order order = myDal.order.Get(item => item?.ID == id);

        if (order.DeliveryDate == null)
            OrderShippingUpdate(id);

        else if (order.ShipDate == null)
            OrderDeliveryUpdate(id);
        else
            throw new BO.InternalErrorException("the order shipped");

    }

    //Local helper functions:
    /// <summary>
    /// A helper function that accepts a data layer order and converts it to be orderForList
    /// </summary>
    /// <param name="item">Order data layer</param>
    /// <returns>OrderForList-display layer</returns>
    /// <exception cref="Exception"></exception>
    private BO.OrderForList createBoOrderFromDoOrder(Do.Order? item)
    {
        var myDoOrderItems = myDal.orderItems.GetAll(x => x?.OrderId == (item?.ID ?? throw new Exception())).ToList();

        //Calculating the price of items in the product in order to arrive at the total price
        double price = myDoOrderItems.Sum(it => it?.Price ?? throw new Exception());


        //Build an order list of the OrderForList type on the database
        BO.OrderForList boOrder = new BO.OrderForList();
        item.CopyBetweenEnriries(boOrder);

        boOrder.status = OrderStatus.OrderConfirmed;
        //Order status check
        if (item?.DeliveryDate != null&& item?.ShipDate == null)
            boOrder.status = OrderStatus.OrderSend;

        else if (item?.ShipDate != null)
            boOrder.status = OrderStatus.OrderProvided;

        boOrder.OrderID = item?.ID ?? throw new BO.InternalErrorException("item without ID");
        boOrder.AmountForItems = myDoOrderItems.Count();
        boOrder.TotalPrice = price;
        return boOrder;
    }

    /// <summary>
    /// Builds an item in the order of a logical layer according to an item in the order of a data layer and additional data
    /// </summary>
    /// <param name="doOrderItem">Item on order - data layer</param>
    /// <param name="price">The price for the entire order varies</param>
    /// <returns></returns>
    /// <exception cref="InternalErrorException">If there is no product of an item in the order in the data layer</exception>
    private BO.OrderItem buildingOrderItem(Do.OrderItem doOrderItem, ref double price)
    {
        //A product request based on the data layer identifier, if the information has not arrived, will throw an error
        Do.Product doProduct;
        try { doProduct = myDal.product.Get(x => x?.ID == (doOrderItem.ProductId)); }
        catch (Do.DalDoesNotExistException ex) { throw new InternalErrorException("this id doesnt exsist", ex); }

        //Building an item in a logical order, accumulating the price of all the items together and adding it to a list of items in a logical order
        BO.OrderItem boOrderItem = new BO.OrderItem();
        doProduct.CopyBetweenEnriries(boOrderItem);
        boOrderItem.ID = doOrderItem.ID;
        boOrderItem.ProductId = doProduct.ID;
        boOrderItem.NameProduct = doProduct.Name;
        boOrderItem.productPrice = doProduct.Price;
        boOrderItem.QuantityPerItem = doOrderItem.Amount;
        boOrderItem.TotalPrice = doProduct.Price * doOrderItem.Amount;
        price += boOrderItem.TotalPrice;
        return boOrderItem;
    }

   
}


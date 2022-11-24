﻿using Do;
namespace Dal;
using DalApi;
using System;
using System.Collections.Generic;
internal class DalOrderItem:IOrderItems
{

    /// <summary>
    /// This action adds an orderItem to the system if there is an available space
    /// </summary>
    /// <param name="orderItem">OrderItem to add</param>
    /// <returns>Return the ID number of the added object</returns>
    /// <exception cref="Exception">If there is no space available for a new order, an error will be thrown</exception>
    public int Add(OrderItem orderItem)
    {
        orderItem.ID = DataSource.Config.AutomaticOrderItem;
        int i;
        //Checking whether the product ID exists in any other case will throw an error
        for (i = 0; i < DataSource.products.Count && DataSource.products[i].ID != orderItem.ProductId; i++) ;
        if (i == DataSource.products.Count)
        {
            throw new Exception("this product is exsist");
        }

        //Checking if the order ID exists in any other case will throw an error
        for (i = 0; i < DataSource.orders.Count && DataSource.orders[i].ID != orderItem.OrderId; i++) ;
        if (i == DataSource.orders.Count)
        {
            throw new Exception("this order is exsist");
        }

        //Adding the order item to the database and updating the actual quantity
        DataSource.orderItems.Add(orderItem);

        return orderItem.ID;
    }


    /// <summary>
    /// This returns the correct order by some ID number
    /// </summary>
    /// <param name="idOrderItem">ID number of desired OrderItem</param>
    /// <returns>Returns the desired OrderItem</returns>
    /// <exception cref="Exception">If the required OrderItem does not exist, an error will be thrown</exception>
    public OrderItem Get(int idOrderItem)
    {
        int i;

        //The loop searches for the location of the OrderItem
        for (i = 0; i < DataSource.orderItems.Count && DataSource.orderItems[i].ID != idOrderItem; i++) ;

        //Checking whether the requested OrderItem is found and returning it otherwise throws an error
        if (DataSource.orderItems[i].ID == idOrderItem)
            return DataSource.orderItems[i];

        throw new Exception("there are no order with this id");
    }


    /// <summary>
    /// This operation receives an order and product ID number and returns a requested order item
    /// </summary>
    /// <param name="idOrder">Order ID number</param>
    /// <param name="idProduct">Product ID number.</param>
    /// <returns> a requested order item</returns>
    /// <exception cref="Exception">In case the requested order item does not exist, an error will be thrown</exception>
    public OrderItem Get(int idOrder, int idProduct)
    {
        int i;
        //Search for the desired order item
        for (i = 0; i < DataSource.orderItems.Count && (DataSource.orderItems[i].ProductId != idProduct || DataSource.orderItems[i].OrderId != idOrder); i++) ;

        //If not found, an error will be thrown
        if (DataSource.orderItems.Count == i)
            throw new Exception("there is no orderItem with this idOrder and idProduct");

        return DataSource.orderItems[i];
    }


    /// <summary>
    /// This action gets an order ID number and returns all the order details
    /// </summary>
    /// <param name="idOrder">Order ID number</param>
    /// <returns>All order details</returns>
    public IEnumerable<OrderItem> GetByIdOrder(int idOrder)
    {
        List<OrderItem> newOrderItems = new List<OrderItem>();

        //Enter all order details of the given ID number
        for (int i = 0; i < DataSource.orderItems.Count; i++)
        {
            if (DataSource.orderItems[i].OrderId == idOrder)
                newOrderItems.Add(DataSource.orderItems[i]);
        }

        return newOrderItems;
    }


    /// <summary>
    /// This function receives an ID number of an order and returns its position in the array
    /// </summary>
    /// <param name="idOrderItem">Order ID number</param>
    /// <returns>Its position in the ordering system</returns>
    private int GetIndex(int idOrderItem)
    {
        int i;
        //The loop searches for the location of the OrderItem
        for (i = 0; i < DataSource.orderItems.Count && DataSource.orderItems[i].ID != idOrderItem; i++) ;

        //Checking whether the requested OrderItem is found and returning it otherwise throws an error
        if (DataSource.orderItems[i].ID == idOrderItem)
            return i;
        return -1;
    }


    /// <summary>
    /// This operation accepts an order and updates its details if it exists, otherwise it will throw an error
    /// </summary>
    /// <param name="updateOrder">orderItem to update</param>
    /// <exception cref="Exception">Throw an error if the requested orderItem does not exist</exception>
    public void Update(OrderItem updateOrderItem)
    {
        int ind = GetIndex(updateOrderItem.ID);
        if (ind != -1)
        {
            DataSource.orderItems[ind] = updateOrderItem;
        }
        else { throw new Exception("there is no orderItem like this"); }
    }

    //?????????????????????????????????????????????????????????????
    /// <summary>
    /// This operation gets an order ID number and deletes it if it exists, otherwise an error will be thrown
    /// </summary>
    /// <param name="idOrder">OrderItem ID number</param>
    /// <exception cref="Exception">In case the order does not exist in the database, an error will be thrown</exception>
    public void Delete(int idOrderItem)
    {
        int ind = GetIndex(idOrderItem);
        if (ind != -1)
        {
            //The loop narrows the hole created after deleting the requested order
            for (int i = ind; i < DataSource.orderItems.Count-1; i++)
            {
                DataSource.orderItems[i] = DataSource.orderItems[i + 1];
            }
        }
        else
            throw new Exception("there is no this id orderItem");
    }


    /// <summary>
    /// This returns all orderIAtems
    /// </summary>
    /// <returns>All orderItems</returns>
    public IEnumerable<OrderItem> GetAll()
    {
        List<OrderItem> newOrderItems = new List<OrderItem>();
        //The loop performs the explicit copying of the array of orderItems
        for (int i = 0; i < DataSource.orderItems.Count; i++)
        {
            newOrderItems.Add(DataSource.orderItems[i]);
        }
        return newOrderItems;
    }
}
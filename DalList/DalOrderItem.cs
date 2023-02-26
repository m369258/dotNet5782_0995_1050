using Do;
namespace Dal;
using DalApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class DalOrderItem : IOrderItems
{

    /// <summary>
    /// This action adds an orderItem to the system if there is an available space
    /// </summary>
    /// <param name="orderItem">OrderItem to add</param>
    /// <returns>Return the ID number of the added object</returns>
    /// <exception cref="Exception">If there is no space available for a new order, an error will be thrown</exception>

    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(OrderItem orderItem)
    {
        orderItem.ID = DataSource.Config.AutomaticOrderItem;
        int i;
        //Checking whether the product ID exists in any other case will throw an error
        Do.Product? p = DataSource.products.Find(currenProduct => {return (currenProduct?.ID == orderItem.ProductId); });

        for (i = 0; i < DataSource.products.Count && DataSource.products[i]?.ID != orderItem.ProductId; i++) ;
        if (i == DataSource.products.Count)
        {
            throw new Do.DalAlreadyExistsException(orderItem.ID, "orderItem", "this product is exsist");
        }

        //Checking if the order ID exists in any other case will throw an error
        for (i = 0; i < DataSource.orders.Count && DataSource.orders[i]?.ID != orderItem.OrderId; i++) ;
        if (i == DataSource.orders.Count)
        {
            throw new Do.DalAlreadyExistsException(orderItem.ID, "orderItem", "this order is exsist");
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
    [MethodImpl(MethodImplOptions.Synchronized)]
    public OrderItem Get(Func<OrderItem?, bool> condition)
    {

        return DataSource.orderItems.FirstOrDefault(myOrderItem => condition(myOrderItem)) ??
          throw new Do.DalDoesNotExistException("there are no orderItem with this id");
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
        for (i = 0; i < DataSource.orderItems.Count && DataSource.orderItems[i]?.ID != idOrderItem; i++) ;

        //Checking whether the requested OrderItem is found and returning it otherwise throws an error
        if (DataSource.orderItems[i]?.ID == idOrderItem)
            return i;
        return -1;
    }


    /// <summary>
    /// This operation accepts an order and updates its details if it exists, otherwise it will throw an error
    /// </summary>
    /// <param name="updateOrder">orderItem to update</param>
    /// <exception cref="DalDoesNotExistException">Throw an error if the requested orderItem does not exist</exception>
   
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(OrderItem updateOrderItem)
    {
        int ind = GetIndex(updateOrderItem.ID);
        if (ind != -1)
        {
            DataSource.orderItems[ind] = updateOrderItem;
        }
        else { throw new Do.DalDoesNotExistException(updateOrderItem.ID, "orderItem", "there is no orderItem like this"); }
    }

    /// <summary>
    /// This operation gets an order ID number and deletes it if it exists, otherwise an error will be thrown
    /// </summary>
    /// <param name="idOrder">OrderItem ID number</param>
    /// <exception cref="DalDoesNotExistException">In case the order does not exist in the database, an error will be thrown</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int idOrderItem)
    {
        int ind = GetIndex(idOrderItem);
        if (ind != -1)
        {
            DataSource.orderItems.RemoveAt(ind);
        }
        else
            throw new Do.DalDoesNotExistException(idOrderItem, "orderItem", "there is no this id orderItem");
    }


    /// <summary>
    /// This returns all orderIAtems
    /// </summary>
    /// <returns>All orderItems</returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Do.OrderItem?> GetAll(Func<Do.OrderItem?, bool>? condition)
    {
        return condition != null ?
               DataSource.orderItems.Where(currOrderItem => condition(currOrderItem)) :
               DataSource.orderItems.Select(currOrderItem => currOrderItem);
    }
}
using Do;
namespace Dal;
using DalApi;
using System.Runtime.CompilerServices;

//using System;
//using System.Collections.Generic;
internal class DalOrder : IOrder
{
    /// <summary>
    /// This action adds an order to the system if there is an available space
    /// </summary>
    /// <param name="order">Order to add</param>
    /// <returns>Return the ID number of the added object</returns>
    /// <exception cref="Exception">If there is no space available for a new order, an error will be thrown</exception>
    /// 
    [MethodImpl(MethodImplOptions.Synchronized)]
    public int Add(Order order)
    {
        order.ID = DataSource.Config.AutomaticOrder;
        DataSource.orders.Add(order);
        return order.ID;
    }

    /// <summary>
    /// This returns the correct order by some ID number
    /// </summary>
    /// <param name="idOrder">ID number of desired order</param>
    /// <returns>Returns the desired order</returns>
    /// <exception cref="Exception">If the required order does not exist, an error will be thrown</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public Order Get(Func<Order?, bool> condition)
    {
        return DataSource.orders.FirstOrDefault(myOrder => condition(myOrder)) ??
            throw new Do.DalDoesNotExistException("there are no order with this id");
    }

    /// <summary>
    /// This returns all orders
    /// </summary>
    /// <returns>All orders</returns>

    [MethodImpl(MethodImplOptions.Synchronized)]
    public IEnumerable<Order?> GetAll(Func<Order?, bool>? condition)
    {
        return condition != null ?
               DataSource.orders.Where(currOrder => condition(currOrder)) :
               DataSource.orders.Select(currOrder => currOrder);
    }

    /// <summary>
    /// This operation gets an order ID number and deletes it if it exists, otherwise an error will be thrown
    /// </summary>
    /// <param name="idOrder">Order ID number</param>
    /// <exception cref="Exception">In case the order does not exist in the database, an error will be thrown</exception>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Delete(int idOrder)
    {
        int ind = GetIndex(idOrder);
        if (ind != -1)
        {
            DataSource.orders.RemoveAt(ind);
        }
        else
            throw new Do.DalDoesNotExistException(idOrder, "Order", "there is no this id order");
    }


    /// <summary>
    /// This function receives an ID number of an order and returns its position in the array
    /// </summary>
    /// <param name="idOrder">Order ID number</param>
    /// <returns>Its position in the ordering system</returns>
    private int GetIndex(int idOrder)
    {
        int i = 0;
        //The loop searches for the location of the requested order
        while (i < DataSource.orders.Count && DataSource.orders[i]?.ID != idOrder)
        {
            i++;
        }
        //If the order is found, its position in the order array will be returned, otherwise -1 will be returned
        if (i < DataSource.orders.Count && DataSource.orders[i]?.ID == idOrder)
            return i;
        return -1;
    }


    /// <summary>
    /// This operation accepts an order and updates its details if it exists, otherwise it will throw an error
    /// </summary>
    /// <param name="updateOrder">Invitation to update</param>
    /// <exception cref="Exception">Throw an error if the requested order does not exist</exception>

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Update(Order updateOrder)
    {
        int ind = GetIndex(updateOrder.ID);
        if (ind != -1)
        {
            DataSource.orders[ind] = updateOrder;
        }
        else { throw new Do.DalDoesNotExistException(updateOrder.ID, "Order", "there is no order like this"); }
    }
}
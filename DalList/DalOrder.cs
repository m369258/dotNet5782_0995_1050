﻿using Do;
namespace Dal;
using DalApi;
public class DalOrder:IOrder
{
    /// <summary>
    /// This action adds an order to the system if there is an available space
    /// </summary>
    /// <param name="order">Order to add</param>
    /// <returns>Return the ID number of the added object</returns>
    /// <exception cref="Exception">If there is no space available for a new order, an error will be thrown</exception>
    public void Add(Order order)
    {
            order.ID = DataSource.Config.AutomaticOrder;
            DataSource.Add(order);
    }


    /// <summary>
    /// This returns the correct order by some ID number
    /// </summary>
    /// <param name="idOrder">ID number of desired order</param>
    /// <returns>Returns the desired order</returns>
    /// <exception cref="Exception">If the required order does not exist, an error will be thrown</exception>
    public Order Get(int idOrder)
    {
        int i = 0;
        //The loop searches for the location of the order
        while (i < DataSource.orders.Count && DataSource.orders[i].ID != idOrder)
        {
            i++;
        }
        //Checking whether the requested order is found and returning it otherwise throws an error
        if (DataSource.orders[i].ID == idOrder)
            return DataSource.orders[i];
        throw new Exception("there are no order with this id");
    }


    /// <summary>
    /// This returns all orders
    /// </summary>
    /// <returns>All orders</returns>
    public IEnumerable <Order> GetAll()
    {
        Order[] newOrders = new Order[DataSource.orders.Count];
        //The loop performs the explicit copying of the array of orders
        for (int i=0;i<DataSource.orders.Count; i++)
        {
            newOrders[i] = new Order();
            newOrders[i] = DataSource.orders[i];
        }
        return newOrders;
    }


    /// <summary>
    /// This operation gets an order ID number and deletes it if it exists, otherwise an error will be thrown
    /// </summary>
    /// <param name="idOrder">Order ID number</param>
    /// <exception cref="Exception">In case the order does not exist in the database, an error will be thrown</exception>
    public void Delete(int idOrder)
    {
        int ind = GetIndex(idOrder);
        if (ind != -1)
        {
            //The loop narrows the hole created after deleting the requested order
            for (int i = ind; i < DataSource.orders.Count; i++)
            {
                DataSource.orders[i] = DataSource.orders[i + 1];
            }
            //Downloading the actual amount of members of the orders after deleting an order
        }
        else
            throw new Exception("there is no this id order");
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
        while (i < DataSource.orders.Count && DataSource.orders[i].ID != idOrder)
        {
            i++;
        }
        //If the order is found, its position in the order array will be returned, otherwise -1 will be returned
        if (DataSource.orders[i].ID == idOrder)
            return i;
        return -1;
    }


    /// <summary>
    /// This operation accepts an order and updates its details if it exists, otherwise it will throw an error
    /// </summary>
    /// <param name="updateOrder">Invitation to update</param>
    /// <exception cref="Exception">Throw an error if the requested order does not exist</exception>
    public void Update(Order updateOrder)
    {
        int ind = GetIndex(updateOrder.ID);
        if (ind != -1)
        {
            DataSource.orders[ind] = updateOrder;//??האם זה נחשב להעתקה עמוקה
        }
        else { throw new Exception("there is no order like this"); }
    }
}

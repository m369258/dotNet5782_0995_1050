using Do;
namespace Dal;

public class DalOrderItem
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
        //Checking whether there is room to add an OrderItem otherwise an error will be thrown
        if (DataSource.orderItems.Length - 1 != DataSource.Config.indexOrderItem)
        {
            try
            {
                DataSource.Add(orderItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else
        {
            throw new Exception("there is no place");
        }
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
        for (i = 0; i < DataSource.Config.indexOrderItem && DataSource.orderItems[i].ID != idOrderItem; i++) ;

        //Checking whether the requested OrderItem is found and returning it otherwise throws an error
        if (DataSource.orderItems[i].ID == idOrderItem)
            return DataSource.orderItems[i];

        throw new Exception("there are no order with this id");
    }


    public OrderItem Get(int idOrder, int idProduct)
    {
        int i;
        for (i = 0; i < DataSource.Config.indexOrderItem && DataSource.orderItems[i].ProductId != idProduct || DataSource.orderItems[i].OrderId != idOrder; i++) ;

        if (DataSource.Config.indexOrderItem == i)
            throw new Exception("there is no orderItem with this idOrder and idProduct");

        return DataSource.orderItems[i];
    }


    public OrderItem[] GetByIdOrder(int idOrder)
    {
        OrderItem[] newOrderItems;


        return newOrderItems;

    }
}

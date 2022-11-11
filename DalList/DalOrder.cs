using Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DalOrder
    {
        /// <summary>
        /// This action adds an order to the system if there is an available space
        /// </summary>
        /// <param name="order">Order to add</param>
        /// <returns>Return the ID number of the added object</returns>
        /// <exception cref="Exception">If there is no space available for a new order, an error will be thrown</exception>
        public int Add(Order order)
        {
            order.ID = DataSource.Config.AutomaticOrder;
            //Checking whether there is room to add an order otherwise an error will be thrown
            if (DataSource.orders.Length - 1 != DataSource.Config.indexOrder)
            {
                DataSource.Add(order);
            }
            else
            {
                throw new Exception("there is no place");
            }
            return order.ID;
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
            //The loop searches for the location of the order to be deleted
            while (i < DataSource.Config.indexOrder && DataSource.orders[i].ID != idOrder)
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
        public Order[] GetAllOrders()
        {
            Order[] newOrders = new Order[DataSource.Config.indexOrder];
            for(int i=0;i<DataSource.Config.indexOrder;i++)
            {

            }
            return DataSource.orders;
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
                for (int i = ind; i < DataSource.Config.indexOrder; i++)
                {
                    //??שאלה האם צריך לבצע כאן את העתקה העמוקה הזאת או שאין צורך
                    DataSource.orders[i].ID = DataSource.orders[i + 1].ID;
                    DataSource.orders[i].CustomerName = DataSource.orders[i + 1].CustomerName;
                    DataSource.orders[i].CustomerEmail = DataSource.orders[i + 1].CustomerEmail;
                    DataSource.orders[i].CustomerAddress = DataSource.orders[i + 1].CustomerAddress;
                    DataSource.orders[i].OrderDate = DataSource.orders[i + 1].OrderDate;
                    DataSource.orders[i].DeliveryDate = DataSource.orders[i + 1].DeliveryDate;
                    DataSource.orders[i].ShipDate = DataSource.orders[i + 1].ShipDate;
                }
                DataSource.Config.indexOrder--;
            }
            else
                throw new Exception("there is no this id order");
        }


        private int GetIndex(int idOrder)
        {
            int i = 0;
            while (i < DataSource.Config.indexOrder && DataSource.orders[i].ID != idOrder)
            {
                i++;
            }
            if (DataSource.orders[i].ID == idOrder)
                return i;
            return -1;
        }
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
    //public int Add(Order order)
    //{
    //    order.ID = DataSource.Config.NextOrderNumber;//שמירה מה ה ID של האורדר וגם ;
    //    DataSource.OrdersList.Add(order);//להחליט אם לעשות מערך או רשימה
    //    if(DataSource.config.indexinthearray>=גודל מערך)//אם זה מערך
    //            throw "אין מקום";
    //    //ואם יש מקום במערך אז צריך ללכת ולהוסיף
    //    return order.ID;
    //}

    //pubic order GetById(int id)
}


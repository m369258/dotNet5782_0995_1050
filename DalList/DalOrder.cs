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

        public int Add(Order order)
        {
            order.ID = DataSource.Config.AutomaticOrder;
            if (DataSource.orders.Length - 1 != DataSource.Config.indexOrder)
            {
                DataSource.Add(order);
            }
            else
            {
                throw new Exception("there are no place");
            }
            return order.ID;
        }

        public Order Get(int idOrder)
        {
            int i = 0;
            while (i < DataSource.Config.indexOrder && DataSource.orders[i].ID != idOrder)
            {
                i++;
            }
            if (DataSource.orders[i].ID == idOrder)
                return DataSource.orders[i];
            throw new Exception("there are no order with this id");
        }

        public Order[] GetAllOrders()
        {
            return DataSource.orders;
        }

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
        }

        public void Update()
        {

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
}

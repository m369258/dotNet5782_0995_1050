using BO;
using DalApi;
using System.Diagnostics;
using System.Net.Sockets;

namespace BlImplementation;

internal class Order : BlApi.IOrder
{
    public DalApi.IDal myDal { get; set; }
    public IEnumerable<BO.OrderForList> GetListOfOrders()
    {
        IEnumerable<Do.OrderItem> myOrderItems = new List<Do.OrderItem>();

        List<BO.OrderForList> ListProducts = new List<BO.OrderForList>();
        foreach (var item in myDal.order.GetAll())
        {
            myOrderItems = myDal.orderItems.GetByIdOrder(item.ID);
            BO.OrderForList order = new BO.OrderForList
            {
                OrderID = item.ID,
                CustomerName = item.CustomerName,
                status = (BO.OrderStatus)((item.DeliveryDate == null && item.ShipDate == null) ? 1 : (item.ShipDate == null) ? 2 : 3),

            };
            ListProducts.Add(order);
        }
        return ListProducts;
    }

    public BO.Order OrderShippingUpdate(int orderId)
    {
        Do.Order doOrder = new Do.Order();
        //2.1 בדיקה האם ההזמנה קיימת
        try { doOrder = myDal.order.Get(orderId); }
        catch { }//זריקה - אין פריט כזה עם האידי הזה

        //במקרה שההזמנה קיימת עלינו לבדוק אם היא כבר נשלחה2.2..
        if (doOrder.DeliveryDate == new DateTime())//אם עדיין לא נשלח
        {
            //3.1 עידכון בישות הנתונים
            doOrder.DeliveryDate = DateTime.Now;
            //BO.Order boOrder = new BO.Order();
            //???צריך לעשות את הדבר המצחיק הזה?? הרי כבר עשינו את זה למעלה
            try { doOrder = myDal.order.Get(orderId); }
            catch { }//זריקה - אין פריט כזה עם האידי הזה

            //על מנת להעתיק לרשימת BO
            IEnumerable<Do.OrderItem> doItems = new List<Do.OrderItem>();
            doItems = myDal.orderItems.GetByIdOrder(doOrder.ID);

            //List<Do.Product> doProducts=new List<Do.Product>();
            //יצירת משכבה הלוגית
            List<BO.OrderItem> boItems = new List<BO.OrderItem>();
            foreach (var item in doItems)
            {
                //doProducts.Add(myDal.product.Get(item.ProductId));

                Do.Product myDoProduct = myDal.product.Get(item.ProductId);


                BO.OrderItem boOrderItem = new BO.OrderItem()
                {
                    ID = item.ID,
                    ProductId = item.ProductId,
                    NameProduct = myDoProduct.Name,
                    productPrice = myDoProduct.Price,
                    QuantityPerItem = item.Amount,
                    TotalPrice = myDoProduct.Price * item.Amount
                };
                boItems.Add(boOrderItem);


            }

            //foreach (var item in doItems)
            //{
            //    Do.Product doProduct = myDal.product.Get(item.ProductId);
            //    BO.OrderItem boOrderItem = new BO.OrderItem
            //    {
            //        ID = item.ID,
            //        ProductId = item.ProductId,
            //        NameProduct = doProduct.Name,
            //        productPrice = doProduct.Price,
            //        QuantityPerItem = item.Amount,
            //        TotalPrice = doProduct.Price * item.Amount
            //    };
            //    price += boOrderItem.TotalPrice;
            //    ListOrderItems.Add(boOrderItem);
            //}

            BO.Order boOrder = new BO.Order()
            {
                CustomerName = doOrder.CustomerName,
                CustomerEmail = doOrder.CustomerEmail,
                CustomerAddress = doOrder.CustomerAddress,


                DeliveryDate = doOrder.DeliveryDate,
                ShipDate = doOrder.ShipDate,
                PaymentDate =new DateTime(),//??
                status = OrderStatus.OrderSend,
                items = boItems,
                totalPrice = 100//??
        };
            return boOrder;
        }
        return null;
    }

}



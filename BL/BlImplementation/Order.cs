using BO;
using DalApi;
using System.Diagnostics;
using System.Net.Sockets;

namespace BlImplementation;

internal class Order : BlApi.IOrder
{
    //Request access to the data layer
    DalApi.IDal myDal = new Dal.DalList();
    // public DalApi.IDal myDal { get; set; }

    public IEnumerable<BO.OrderForList> GetListOfOrders()
    {
        double price;

        IEnumerable<Do.OrderItem> myDOOrderItems;//List of order details
        List<BO.OrderForList> boListOrders = new List<BO.OrderForList>();//Order list
        //Request a list of orders from the data layer
        IEnumerable<Do.Order> doOrders = myDal.order.GetAll();

        //Build an order list of the OrderForList type (logical entity) based on the database
        foreach (var item in doOrders)
        {
            price = 0;
            myDOOrderItems = myDal.orderItems.GetByIdOrder(item.ID);

            //Calculating the price of items in the product in order to arrive at the total price
            foreach (var it in myDOOrderItems)
            {
                price += it.Price * it.Amount;
            }

            //Build an order list of the OrderForList type on the database
            BO.OrderForList order = new BO.OrderForList
            {
                OrderID = item.ID,
                CustomerName = item.CustomerName,
                status = (BO.OrderStatus)((item.DeliveryDate != DateTime.MinValue && item.ShipDate != DateTime.MinValue) ? 3 : (item.ShipDate != DateTime.MinValue) ? 2 : 1),
                AmountForItems = myDOOrderItems.Count(),
                TotalPrice = price
            };
            boListOrders.Add(order);
        }

        return boListOrders;
    }

    public BO.Order GetOrderDetails(int idOrder)
    {
        double price = 0;
        IEnumerable<Do.OrderItem> myOrderItems = new List<Do.OrderItem>();
        List<BO.OrderItem> ListOrderItems = new List<BO.OrderItem>();
        if (idOrder > 0)
        {
            try
            {

                Do.Order doOrder = myDal.order.Get(idOrder);
                myOrderItems = myDal.orderItems.GetByIdOrder(idOrder);
                foreach (var item in myOrderItems)
                {
                    Do.Product doProduct = myDal.product.Get(item.ProductId);
                    BO.OrderItem boOrderItem = new BO.OrderItem
                    {
                        ID = item.ID,
                        ProductId = item.ProductId,
                        NameProduct = doProduct.Name,
                        productPrice = doProduct.Price,
                        QuantityPerItem = item.Amount,
                        TotalPrice = doProduct.Price * item.Amount
                    };
                    price += boOrderItem.TotalPrice;
                    ListOrderItems.Add(boOrderItem);
                }
                BO.Order order = new BO.Order()
                {
                    ID = doOrder.ID,
                    CustomerName = doOrder.CustomerName,
                    status = (BO.OrderStatus)((doOrder.DeliveryDate != DateTime.MinValue && doOrder.ShipDate != DateTime.MinValue) ? 3 : (doOrder.ShipDate != DateTime.MinValue) ? 2 : 1),
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
            catch { throw; }
        }
        else throw new BO.IncorrectIndex("negetive id");
    }

    public BO.Order OrderDeliveryUpdate(int idOrder)
    {
        IEnumerable<Do.OrderItem> doOrderItems = new List<Do.OrderItem>();
        List<BO.OrderItem> boOrderItems = new List<BO.OrderItem>();
        double price = 0.0;
        Do.Order doOrder = new Do.Order();
        ////חריגות
        try
        {
            doOrder = myDal.order.Get(idOrder);
        }
        catch { }

        if (doOrder.ShipDate != DateTime.MinValue)
            throw new Exception("ההזמנה כבר סופקה");
        if (doOrder.DeliveryDate == DateTime.MinValue)
            throw new Exception("");
        doOrder.ShipDate = DateTime.Now;
        try
        {
            myDal.order.Update(doOrder);
        }
        catch { }
        doOrderItems = myDal.orderItems.GetByIdOrder(idOrder);
        foreach (var item in doOrderItems)
        {
            Do.Product doProduct = myDal.product.Get(item.ProductId);
            BO.OrderItem boOrderItem = new BO.OrderItem
            {
                ID = item.ID,
                ProductId = item.ProductId,
                NameProduct = doProduct.Name,
                productPrice = doProduct.Price,
                QuantityPerItem = item.Amount,
                TotalPrice = doProduct.Price * item.Amount
            };
            price += boOrderItem.TotalPrice;
            boOrderItems.Add(boOrderItem);
        }
        BO.Order order = new BO.Order
        {
            ID = idOrder,
            CustomerName = doOrder.CustomerName,
            CustomerAddress = doOrder.CustomerAddress,
            CustomerEmail = doOrder.CustomerEmail,
            status = (BO.OrderStatus)((doOrder.DeliveryDate != DateTime.MinValue && doOrder.ShipDate != DateTime.MinValue) ? 3 : (doOrder.ShipDate != DateTime.MinValue) ? 2 : 1),
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
        Do.Order doOrder = new Do.Order();
        ////חריגות
        try
        {
            doOrder = myDal.order.Get(idOrder);
        }
        catch { }

        BO.OrderTracking myOrderTracking = new BO.OrderTracking
        {
            ID = idOrder,
            Status = (BO.OrderStatus)((doOrder.DeliveryDate != DateTime.MinValue && doOrder.ShipDate != DateTime.MinValue) ? 3 : (doOrder.ShipDate != DateTime.MinValue) ? 2 : 1),
            ///////tuple
        };

        return myOrderTracking;
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


            if (doOrder.DeliveryDate != DateTime.MinValue)
                throw new Exception("ההזמנה כבר נשלחה");
            doOrder.DeliveryDate = DateTime.Now;

            myDal.order.Update(doOrder);

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

            double price = 0;
            List<BO.OrderItem> ListOrderItems = new List<BO.OrderItem>();
            foreach (var item in doItems)
            {
                Do.Product doProduct = myDal.product.Get(item.ProductId);
                BO.OrderItem boOrderItem = new BO.OrderItem
                {
                    ID = item.ID,
                    ProductId = item.ProductId,
                    NameProduct = doProduct.Name,
                    productPrice = doProduct.Price,
                    QuantityPerItem = item.Amount,
                    TotalPrice = doProduct.Price * item.Amount
                };
                price += boOrderItem.TotalPrice;
                ListOrderItems.Add(boOrderItem);
            }

            BO.Order boOrder = new BO.Order()
            {
                CustomerName = doOrder.CustomerName,
                CustomerEmail = doOrder.CustomerEmail,
                CustomerAddress = doOrder.CustomerAddress,


                DeliveryDate = doOrder.DeliveryDate,
                ShipDate = doOrder.ShipDate,
                PaymentDate = doOrder.OrderDate,//??
                status = OrderStatus.OrderSend,
                items = boItems,
                totalPrice = price//??
            };
            return boOrder;
        }
        return null;
    }

}



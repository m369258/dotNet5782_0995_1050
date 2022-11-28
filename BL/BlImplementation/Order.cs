using BO;

namespace BlImplementation;

internal class Order : BlApi.IOrder
{
    public DalApi.IDal myDal { get; set; }
    public IEnumerable<BO.OrderForList> GetListOfOrders() {
        double price = 0;
        IEnumerable<Do.OrderItem> myOrderItems = new List<Do.OrderItem>();
        List<BO.OrderForList> ListOrders = new List<BO.OrderForList>();
        foreach (var item in myDal.order.GetAll())
        {
            myOrderItems = myDal.orderItems.GetByIdOrder(item.ID);
            foreach (var it in myOrderItems)
            {
                price += it.Price;
            }
            BO.OrderForList order = new BO.OrderForList
            {
                OrderID = item.ID,
                CustomerName = item.CustomerName,
                status = (BO.OrderStatus)((item.DeliveryDate == null && item.ShipDate == null) ? 1 : (item.ShipDate == null) ? 2 : 3),
                AmountForItems = myOrderItems.Count(),
                TotalPrice = price
            };
            ListOrders.Add(order);
        }
        return ListOrders;
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
                    status = (BO.OrderStatus)((doOrder.DeliveryDate == null && doOrder.ShipDate == null) ? 1 : (doOrder.ShipDate == null) ? 2 : 3),
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
        IEnumerable<Do.Order> myOrders = new List<Do.Order>();
         BO.Order order = new BO.Order();
        return order;
    }

    public BO.OrderTracking OrderTracking(int idOrder)
    {
        ////חריגות
        bool isExsist = false;
        Do.Order doOrder = new Do.Order();
        IEnumerable<Do.Order> boOrders = new List<Do.Order>();
        boOrders = myDal.order.GetAll();
        foreach(var item in boOrders)
        {
            if(item.ID==idOrder)
            {
                isExsist = true;
                doOrder = item;
            }
        }
        if (!isExsist)
            throw new Exception("");
        BO.OrderTracking myOrderTracking = new BO.OrderTracking
        {
            ID = idOrder,
            Status = (BO.OrderStatus)((doOrder.DeliveryDate == null && doOrder.ShipDate == null) ? 1 : (doOrder.ShipDate == null) ? 2 : 3),
            ///////tuple
        };
        return myOrderTracking;

    }

}

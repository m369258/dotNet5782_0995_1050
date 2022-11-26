namespace BlImplementation;

internal class Order: BlApi.IOrder
{
    public DalApi.IDal myDal { get; set; }
    public IEnumerable<BO.OrderForList> GetListOfOrders() {

        List<BO.OrderForList> ListProducts = new List<BO.OrderForList>();
        foreach (var item in myDal.order.GetAll())
        {
            BO.OrderForList order = new BO.OrderForList
            {
                OrderID = item.ID,
                CustomerName = item.CustomerName,
                status =(BO.OrderStatus)((item.DeliveryDate == null && item.ShipDate == null) ? 1 :( item.ShipDate == null) ? 2: 3)

            };
            ListProducts.Add(order);
        }
        return ListProducts;
    }
}

using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

sealed internal class DalXml : IDal
{
    //על מנת לדאוד שזה סינגלטון עשיתי: האם זה בסדר????
    public static IDal Instance { get; } = new DalXml();
    //static DalXml()בשביל זה צריך using....?
    //{
    //    DataSource.intitialize();
    //}

    public IProduct product { get; } = new Dal.Product();

    public IOrder order { get; } = new Dal.Order();

    public IOrderItems orderItems { get; } = new Dal.OrderItem();

    public IUser users { get; } = new Dal.User();
}

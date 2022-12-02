
using BlApi;
using BO;
using DalApi;
using Do;

namespace BlImplementation;

internal class Cart : BlApi.ICart
{
    DalApi.IDal MyDal = new Dal.DalList();
    public BO.Cart Add(BO.Cart myCart, int idProduct)
    {
        bool isExist = false;
        int i;
        for (i = 0; i < myCart.items.Count && !isExist; i++)
        {
            if (myCart.items[i].ProductId == idProduct)
                isExist = true;
        }


        //אם מוצר לא קיים בסל קניות
        if (!isExist)
        {
            //int productId = myCart.items[i].ProductId;
            Do.Product myProduct;
            try { myProduct = MyDal.product.Get(idProduct); }
            catch { throw; }//אי די של פריט לא קיים(זה יכול לקרות שעד שהזמינו הסירו את הפריט...)
            if (myProduct.InStock >= 1)
            {
                BO.OrderItem myOrderItem = new BO.OrderItem();
                myOrderItem.ProductId = idProduct;
                myOrderItem.NameProduct = myProduct.Name;
                myOrderItem.productPrice = myProduct.Price;
                myOrderItem.QuantityPerItem = 1;
                myOrderItem.productPrice = myProduct.Price;
                myOrderItem.TotalPrice = myProduct.Price;
                myCart.items.Add(myOrderItem);
                myCart.TotalPrice += myProduct.Price;
            }
            else throw new NotEnoughInStock("There is not enough stock of this product.");
        }
        else//אם המוצר קיים בסל קניות:
        {
            int productId = myCart.items[i - 1].ProductId;
            Do.Product myProduct;
            try { myProduct = MyDal.product.Get(productId); }
            catch { throw; }//אי די של פריט לא קיים(זה יכול לקרות שעד שהזמינו הסירו את הפריט...)
            if (myCart.items[i - 1].QuantityPerItem <= myProduct.InStock)
            {
                myCart.items[i - 1].QuantityPerItem++;
                myCart.items[i - 1].productPrice = myProduct.Price;
                myCart.items[i - 1].TotalPrice += myProduct.Price;
                myCart.TotalPrice += myProduct.Price;
            }
            else throw new NotEnoughInStock("There is not enough stock of this product.");
        }

        return myCart;
    }

    public void MakeAnOrder(BO.Cart myCart)
    {
        int productId;
        //במקרה בו השם או הכתובת של הלקוח ריקים תיזרק שגיאה
        if (myCart.CustomerName == "")
            throw new InvalidArgumentException("empty customer name");
        if (myCart.CustomerAddress == "")
            throw new InvalidArgumentException("empty customer address");

        if (!myCart.CustomerEmail.Contains("@") || !myCart.CustomerEmail.Contains("."))
            throw new InvalidArgumentException("Invalid email");

        //בדיקה שכל המוצרים קיימים
        foreach (var item in myCart.items)
        {
            //בדיקה האם קיים במלאי
            productId = item.ProductId;
            Do.Product myProduct;
            try { myProduct = MyDal.product.Get(productId); }//במקרה שלא קיים תיזרק שגיאה משכבת הנותנים
            catch (Exception ex) { throw; }//!!לטפל בשגיאה
            //בדיקה האם הכמויות חיוביות
            if (item.QuantityPerItem < 0)
                throw new InvalidArgumentException("negative quantity");
            //אין מספיק במלאי
            if (item.QuantityPerItem > myProduct.InStock)
                throw new InvalidArgumentException("No quantity in stock");
        }

        //יצירת אובייקטט מDO
        Do.Order myOrder = new Do.Order()
        {
            CustomerName = myCart.CustomerName,
            CustomerAddress = myCart.CustomerAddress,
            CustomerEmail = myCart.CustomerEmail,
            OrderDate = DateTime.Now,
            ShipDate = DateTime.MinValue,
            DeliveryDate = DateTime.MinValue,
        };

        int orderId = MyDal.order.Add(myOrder);

        foreach (var item in myCart.items)
        {
            Do.OrderItem myOrderItem = new Do.OrderItem()
            {
                OrderId = orderId,
                ProductId = item.ProductId,
                Price = item.productPrice,//???האם צריך מחיר רק למוצר יחיד או לכל המוצרים?????????????????????????
                Amount = item.QuantityPerItem
            };
            try { MyDal.orderItems.Add(myOrderItem); }//לעשות כאן זריקת חריגה במקרה בו אין אפשרות להוסיף........
            catch { throw; }//לזרוק שגיאה מתאימה!!!!

            Do.Product product;
            try { product = MyDal.product.Get(myOrderItem.ProductId); }
            catch { throw; }//הפריט כבר קיים
            product.InStock -= myOrderItem.Amount;
            MyDal.product.Update(product);
        }
    }
    private void Add(BO.Cart myCart, int idProduct, int newQuantity)
    {
        Do.Product myProduct;
        try { myProduct = MyDal.product.Get(idProduct); }
        catch { throw; }//אי די של פריט לא קיים(זה יכול לקרות שעד שהזמינו הסירו את הפריט...)
        if (myProduct.InStock >= newQuantity)
        {
            BO.OrderItem myOrderItem = new BO.OrderItem();
            myOrderItem.ProductId = idProduct;
            myOrderItem.NameProduct = myProduct.Name;
            myOrderItem.productPrice = myProduct.Price;
            myOrderItem.QuantityPerItem = newQuantity;
            myOrderItem.productPrice = myProduct.Price;
            myOrderItem.TotalPrice = myProduct.Price * newQuantity;
            myCart.items.Add(myOrderItem);
            myCart.TotalPrice += myProduct.Price * newQuantity;
        }
        else throw new NotEnoughInStock("There is not enough stock of this product.");
    }

    public BO.Cart Update(BO.Cart myCart, int idProduct, int newQuantity)
    {
        bool isExist = false;
        int i;
        for (i = 0; i < myCart.items.Count && !isExist; i++)
        {
            if (myCart.items[i].ProductId == idProduct)
                isExist = true;
        }

        if (isExist)
        {
            //אם הכמות גדלה 
            if (newQuantity > myCart.items[i-1].QuantityPerItem)
            {
                int productId = myCart.items[i-1].ProductId;
                Do.Product myProduct;
                try { myProduct = MyDal.product.Get(productId); }
                catch { throw; }//אי די של פריט זה אינו קיים
                if (newQuantity <= myProduct.InStock)
                {
                    myCart.items[i-1].QuantityPerItem = newQuantity;
                    myCart.items[i-1].TotalPrice = myCart.items[i-1].productPrice * newQuantity;
                }
                else throw new NotEnoughInStock("There is not enough stock of this product.");
            }
            //אם הכמות קטנה
            else if (newQuantity < myCart.items[i-1].QuantityPerItem&& newQuantity!=0)
            {
                int productId = myCart.items[i-1].ProductId;
                Do.Product myProduct;
                try { myProduct = MyDal.product.Get(productId); }
                catch { throw; }//אי די של פריט זה אינו קיים
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!?????????????????????????????????
                //יש לדאוג במקרה בו אני משנה את הכמות וכרגע יש פחות מהכמות שאני משנה
                //יש לדאוג לכל הפרטים הקטנים בו למשל אני משנה משהו בסל וכבר פריט אחד אינו קיים וכו 
                if (newQuantity <= myProduct.InStock)
                {
                    myCart.items[i-1].QuantityPerItem = newQuantity;
                    myCart.items[i-1].productPrice = myCart.items[i-1].productPrice * newQuantity;
                    myCart.items[i-1].TotalPrice = myCart.items[i-1].productPrice * newQuantity;
                }
            }
            //אם הכמות נהייתה 0
            else
            {
                myCart.TotalPrice -= myCart.items[i - 1].TotalPrice;

                myCart.items.Remove(myCart.items[i-1]);
            }

            //?????????????????????????????????????????????????????????????????????????????????????????
            //?? האם צריך לבדוק האם הכמות החדשה שקיבלתי היא שלילית או שישר כשמקיש את המספר לזרוק שגיאה
        }
        else
        {
            Add(myCart, idProduct, newQuantity);
        }

        return myCart;
    }
}

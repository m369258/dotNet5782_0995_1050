
using BO;
using DalApi;

namespace BlImplementation;

internal class Cart : BlApi.ICart
{
    public DalApi.IDal MyDal { get; set; }
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
            int productId = myCart.items[i].ProductId;
            Do.Product myProduct = MyDal.product.Get(productId);
            if (myCart.items[i].QuantityPerItem <= myProduct.InStock)
            {
                myCart.items[i].productPrice = myProduct.Price;
                myCart.items[i].TotalPrice += myProduct.Price;
            }
            else throw new NotEnoughInStock("There is not enough stock of this product.");
        }
        else//אם המוצר קיים בסל קניות:
        {
            int productId = myCart.items[i].ProductId;
            Do.Product myProduct = MyDal.product.Get(productId);
            if (myCart.items[i].QuantityPerItem <= myProduct.InStock)
            {
                myCart.items[i].QuantityPerItem++;
                myCart.items[i].productPrice += myProduct.Price;
                myCart.items[i].TotalPrice += myProduct.Price;
            }
            else throw new NotEnoughInStock("There is not enough stock of this product.");
        }

        return myCart;
    }

    public void MakeAnOrder(BO.Cart myCart)
    {
        int productId;
        //במקרה בו השם או הכתובת של הלקוח ריקים תיזרק שגיאה
        if(myCart.CustomerName==null||myCart.CustomerAddress==null)
            //throw;//תיזרק שגיאה פרטי הלקוח ריקים
        if(!myCart.CustomerEmail.Contains("@")||!myCart.CustomerEmail.Contains("."))
                //throw;//זריקה כתובת מייל בלקוח אינו תקינה

        //בדיקה שכל המוצרים קיימים
        foreach (var item in myCart.items)
        {
            //בדיקה האם קיים במלאי
            productId = item.ProductId;
            Do.Product myProduct = MyDal.product.Get(productId);//במקרה שלא קיים תיזרק שגיאה משכבת הנותנים
            //בדיקה האם הכמויות חיוביות
            if(item.QuantityPerItem <0)
                //throw;//תיזרק שגיאה הכמות שלילית
            //יש מספיק במלאי
            if (item.QuantityPerItem > myProduct.InStock)
                //throw;//אז תיזרק שגיאה של אין מספיק במלאי
            
        }
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
            if (newQuantity > myCart.items[i].QuantityPerItem)
            {
                int productId = myCart.items[i].ProductId;
                Do.Product myProduct = MyDal.product.Get(productId);
                if (newQuantity <= myProduct.InStock)
                {
                    int plusQuantity = newQuantity - myCart.items[i].QuantityPerItem;
                    myCart.items[i].QuantityPerItem = newQuantity;
                    myCart.items[i].productPrice += myCart.items[i].productPrice * plusQuantity;
                    myCart.items[i].TotalPrice += myCart.items[i].productPrice * plusQuantity;
                }
                else throw new NotEnoughInStock("There is not enough stock of this product.");
            }
            //אם הכמות קטנה
            else if (newQuantity < myCart.items[i].QuantityPerItem)
            {
                int minusQuantity = newQuantity - myCart.items[i].QuantityPerItem;
                myCart.items[i].QuantityPerItem = newQuantity;
                myCart.items[i].productPrice -= myCart.items[i].productPrice * minusQuantity;
                myCart.items[i].TotalPrice -= myCart.items[i].productPrice * minusQuantity;
            }
            //אם הכמות נהייתה 0
            else
            {
                myCart.items.Remove(myCart.items[i]);
                myCart.TotalPrice -= myCart.items[i].productPrice;
            }

            //?????????????????????????????????????????????????????????????????????????????????????????
            //?? האם צריך לבדוק האם הכמות החדשה שקיבלתי היא שלילית או שישר כשמקיש את המספר לזרוק שגיאה
        }

        return myCart;
    }
}

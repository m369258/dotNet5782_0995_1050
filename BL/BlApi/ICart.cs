namespace BlApi;

public interface ICart
{
    /// <summary>
    /// Adding a product to the shopping cart
    /// </summary>
    /// <param name="myCart">a shopping cart object</param>
    /// <param name="idProduct">a product ID</param>
    /// <returns>will return an updated shopping cart object (logical entity).</returns>
    public BO.Cart Add(BO.Cart myCart, int idProduct);

    /// <summary>
    ///  Updating the quantity of a product in the shopping basket
    /// </summary>
    /// <param name="myCart">Shopping Cart object</param>
    /// <param name="idProduct">ProductId</param>
    /// <param name="newQuantity">newQuantity</param>
    /// <returns>will return an updated shopping cart object (logical entity).</returns>
    public BO.Cart Update(BO.Cart myCart, int idProduct, int newQuantity);

    /// <summary>
    /// Basket confirmation for order / placing an order
    /// </summary>
    /// <param name="myCart">You will receive a shopping basket (which this time includes the buyer's information - name, email address, address)</param>
    public void MakeAnOrder(BO.Cart myCart);

    //public BO.Cart Delete(BO.Cart myCart, int idProduct, List<Tuple<int, int>> items = null);
    public BO.Cart Delete(BO.Cart myCart, int idProduct);

}

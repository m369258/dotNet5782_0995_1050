namespace Do;

/// <summary>
/// exception in case a product/order/orderItem has been requested and its id doesn't exist: 
/// </summary>
public class DalDoesNotExistException : Exception
{
    public int EntityID;
    public string EntityName;
    public DalDoesNotExistException(int id, string name)
        : base() { EntityID = id; EntityName = name; }
    public DalDoesNotExistException(int id, string name, string massage)
        : base(massage) { EntityID = id; EntityName = name; }
    public DalDoesNotExistException(int id, string name, string massage, Exception innerException)
        : base(massage, innerException) { EntityID = id; EntityName = name; }
    public DalDoesNotExistException( string massage)
       : base(massage) {  }
    public override string ToString()
    {
        if (EntityID != -1)
            return $"{EntityName} number {EntityID} does not exist.";
        else  //in case it's an orderItem and or the productID wasn't found or the orderID wasn't found 
            return $"{EntityName} does not exist.";
    }
}

/// <summary>
/// exception in case a product/order/orderItem has been requested to be added and its id already exists: 
/// </summary>
public class DalAlreadyExistsException : Exception
{
    public int EntityID;
    public string EntityName;
    public DalAlreadyExistsException(int id, string name)
        : base() { EntityID = id; EntityName = name; }
    public DalAlreadyExistsException(int id, string name, string massage)
        : base(massage) { EntityID = id; EntityName = name; }
    public DalAlreadyExistsException(int id, string name, string massage, Exception innerException)
        : base(massage, innerException) { EntityID = id; EntityName = name; }
    public override string ToString() =>
        $"{EntityName} number {EntityID} already exists.";
}

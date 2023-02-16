namespace BlApi;
public class Factory
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>The method will return an instance of the Bl class (from the BlImplementation subfolder)</returns>
    static public BlApi.IBl Get() { return new BlApi.Bl(); }
}

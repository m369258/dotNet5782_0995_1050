using System.Collections;
using System.Reflection;

namespace BO;

internal static class Tools
{
    /// <summary>
    /// An extension method to all entities of the logical layer and prints the entire entity
    /// </summary>
    /// <typeparam name="T">enentity</typeparam>
    /// <param name="t">enentity to print</param>
    /// <returns>A string with all the entity details</returns>
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";

        //Going over all the properties in their type check the measure and it is a collection will run the function for each object in the collection
        foreach (PropertyInfo item in t!.GetType().GetProperties())
        {
            var val = item.GetValue(t, null);
            if (!(val is string) && val is IEnumerable list)
            {
                foreach (var listItem in list)
                {
                    str += listItem;
                }
            }
            else
                str += "\n" + item.Name +
                           ": " + item.GetValue(t, null);
        }
        return str;
    }

    /// <summary>
    /// Converts between BO type and DO and vice versa according to the required case
    /// </summary>
    /// <typeparam name="U">B0/DO</typeparam>
    /// <typeparam name="T">BO/DO</typeparam>
    /// <param name="fromEntity">BO/DO Entity-Conversion from one type to another</param>
    /// <param name="toEntity">BO/DO Entity-Converting from this type to a received type</param>
    public static void CopyBetweenEnriries<U, T>(this T fromEntity, U toEntity)
    {
        //Place the type type of the resulting object to convert to its type
        Type uType = toEntity!.GetType();

        //Passing over all the properties of the object that converts it and copying the exactly equal properties
        foreach (PropertyInfo prop in fromEntity!.GetType().GetProperties())
        {
            PropertyInfo? uProp = uType.GetProperty(prop.Name);
            if(uProp?.PropertyType==prop.PropertyType)
            {
                uProp.SetValue(toEntity, prop.GetValue(fromEntity, null), null);
            }
        }
    }
}

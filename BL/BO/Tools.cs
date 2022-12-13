using System.Collections;
using System.Reflection;

namespace BO;

internal static class Tools
{
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";
        var myType = t.GetType().GetProperties();
        foreach (PropertyInfo item in t.GetType().GetProperties())
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


    public static void CopyBetweenEnriries<U, T>(this T fromEntity, U toEntity)
    {
        Type uType = toEntity.GetType();
        foreach (PropertyInfo prop in fromEntity.GetType().GetProperties())
        {
            PropertyInfo? uProp = uType.GetProperty(prop.Name);
            if(uProp?.PropertyType==prop.PropertyType)
            {
                uProp.SetValue(toEntity, prop.GetValue(fromEntity, null), null);
            }
        }
    }
}

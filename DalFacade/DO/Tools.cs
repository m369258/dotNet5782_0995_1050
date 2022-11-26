using System.Reflection;

namespace DO;

/// <summary>
/// 
/// </summary>
static class Tools
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";
        foreach (PropertyInfo item in t.GetType().GetProperties())
            str += "\n" + item.Name +
            ": " + item.GetValue(t, null);

        return str;
    }
}

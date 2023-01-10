using System;
using System.Globalization;
using System.Windows.Data;
namespace PL;

//internal class Converts
//{
//}

public class ConverIntToString : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

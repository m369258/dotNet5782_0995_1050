using BO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace PL;

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


public class NotBooleanToVisibilityConverter : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Cart cart = (BO.Cart)value;
        if (cart != null && cart.items != null && cart.items.Count != 0)
        {
            return Visibility.Hidden; //Visibility.Collapsed;
        }
        else
        {
            return Visibility.Visible;
        }
    }

    //convert from target property type to source property type
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}

public class NotBooleanToVisibilityConverter2 : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Cart cart = (BO.Cart)value;
        if (cart != null && cart.items != null && cart.items.Count != 0)
        {
            return Visibility.Visible; //Visibility.Collapsed;
        }
        else
        {
            return Visibility.Hidden;
        }
    }

    //convert from target property type to source property type
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}




/// <summary>
/// convert image path to picture
/// </summary>
class ConvertPathToBitmapImage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            string imageRelativeName = (string)value;
            string currentDir = Environment.CurrentDirectory[..^4];
            string imageFullName = currentDir + imageRelativeName;//direction of the picture
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageFullName));//makes the picture
            return bitmapImage;
        }
        catch
        {
            string imageRelativeName = @"\pics\default.jpg";//default picture
            string currentDir = Environment.CurrentDirectory[..^4];
            string imageFullName = currentDir + imageRelativeName;//direction of the picture
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageFullName));//makes the picture
            return bitmapImage;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
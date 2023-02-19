using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
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

public class NotBooleanToVisibileConverter : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isVisible = (bool)value;
        if (isVisible)
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

public class NotBooleanToVisibilityConverter : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        ObservableCollection<BO.ProductItem> my = (ObservableCollection<BO.ProductItem>)value;
        return my != null && my.Any(product => product.Amount > 0);
    }
    //convert from target property type to source property type
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


class ConvertPathToBitmapImage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        try
        {
            //if (value == null)
            //{
            //    return null;
            //}
                string imageRelativeName = (string)value;
                string currentDir = Environment.CurrentDirectory[..^4];
                string imageFullName = currentDir + imageRelativeName;//direction of the picture
                BitmapImage bitmapImage = new BitmapImage(new Uri(imageRelativeName, UriKind.Relative));//makes the picture
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





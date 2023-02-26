using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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

public class NotConvertStringToVisibility : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string name = (string)value;
        if (name != null)
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

public class ConvertStringToVisibility : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string name = (string)value;
        if (name == null)
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
        const string imagesDirectory = @"PL\img\catalog";

        try
        {
            if (value == "" || value == null)
                throw new Exception();
            string imageRelativeName = (string)value;
            string? currentDir = Directory.GetParent(Environment.CurrentDirectory)?.FullName;
            string imageFullName = Path.Combine(currentDir ?? throw new Exception(), imagesDirectory, imageRelativeName);
            //string imageFullName = currentDir + imageRelativeName;//direction of the picture
            // BitmapImage bitmapImage = new BitmapImage(new Uri(imageRelativeName, UriKind.Relative));//makes the picture
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageFullName, UriKind.Absolute));//makes the picture

            return bitmapImage;
        }
        catch
        {
            string imageRelativeName = @"logo.png";//default picture
            string? currentDir = Directory.GetParent(Environment.CurrentDirectory)?.FullName;
            string imageFullName = Path.Combine(currentDir ?? throw new Exception(), imagesDirectory, imageRelativeName);
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageFullName, UriKind.Absolute));//makes the picture
            return bitmapImage;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class NotBooleanToVisibilityConverter1 : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Cart cart = (BO.Cart)value;
        if (cart != null && cart.items != null && cart.items.Count != 0)
        {
            return Visibility.Hidden;
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

    // private void btnPayment_Click(object sender, RoutedEventArgs e) => new paymentWindow(MyCart).ShowDialog();
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


using System;
using System.Text.RegularExpressions;
using System.Windows;
namespace PL;

/// <summary>
/// Interaction logic for SignInWindow.xaml
/// </summary>
public partial class SignInWindow : Window
{
    private BlApi.IBl bl = BlApi.Factory.Get();

    public BO.Users user
    {
        get { return (BO.Users)GetValue(userProperty); }
        set { SetValue(userProperty, value); }
    }

    // Using a DependencyProperty as the backing store for user.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty userProperty =
        DependencyProperty.Register("user", typeof(BO.Users), typeof(SignInWindow), new PropertyMetadata(null));

    public string ConfirmPassword
    {
        get { return (string)GetValue(ConfirmPasswordProperty); }
        set { SetValue(ConfirmPasswordProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ConfirmPassword.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ConfirmPasswordProperty =
        DependencyProperty.Register("ConfirmPassword", typeof(string), typeof(SignInWindow), new PropertyMetadata(""));


    public bool IsFill
    {
        get { return (bool)GetValue(IsFillProperty); }
        set { SetValue(IsFillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsFill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsFillProperty =
        DependencyProperty.Register("IsFill", typeof(bool), typeof(SignInWindow), new PropertyMetadata(false));

    //עבור אימייל לא תקין
    public bool IsInvalidEmail
    {
        get { return (bool)GetValue(IsInvalidEmailProperty); }
        set { SetValue(IsInvalidEmailProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsInvalidEmail.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsInvalidEmailProperty =
        DependencyProperty.Register("IsInvalidEmail", typeof(bool), typeof(SignInWindow), new PropertyMetadata(false));

    public bool NoTheSamePassword
    {
        get { return (bool)GetValue(NoTheSamePasswordProperty); }
        set { SetValue(NoTheSamePasswordProperty, value); }
    }

    // Using a DependencyProperty as the backing store for NoTheSamePassword.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NoTheSamePasswordProperty =
        DependencyProperty.Register("NoTheSamePassword", typeof(bool), typeof(SignInWindow), new PropertyMetadata(false));

    public SignInWindow(BO.TypeOfUser position)
    {
        user = new BO.Users();
        InitializeComponent();
        user.TypeOfUser = position;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (!checkEmail())
            IsInvalidEmail = true;
        else
            IsInvalidEmail = false;//צריך את זה??

        if ((string?)ConfirmPassword != user.Password)
            NoTheSamePassword = true;
        else
        {
            NoTheSamePassword = false;

            if ((user.Name == null || user.Address == null || user.Email == null ||
                       user.Password == null || ConfirmPassword == null))
                IsFill = true;
            else
            {
                IsFill = false;
                try { bl.user.AddUser(user); }
                //יש לנו גם לייבל שזורק מייל לא תקין וגם שגיאה, האם אפשר לוותר על הזריקת החריגה???????????????..
                catch (BO.InvalidArgumentException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (BO.AlreadyExsist ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                System.Windows.MessageBox.Show("You've successfully signed up!😊", "🍰", MessageBoxButton.OK);
                this.Close();
            }
        }
    }

    /// <summary>
    /// check if the email is valid
    /// </summary>
    /// <returns></returns>
    private bool checkEmail()
    {
        string email = txtEmail.Text;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        return match.Success;
    }
}

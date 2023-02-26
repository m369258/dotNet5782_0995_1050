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

    #region dp
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
    #endregion

    /// <summary>
    /// constructive action
    /// </summary>
    /// <param name="position">customer or manager</param>
    public SignInWindow(BO.TypeOfUser position)
    {
        user = new BO.Users();
        InitializeComponent();
        user.TypeOfUser = position;
    }

    /// <summary>
    /// sign in to the system
    /// </summary>
    /// <param name="sender">btn sign in</param>
    /// <param name="e">more details</param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if ((user.Name == null || user.Address == null || user.Email == null ||
                      user.Password == null || ConfirmPassword == null || ConfirmPassword == "" ||
                      user.Name == "" || user.Address == "" || user.Email == "" ||
                      user.Password == ""))
            IsFill = true;
        else
            IsFill = false;

        if (!checkEmail())
            IsInvalidEmail = true;
        else
            IsInvalidEmail = false;

        if ((string?)ConfirmPassword != user.Password)
            NoTheSamePassword = true;
        else
        {
            NoTheSamePassword = false;
        }

        if (NoTheSamePassword == false && IsInvalidEmail == false && IsFill == false)
        {
            IsFill = false;
            try { bl.user.AddUser(user); }
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
            new LogInWindow().Show();
            this.Close();
            return;
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

    /// <summary>
    /// Opening the main window and closing the current window.
    /// </summary>
    private void txtBack_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
namespace PL.MainPages;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class Homepage : Window
{
    /// <summary>
    /// Bl object to have an access to the Bl functions
    /// </summary>
    private BlApi.IBl bl = BlApi.Factory.Get();
    bool played = true;
    public Homepage()
    {
        InitializeComponent();
        VideoControl.Play();
    }
    /// <summary>
    /// button for logging in
    /// </summary>
    /// <param name="sender">the sender to the event</param>
    /// <param name="e">the event</param>
    private void BtnLogIn_Click(object sender, RoutedEventArgs e) => new LogInWindow().ShowDialog();
    /// <summary>
    /// button to close the window
    /// </summary>
    /// <param name="sender">the sender to the event</param>
    /// <param name="e">the event</param>
    private void Button_Click(object sender, RoutedEventArgs e) => this.Close();

    /// <summary>
    /// open instagram link
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void inst_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = "https://www.instagram.com/ilovecupcakes_tlv/",
            UseShellExecute = true
        });
    }
    /// <summary>
    /// open twitter link
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void twitter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = "https://twitter.com/ilovecupcakesll",
            UseShellExecute = true
        });
    }
    /// <summary>
    /// open pinterest link
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pinterest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = "https://www.pinterest.com/officialpandora/",
            UseShellExecute = true
        });
    }
    /// <summary>
    /// open youtube link
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void youtube_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        System.Diagnostics.Process.Start(new ProcessStartInfo
        {
            FileName = "https://www.facebook.com/ilovecupcakes.co.il/",
            UseShellExecute = true
        });
    }
    /// <summary>
    /// when window is activated, play video
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ___Pandora_Israel___Activated(object sender, EventArgs e)
    {
        VideoControl.Play();
    }
    /// <summary>
    /// when window is deactivated, pause video
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ___Pandora_Israel___Deactivated(object sender, EventArgs e)
    {
        VideoControl.Pause();
    }
}
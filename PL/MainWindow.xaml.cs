﻿using PL.Product;
using System.Windows;
namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private BlApi.IBl bl = new BlApi.Bl();

    public MainWindow()
    {
        InitializeComponent();
        this.Activated += (s, a) => this.ApplyState();
        this.LocationChanged += (s, a) => this.SetState();

    }

    private void ShowProductsButton_Click(object sender, RoutedEventArgs e) => new ProductForListWindow().Show();
}

﻿using PL.Order;
using PL.Product;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ShowProductsButton_Click(object sender, RoutedEventArgs e) => new ProductForListWindow().Show();

    private void bntAddOrder_Click(object sender, RoutedEventArgs e)=>new OrderForListWindow().Show();

    private void btnTrackingOrders_Click(object sender, RoutedEventArgs e)=>new TrackingOrdersWindow().Show();  
}

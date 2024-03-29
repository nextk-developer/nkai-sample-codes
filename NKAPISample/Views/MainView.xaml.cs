﻿using CommunityToolkit.Mvvm.DependencyInjection;
using NKAPISample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NKAPISample.Views
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox)?.ScrollToEnd();
        }

    }
}

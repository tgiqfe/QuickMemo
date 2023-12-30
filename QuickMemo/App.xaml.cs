﻿using System.Configuration;
using System.Data;
using System.Windows;

namespace QuickMemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Item.BindingParam = new BindingParam();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
        }
    }

}

using AppDependencies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF_MVVM.ViewModels;
using WPF_MVVM.Windows;

namespace WPF_MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Dependencies d = new Dependencies();

            MainWindow app = new MainWindow();
            MainWindowViewModel context = new MainWindowViewModel(d);
            app.DataContext = context;
            app.Show();
        }
    }
}

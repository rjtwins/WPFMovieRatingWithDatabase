using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MovieRatingWithDatabase;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IDataController APICaller = new DataController();
            MainWindow Display = new MainWindow();

            IController Controller = new Controller(Display.MainPage, APICaller);
            Display.SetController(Controller);
            MainWindow.Show();
        }
    }
}
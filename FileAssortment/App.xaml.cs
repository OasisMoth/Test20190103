using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FileAssortment
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public void Application_StartUp(object sender, StartupEventArgs e)
        {
            var w = new MainWindow();
            w.ShowDialog();
        }
    }
}

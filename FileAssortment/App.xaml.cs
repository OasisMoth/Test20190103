using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FileAssortment.Properties;
using log4net;

namespace FileAssortment
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Application_StartUp(object sender, StartupEventArgs e)
        {
            logger.Info("Launch FileAssorter");
            var w = new MainWindow();
            w.ShowDialog();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Fatal("UnhandledException :", e.Exception);
            MessageBox.Show(MainWindow, FileAssortment.Properties.Resources.M_UnhandledException, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            // 例外処理済みのフラグを立て,プログラムを正常終了させる
            e.Handled = true;
            this.Shutdown();
        }

    }
}

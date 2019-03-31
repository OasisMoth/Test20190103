using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileAssortment
{
    public interface IWindowService
    {
        void ShowDialog();
    }

    public class WindowService
    {
        public WindowService(Window owner, DataContextBase dataContext)
        {
            this._DataContext = dataContext;
            this._Owner = owner;
        }

        protected Window _Owner;

        protected DataContextBase _DataContext;
    }

    public class ConfigWindowService : WindowService, IWindowService
    {
        public ConfigWindowService(Window owner, DataContextBase dataContext) : base(owner, dataContext) { }

        public  void ShowDialog()
        {
            var configWindow = new ConfigWindow()
            {
                DataContext = this._DataContext,
                Owner = this._Owner,
            };

            configWindow.ShowDialog();
        }
    }
}

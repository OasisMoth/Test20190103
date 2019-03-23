using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileAssortment
{
    public interface IDialogService
    {
        void ShowMessage(string message);

        void ShowMessage(string message, string caption, MessageBoxButton button);
    }

    public class DialogService : IDialogService
    {
        public DialogService(Window owner)
        {
            this._owner = owner;
        }

        private Window _owner;

        public void ShowMessage(string message)
        {
            if (this._owner != null)
            {
                MessageBox.Show(this._owner, message);
            }
            else
            {
                MessageBox.Show(message);
            }
        }

        public void ShowMessage(string message, string caption, MessageBoxButton button)
        {
            if (this._owner != null)
            {
                MessageBox.Show(this._owner, message, caption, button);
            }
            else
            {
                MessageBox.Show(message, caption, button);
            }
        }
    }
}

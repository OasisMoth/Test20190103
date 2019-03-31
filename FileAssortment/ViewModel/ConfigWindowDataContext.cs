using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileAssortment.ViewModel
{
    public class ConfigWindowDataContext : DataContextBase
    {
        public string FolderCreateValue
        {
            get { return this._FolderCreateValue; }
            set { if (_FolderCreateValue != value) this.SetAndNotify(ref _FolderCreateValue, value); }
        }
        private string _FolderCreateValue;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileAssortment.Properties;

namespace FileAssortment
{
    public class MainWindowDataContext : DataContextBase
    {
        FileAssort _assorter = new FileAssort();

        #region Property
        public string TargetDirectory
        {
            get { return this._TargetDirectory; }
            set { if (_TargetDirectory != value) this.SetAndNotify(ref _TargetDirectory, value); }
        }
        private string _TargetDirectory = Resources.M_NoSelectFolder;
        #endregion

        #region AssortButton
        public DelegateCommand AssortCommand
        {
            get
            {
                if(this._AssortCommand == null) this._AssortCommand = new DelegateCommand(AssortExecute, CanAssortExecute);
                return this._AssortCommand;
            }
        }
        private DelegateCommand _AssortCommand;

        private void AssortExecute()
        {
            _assorter.AssortFile(this.TargetDirectory);
        }

        private bool CanAssortExecute()
        {
            return this.TargetDirectory != Resources.M_NoSelectFolder;
        }
        #endregion

        #region TargetButton
        public DelegateCommand TargetCommand
        {
            get
            {
                if (this._TargetCommand == null)
                {
                    this._TargetCommand = new DelegateCommand(TargetExecute);
                }
                return this._TargetCommand;
            }
        }
        private DelegateCommand _TargetCommand;

        private void TargetExecute()
        {
            this.TargetDirectory = this._assorter.SelectDirectory();
        }
        #endregion
    }
}

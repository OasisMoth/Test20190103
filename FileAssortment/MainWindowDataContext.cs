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

        public bool IsProcessing
        {
            get { return this._IsProcessing; }
            set { if (_IsProcessing != value) this.SetAndNotify(ref _IsProcessing, value); }
        }
        private bool _IsProcessing = false;
        #endregion

        #region AssortButton
        public DelegateCommand AssortButton
        {
            get
            {
                if(this._AssortButton == null) this._AssortButton = new DelegateCommand(AssortExecute, CanAssortExecute);
                return this._AssortButton;
            }
        }
        private DelegateCommand _AssortButton;

        private void AssortExecute()
        {
            this.IsProcessing = true;
            _assorter.AssortFile(this.TargetDirectory);
        }

        private bool CanAssortExecute()
        {
            return this.TargetDirectory != Resources.M_NoSelectFolder;
        }
        #endregion

        #region TargetButton
        public DelegateCommand TargetButton
        {
            get
            {
                if (this._TargetButton == null)
                {
                    this._TargetButton = new DelegateCommand(TargetExecute);
                }
                return this._TargetButton;
            }
        }
        private DelegateCommand _TargetButton;

        private void TargetExecute()
        {
            this.IsProcessing = true;
            this.TargetDirectory = this._assorter.SelectDirectory();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FileAssortment.Properties;

namespace FileAssortment
{
    public class MainWindowDataContext : DataContextBase
    {
        public MainWindowDataContext(IDialogService dialogService, IWindowService windowService)
        {
            this._DialogService = dialogService;
            this._ConfigWindowService = windowService;
        }

        private IDialogService _DialogService;

        private IWindowService _ConfigWindowService;

        private readonly FileAssort Assorter = new FileAssort();

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

        #region AssortButton
        public DelegateCommand AssortButton
        {
            get
            {
                if (this._AssortButton == null) this._AssortButton = new DelegateCommand(AssortExecute, CanAssortExecute);
                return this._AssortButton;
            }
        }
        private DelegateCommand _AssortButton;

        private void AssortExecute()
        {
            this.IsProcessing = true;
            void hundler(object s, AssortCompleteEventArgs e)
            {
                this.IsProcessing = false;
                var msg = e.HasError ? Resources.M_AssortCompleteWithError : Resources.M_AssortComplete;
                this._DialogService.ShowMessage(msg, Resources.W_ApplicationTitle, MessageBoxButton.OK);

                this.Assorter.AssortComplete -= hundler;
            }

            this.Assorter.AssortComplete += hundler;
            this.Assorter.AssortFile(this.TargetDirectory);
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
            this.TargetDirectory = this.Assorter.SelectTargetDirectory();
        }
        #endregion

        public DelegateCommand ConfigMenu
        {
            get
            {
                if (this._ConfigMenu == null)
                {
                    this._ConfigMenu = new DelegateCommand(ConfigMenuExecute);
                }
                return this._ConfigMenu;
            }
        }
        public DelegateCommand _ConfigMenu;

        private void ConfigMenuExecute()
        {
            this._ConfigWindowService.ShowDialog();
        }
    }
}

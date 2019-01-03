using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FileAssortment
{
    /// <summary>
    /// ViewModelの基本クラス。INotifyPropertyChangedの実装を提供する。
    /// </summary>
    public class DataContextBase : INotifyPropertyChanged
    {
        /// <summary>
        /// プロパティの変更があったときに発行する。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// PropertyChangedイベントを発行する。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetAndNotify<T>(ref T property, T value, [CallerMemberName]string propertyName = "")
        {
            property = value;
            OnPropertyChanged(propertyName);
        }
    }

}

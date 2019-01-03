using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileAssortment
{
    /// <summary>
    /// デリゲートを受け取るICommand
    /// </summary>
    public sealed class DelegateCommand : ICommand
    {
        private Action execute;

        private Func<bool> canExecute;

        /// <summary>
        /// コマンドのExecuteメソッドで実行する処理を指定してDelegateCommandのインスタンスを
        /// 作成する。
        /// </summary>
        /// <param name="execute">Executeメソッドで実行する処理</param>
        public DelegateCommand(Action execute) : this(execute, () => true)
        {
        }

        /// <summary>
        /// コマンドのExecuteメソッドで実行する処理とCanExecuteメソッドで実行する処理を指定して
        /// DelegateCommandのインスタンスを作成する。
        /// </summary>
        /// <param name="execute">Executeメソッドで実行する処理</param>
        /// <param name="canExecute">CanExecuteメソッドで実行する処理</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// コマンドを実行する。
        /// </summary>
        public void Execute()
        {
            this.execute();
        }

        /// <summary>
        /// コマンドの実行可否を問い合わせる。
        /// </summary>
        /// <returns>実行可能な場合はtrue</returns>
        public bool CanExecute()
        {
            return this.canExecute();
        }

        /// <summary>
        /// ICommand.CanExecuteの明示的な実装。CanExecuteメソッドに処理を委譲する。
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        /// <summary>
        /// CanExecuteの結果に変更があったことを通知するイベント。
        /// WPFのCommandManagerのRequerySuggestedイベントをラップする形で実装。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// ICommand.Executeの明示的な実装。Executeメソッドに処理を委譲する。
        /// </summary>
        /// <param name="parameter"></param>
        void ICommand.Execute(object parameter)
        {
            this.Execute();
        }
    }
}

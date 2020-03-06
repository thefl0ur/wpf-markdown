//using Neo.Markdig.Xaml;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace markdown_render
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        internal void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private string changelog;
        public string Changelog {
            get
            {
                return changelog;
            }
            set
            {
                changelog = value;
                RaisePropertyChanged("Changelog");
            }
        }

        private void onCommand(object o)
        {
            MessageBox.Show($"Кликнули по ссылке. Содержимое: {(((ExecutedRoutedEventArgs)o).Parameter)}");
        }

        public MainWindowViewModel()
        {
            Command = new RelayCommand(onCommand);
            Changelog = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Readme.txt"));
        }

        public RelayCommand Command;


    }

    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;
        private object removePrinter;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(object removePrinter)
        {
            this.removePrinter = removePrinter;
        }
        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        #endregion // ICommand Members
    }
}

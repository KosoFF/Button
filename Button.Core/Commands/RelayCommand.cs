﻿using System;
using System.Windows.Input;

namespace Button.Core.Commands
{
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        #endregion // Fields

        #region Ctor

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Ctor

        #region ICommand members

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion //ICommand Members
    }

    public class RelayCommand<T> : ICommand
    {
        #region Fields

        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        #endregion

        #region Ctor 

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        ///     Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand members

        /// <summary>
        ///     Determines whether this <see cref="RelayCommand" /> can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke((T) parameter) ?? true;
        }

        /// <summary>
        ///     Executes the <see cref="RelayCommand" /> on the current command target.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command. If the command does not require data to be passed, this object can be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            _execute((T) parameter);
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Method used to raise the <see cref="CanExecuteChanged" /> event
        ///     to indicate that the return value of the <see cref="CanExecute" />
        ///     method has changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
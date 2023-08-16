using System;
using System.Windows.Input;

namespace SteeringWheel.Service.Buttons
{
    internal class ActionCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private readonly Action? _action;
        Action<object>? _executeDelegate;

        private static ActionCommand? _instance = null;
        public static ActionCommand Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ActionCommand();
                }
                return _instance;
            }
        }

        public ActionCommand(Action<object>? executeDelegate)
        {
            _executeDelegate = executeDelegate!;
        }

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public ActionCommand()
        {
        }

        public void Execute(object? parameter)
        {

            if (_action != null)
                _action();

            if (_action == null)
                _executeDelegate!(parameter!);
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }
    }
}

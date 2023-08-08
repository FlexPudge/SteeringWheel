using SteeringWheel.Service;
using SteeringWheel.Service.Buttons;
using System.Windows.Input;

namespace SteeringWheel.ViewModels
{
    internal class ButtonXYViewModel : ViewModelBase
    {

        private ICommand? commandLeft;
        public ICommand CommandLeft
        {
            get
            {
                return commandLeft
                    ?? (commandLeft = new ActionCommand(() =>
                    {
                        MovementMethod.GoToXMinus();
                    }));
            }
        }
        private ICommand? commandRight;
        public ICommand CommandRight
        {
            get
            {
                return commandRight
                    ?? (commandRight = new ActionCommand(() =>
                    {
                        MovementMethod.GoToXPlus();
                    }));
            }
        }
        private ICommand? commandUp;
        public ICommand CommandUp
        {
            get
            {
                return commandUp
                    ?? (commandUp = new ActionCommand(() =>
                    {
                        MovementMethod.GoToYPlus();
                    }));
            }
        }

        private ICommand? commandDown;
        public ICommand CommandDown
        {
            get
            {
                return commandDown
                    ?? (commandDown = new ActionCommand(() =>
                    {
                        MovementMethod.GoToYMinus();
                    }));
            }
        }
    }
}

using SteeringWheel.Service;
using SteeringWheel.Service.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SteeringWheel.ViewModels
{
    public class ButtonZViewModel : ViewModelBase
    {
        private ICommand? commandUp;
        public ICommand CommandUp
        {
            get
            {
                return commandUp
                    ?? (commandUp = new ActionCommand(() =>
                    {
                        MovementMethod.GoToZPlus();
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
                        MovementMethod.GoToZMinus();
                    }));
            }
        }
    }
}

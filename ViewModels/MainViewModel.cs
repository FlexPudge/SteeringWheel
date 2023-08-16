using SteeringWheel.Connection;
using SteeringWheel.Service;
using SteeringWheel.Service.Buttons;
using SteeringWheel.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace SteeringWheel.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string? _statusConnection;
        private double _progressBarValue;
        private ConnectionPortView? _connectionPortView;

        #region координаты
        private double _coordValueX;
        private double _coordValueY;
        private double _coordValueZ;
        private double _coordValueA;

        public double CoordValueX
        {
            get
            {
                if (_coordValueX == 0)
                {
                    _coordValueX = 0;
                    return _coordValueX;
                }
                return _coordValueX;
            }
            set
            {
                _coordValueX = value;
                OnPropertyChanged("CoordValueX");
            }
        }
        public double CoordValueY
        {
            get
            {
                if (_coordValueY == 0)
                {
                    _coordValueY = 0;
                    return _coordValueY;
                }
                return _coordValueY;
            }
            set
            {
                _coordValueY = value;
                OnPropertyChanged("CoordValueY");
            }
        }
        public double CoordValueZ
        {
            get
            {
                if (_coordValueZ == 0)
                {
                    _coordValueZ = 0;
                    return _coordValueZ;
                }
                return _coordValueZ;
            }
            set
            {
                _coordValueZ = value;
                OnPropertyChanged("CoordValueZ");
            }
        }
        public double CoordValueA
        {
            get
            {
                if (_coordValueA == 0)
                {
                    _coordValueA = 0;
                    return _coordValueA;
                }
                return _coordValueA;
            }
            set
            {
                _coordValueA = value;
                OnPropertyChanged("CoordValueA");
            }
        }
        #endregion

        public string StatusConnection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_statusConnection))
                {
                    _statusConnection = "Нет подключения";
                }
                return _statusConnection;
            }
            set
            {
                _statusConnection = value;
                OnPropertyChanged("StatusConnection");
            }
        }       
        public double ProgressBarValue
        {
            get => _progressBarValue;
            set
            {
                _progressBarValue = value;
                OnPropertyChanged("ProgressBarValue");
                BroadcastStepValue.SetStepValue(ProgressBarValue);
            }
        }

        private ICommand? openConnectedViewCommand;
        public ICommand OpenConnectedViewCommand
        {
            get
            {
                return openConnectedViewCommand
                    ?? (openConnectedViewCommand = new ActionCommand(() =>
                    {
                        _connectionPortView = new ConnectionPortView();
                        _connectionPortView.dataContextVM.Notify += CheckStatusConnection;
                        _connectionPortView.Show();
                    }));
            }
        }

        private ICommand? updateCoordinats;
        public ICommand UpdateCoordinats
        {
            get
            {
                return updateCoordinats
                    ?? (updateCoordinats = new ActionCommand(() =>
                    {
                        UpdateCoordinates();
                    }));
            }
        }

        private ICommand? keyDetectedCommand;
        public ICommand KeyDetectedCommand
        {
            get
            {
                this.keyDetectedCommand = this.keyDetectedCommand ?? new ActionCommand
                    (x => SelectedKey(x.ToString()!));
                return this.keyDetectedCommand;
            }
        }

        #region скрытые комманды
        private ICommand? addPosition;
        public ICommand AddPosition
        {
            get
            {
                return addPosition
                    ?? (addPosition = new ActionCommand(() =>
                    {
                        SavePositionInFile.AddPositionFiles(CoordValueX,
                            CoordValueY,CoordValueZ);
                    }));
            }
        }

        private ICommand? savePosition;
        public ICommand SavePosition
        {
            get
            {
                return savePosition
                    ?? (savePosition = new ActionCommand(() =>
                    {
                        SavePositionInFile.SaveFiles();
                    }));
            }
        }
        #endregion

        private TypeConnection enumRadio;
        public TypeConnection EnumRadio
        {
            get => enumRadio;
            set
            {
                enumRadio = value;
            }
        }
     
        private void CheckStatusConnection(bool value)
        {
            if (value)
            {
                StatusConnection = "Есть подключение";
            }
            else 
            {
                StatusConnection = "Нет подключения";
            }
            _connectionPortView!.Close();
        }
        private void UpdateCoordinates()
        {
            SerialPortConnection.Sender(Commands.LoadCoordinats());
            Thread.Sleep(500);
            ConvertCoordinates();
        }
        private void ConvertCoordinates()
        {
            int count = 0;
            var response = SerialPortConnection.Response();
            if (response == null)
            {
                MessageBox.Show("Нет подключения");
                return;
            }
            if (response.Length != 0)
            {
                byte[] coord = new byte[2];
                ObservableCollection<double> Coordinates = new ObservableCollection<double>();
                for (int i = 2; i < response.Length - 1; i++)
                {
                    coord[count] = response[i];
                    count++;
                    if (count == 2)
                    {
                        var num = BitConverter.ToUInt16(coord, 0);
                        Coordinates.Add(num / 8);
                        count = 0;
                    }

                }
                CoordValueA = Coordinates[0];
                CoordValueX = Coordinates[1];
                CoordValueY = Coordinates[2];
                CoordValueZ = Coordinates[3];
            }
        }

        private void SelectedKey(string key)
        {
            MovementMethod.SelectedMovementMethod(key);
        }
    }
}

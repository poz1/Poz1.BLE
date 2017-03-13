using Poz1.BLE.Core;
using Poz1.BLE.Sample.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Poz1.BLE.Sample.ViewModel
{
    class DevicesViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public ObservableCollection<IBLEDevice> DevicesList { get; set; }
    
        bool deviceScanning;
        public bool DeviceScanning
        {
            get { return deviceScanning; }
            set
            {
                deviceScanning = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeviceSelectedCommand { get { return new Command<IBLEDevice>(async x => await NavigateToServicesPage(x)); } }
        public ICommand StartScanCommand { get { return new Command(async () => await GetDevices()); } }

        public DevicesViewModel()
        {
            DevicesList = new ObservableCollection<IBLEDevice>();
        }

        private async Task NavigateToServicesPage(IBLEDevice device)
        {
            await Navigation.PushAsync(new ServicesPage(device));
        }

        public async Task GetDevices()
        {
            DeviceScanning = true;

            DevicesList.Clear();
            try
            {
                var list = await DependencyService.Get<IBLEAdapter>().ScanForDevicesAsync();
                foreach (var device in list)
                {
                    DevicesList.Add(device);
                }
            }
            catch(Exception ey)
            {
            }
      

            DeviceScanning = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

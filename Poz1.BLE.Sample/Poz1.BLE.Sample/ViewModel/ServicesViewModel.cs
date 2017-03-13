using Poz1.BLE.Core;
using Poz1.BLE.Sample.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Poz1.BLE.Sample.ViewModel
{
    class ServicesViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public ObservableCollection<IBLEService> ServicesList { get; set; }

        bool scanForServices;
        public bool ScanForServices
        {
            get { return scanForServices; }
            set
            {
                scanForServices = value;
                OnPropertyChanged();
            }
        }

        public ICommand ServiceSelectedCommand { get { return new Command<IBLEService>(async x => await NavigateToCharacteristicsPage(x)); } }
        public ServicesViewModel(IBLEDevice device)
        {
            ServicesList = new ObservableCollection<IBLEService>();
            App.Device = device;
        }

        public async Task GetServicesAsync()
        {
            ScanForServices = true;

            await App.Device.ConnectAsync();

            ServicesList.Clear();

            var list = await App.Device.GetServicesAsync();
            foreach (var service in list)
            {
                ServicesList.Add(service);
            }

            ScanForServices = false;
        }

        private async Task NavigateToCharacteristicsPage(IBLEService service)
        {
            await Navigation.PushAsync(new CharacteristicsPage(service));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

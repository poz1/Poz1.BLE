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
    class CharacteristicsViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public ObservableCollection<IBLECharacteristic> CharacteristicsList { get; set; }

        public ICommand CharacteristicSelectedCommand { get { return new Command<IBLECharacteristic>(async x => await NavigateToCharacteristicsPage(x)); } }
        public CharacteristicsViewModel(IBLEService service)
        {
            CharacteristicsList = new ObservableCollection<IBLECharacteristic>(service.Characteristics);
        }

        private async Task NavigateToCharacteristicsPage(IBLECharacteristic characteristic)
        {
            await Navigation.PushAsync(new CharacteristicDetailPage(characteristic));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

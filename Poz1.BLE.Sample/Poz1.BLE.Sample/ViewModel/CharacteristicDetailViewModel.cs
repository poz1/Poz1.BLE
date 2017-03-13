using Poz1.BLE.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Poz1.BLE.Sample.ViewModel
{
    class CharacteristicDetailViewModel : INotifyPropertyChanged
    {
        public IBLECharacteristic Characteristic { get; private set; }

        string value;
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadValueCommand { get { return new Command(async () => await GetValue()); } }

        public CharacteristicDetailViewModel(IBLECharacteristic characteristic)
        {
            Characteristic = characteristic;
        }

        public async Task GetValue()
        {
            if (Characteristic.Properties == CharacteristicPropertyType.Read)
            {
                var byteArray = (await App.Device.ReadCharacteristicAsync(Characteristic)).Value;
                Value = System.Text.Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            }
            else
                Value = "This Characteristic does not support Reading";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

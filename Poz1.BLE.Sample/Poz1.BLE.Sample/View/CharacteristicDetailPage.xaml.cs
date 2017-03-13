using Poz1.BLE.Core;
using Poz1.BLE.Sample.ViewModel;
using Xamarin.Forms;

namespace Poz1.BLE.Sample.View
{
    public partial class CharacteristicDetailPage : ContentPage
    {
        private CharacteristicDetailViewModel vm;
        public CharacteristicDetailPage(IBLECharacteristic characteristic)
        {
            InitializeComponent();
            BindingContext = vm = new CharacteristicDetailViewModel(characteristic);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await vm.GetValue();
        }
    }
}

using Poz1.BLE.Core;
using Poz1.BLE.Sample.ViewModel;
using Xamarin.Forms;

namespace Poz1.BLE.Sample.View
{
    public partial class ServicesPage : ContentPage
    {
        private ServicesViewModel vm;

        public ServicesPage(IBLEDevice device)
        {
            InitializeComponent();
            BindingContext = vm =  new ServicesViewModel(device) { Navigation = Navigation};
        }

        protected async override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            await vm.GetServicesAsync();
        }
    }
}

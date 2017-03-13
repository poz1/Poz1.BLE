using Poz1.BLE.Sample.ViewModel;
using Xamarin.Forms;

namespace Poz1.BLE.Sample.View
{
    public partial class DevicesPage : ContentPage
    {
        private DevicesViewModel vm;
        public DevicesPage()
        {
            InitializeComponent();
            BindingContext = vm = new DevicesViewModel() { Navigation = Navigation };
        }
        protected async override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            await vm.GetDevices();
        }
    }
}

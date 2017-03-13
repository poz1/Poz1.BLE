using Poz1.BLE.Core;
using Poz1.BLE.Sample.ViewModel;
using Xamarin.Forms;

namespace Poz1.BLE.Sample.View
{
    public partial class CharacteristicsPage : ContentPage
    {
        public CharacteristicsPage(IBLEService service)
        {
            InitializeComponent();
            BindingContext = new CharacteristicsViewModel(service) { Navigation = Navigation};
        }
    }
}

using System.Windows.Input;
using Xamarin.Forms;

namespace Poz1.BLE.Sample.Control
{
    class BindableListView : ListView
    {
        public static BindableProperty ItemClickCommandProperty = BindableProperty.Create(nameof(ItemClickCommand), typeof(ICommand), typeof(BindableListView), null);
        public BindableListView()
        {
            ItemTapped += OnItemTapped;
        }

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && ItemClickCommand != null && ItemClickCommand.CanExecute(e))
            {
                ItemClickCommand.Execute(e.Item);
                SelectedItem = null;
            }
        }
    }
}

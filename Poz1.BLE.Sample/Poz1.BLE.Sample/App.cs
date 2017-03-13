using Poz1.BLE.Core;
using Poz1.BLE.Sample.View;
using Xamarin.Forms;

namespace Poz1.BLE.Sample
{
    public class App : Application
    {
        public static IBLEDevice Device { get; set; }

        public App()
        {
            MainPage = new NavigationPage(new DevicesPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Poz1.BLE.Core;
using Poz1.BLE.Droid;

namespace Poz1.BLE.Sample.Droid
{
    [Activity(Label = "Poz1.BLE.Sample", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.DependencyService.Register<IBLEAdapter, BLEAdapter>();
            Xamarin.Forms.DependencyService.Register<IBLEDevice, BLEDevice>();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}


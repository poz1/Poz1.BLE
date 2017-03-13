using Poz1.BLE.Core;
using Poz1.BLE.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Poz1.BLE.Sample.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            Xamarin.Forms.DependencyService.Register<IBLEAdapter, Adapter>();
            Xamarin.Forms.DependencyService.Register<IBLEDevice,Poz1.BLE.UWP.BLEDevice>();

            LoadApplication(new Poz1.BLE.Sample.App());
        }
    }
}

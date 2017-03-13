using Poz1.BLE.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Devices.Radios;

namespace Poz1.BLE.UWP
{
    public class Adapter : IBLEAdapter
    {
        // Request the IsPaired property so we can display the paired status in the UI
        private string[] _requestedProperties = { "System.Devices.Aep.IsPaired" };
        private string _filter = "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"";
        //for bluetooth LE Devices

        private DeviceWatcher _watcher;

        private List<IBLEDevice> _discoveredDevices;
        private TaskCompletionSource<IList<IBLEDevice>> _scanForDevicesTCS;
        private CancellationTokenSource cancelScan;

        public TimeSpan ScanTimeout { get; private set; }
        public bool IsScanning { get; private set; }

        public Adapter() : this(TimeSpan.FromSeconds(10))
        { }
        public Adapter(TimeSpan scanTimeout)
        {
            ScanTimeout = scanTimeout;
        }

        public Task<IList<IBLEDevice>> ScanForDevicesAsync()
        {   
            _scanForDevicesTCS = new TaskCompletionSource<IList<IBLEDevice>>();
            cancelScan = new CancellationTokenSource();
            try
            {
                _watcher = DeviceInformation.CreateWatcher(_filter, _requestedProperties, DeviceInformationKind.AssociationEndpoint);
                _watcher.EnumerationCompleted += _watcher_EnumerationCompleted;
                _watcher.Added += _watcher_Added;

                _discoveredDevices = new List<IBLEDevice>();
                IsScanning = true;

                _watcher.Start();

                Task.Run(async () =>
                {
                    if (!await GetBluetoothIsEnabledAsync())
                        _scanForDevicesTCS.TrySetException(new Exception("Bluetooth is Disabled"));

                    await Task.Delay((int)ScanTimeout.TotalMilliseconds,cancelScan.Token);

                    cancelScan.Token.ThrowIfCancellationRequested();

                    if (IsScanning)
                    {
                        IsScanning = false;
                        _watcher.Stop();
                        _scanForDevicesTCS.SetResult(_discoveredDevices);
                    }

                }, cancelScan.Token);
            }
            catch(Exception e)
            {
                _scanForDevicesTCS.TrySetException(e);
            }
            return _scanForDevicesTCS.Task;
        }

        private static async Task<bool> GetBluetoothIsEnabledAsync()
        {
            var radios = await Radio.GetRadiosAsync();
            var bluetoothRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth);
            return bluetoothRadio != null && bluetoothRadio.State == RadioState.On;
        }

        private async void _watcher_Added(DeviceWatcher sender, DeviceInformation dvc)
        {
            var bluetoothDevice = await BluetoothLEDevice.FromIdAsync(dvc.Id);

            var device = new BLEDevice(bluetoothDevice);
            if (_discoveredDevices.All(x => x.Guid != device.Guid))
            {
                _discoveredDevices.Add(device);
            }
        }

        private void _watcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            IsScanning = false;
            cancelScan.Cancel();
            _scanForDevicesTCS.SetResult(_discoveredDevices);
        }
    }
}

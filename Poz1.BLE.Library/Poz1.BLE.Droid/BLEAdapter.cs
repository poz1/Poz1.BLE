using Android.App;
using Android.Bluetooth;
using Poz1.BLE.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Bluetooth.LE;
using Android.Runtime;

namespace Poz1.BLE.Droid
{
    public class BLEAdapter : ScanCallback, IBLEAdapter
	{
		private readonly BluetoothLeScanner _adapter;

        private List<IBLEDevice> _discoveredDevices;
        private TaskCompletionSource<IList<IBLEDevice>> _scanForDevicesTCS;

        public BLEAdapter() : this(TimeSpan.FromSeconds(10))
        { }
        public BLEAdapter(TimeSpan scanTimeout)
        {
            _adapter = ((BluetoothManager)Application.Context.GetSystemService("bluetooth")).Adapter.BluetoothLeScanner;
            ScanTimeout = scanTimeout;
        }

		public Task<IList<IBLEDevice>> ScanForDevicesAsync()
		{
            _scanForDevicesTCS = new TaskCompletionSource<IList<IBLEDevice>>();
            _discoveredDevices = new List<IBLEDevice>();

            IsScanning = true;

            _adapter.StartScan(this);

            Task.Run(async () =>
            {
                await Task.Delay((int)ScanTimeout.TotalMilliseconds);
                if (IsScanning)
                {
                    IsScanning = false;
                    _adapter.StopScan(this);

                    _scanForDevicesTCS.SetResult(_discoveredDevices);
                }
            });

            return _scanForDevicesTCS.Task;
        }

		public bool IsScanning { get; private set; }

		public TimeSpan ScanTimeout { get; private set; }

        public override void OnScanResult([GeneratedEnum] ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);

            var device = new BLEDevice(result.Device);
            if (_discoveredDevices.All(x => x.Guid != device.Guid))
            {
                _discoveredDevices.Add(device);
            }
        }
    }
}


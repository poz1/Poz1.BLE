using CoreBluetooth;
using Poz1.BLE.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using CoreFoundation;

namespace Poz1.BLE.iOS
{
    public class BLEAdapter : IBLEAdapter
    {
        CentralManagerDelegate _centralManagerDelegate;
        CBCentralManager _centralManager;

        private List<IBLEDevice> _discoveredDevices;
        private TaskCompletionSource<IList<IBLEDevice>> _scanForDevicesTCS;

        public BLEAdapter() : this(TimeSpan.FromSeconds(10))
        { }
        public BLEAdapter(TimeSpan scanTimeout)
        {
            ScanTimeout = scanTimeout;
            _centralManagerDelegate = new CentralManagerDelegate(ScanTimeout, _isScanning, _scanForDevicesTCS);
            _centralManager = new CBCentralManager(_centralManagerDelegate, DispatchQueue.CurrentQueue);

        }

        public bool IsScanning { get { return _isScanning; } }
        private Boolean _isScanning;

        public TimeSpan ScanTimeout { get; private set; }
        public Task<IList<IBLEDevice>> ScanForDevicesAsync()
        {
            _scanForDevicesTCS = new TaskCompletionSource<IList<IBLEDevice>>();
            _discoveredDevices = new List<IBLEDevice>();

            _isScanning = true;

            Task.Run(() => _centralManagerDelegate.ScanForDevices(_centralManager));

            return _scanForDevicesTCS.Task;
        }
    }
}
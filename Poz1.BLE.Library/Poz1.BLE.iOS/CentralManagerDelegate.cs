using System;
using System.Collections.Generic;
using System.Linq;
using CoreBluetooth;
using System.Threading.Tasks;
using Poz1.BLE.Core;
using Foundation;
using Poz1.BLE.iOS;

namespace Poz1.BLE.iOS
{
    class CentralManagerDelegate : CBCentralManagerDelegate
    {
        private TimeSpan _scanTimeout;
        private DeviceTaskCompletitionSource _centralManagerTaskCompletitionSource;
        private TaskCompletionSource<IList<IBLEDevice>> _scanForDevicesTCS;

        List<IBLEDevice> _discoveredDevices;
        Boolean _isScanning;

        public CentralManagerDelegate(DeviceTaskCompletitionSource centralManagerTaskCompletitionSource)
        {
            _centralManagerTaskCompletitionSource = centralManagerTaskCompletitionSource;
        }

        public CentralManagerDelegate(TimeSpan timeout, Boolean isScanning, TaskCompletionSource<IList<IBLEDevice>> scanForDevicesTCS)
        {
            _scanTimeout = timeout;
            _scanForDevicesTCS = scanForDevicesTCS;
            _isScanning = isScanning;
            _discoveredDevices = new List<IBLEDevice>();
        }

        public async Task ScanForDevices(CBCentralManager manager)
        {
            if (manager.State == CBCentralManagerState.PoweredOn)
            {
                //Passing in null scans for all peripherals. Peripherals can be targeted by using CBUIIDs
                CBUUID[] cbuuids = null;
                manager.ScanForPeripherals(cbuuids);

                await Task.Delay((int)_scanTimeout.TotalMilliseconds);

                manager.StopScan();

                _isScanning = false;
                _scanForDevicesTCS.SetResult(_discoveredDevices);
            }
            else
            {
                Console.WriteLine("Bluetooth is not available");
                _scanForDevicesTCS.SetException(new Exception("Bluetooth is not available"));
            }
        }

        override public void UpdatedState(CBCentralManager manager)
        {
            
        }


        public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
        {
            Console.WriteLine("Discovered {0}, data {1}, RSSI {2}", peripheral.Name, advertisementData, RSSI);
            _discoveredDevices.Add(new BLEDevice(peripheral, int.Parse(RSSI.ToString())));
        }

        public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            base.ConnectedPeripheral(central, peripheral);
            _centralManagerTaskCompletitionSource.ConnectTCS.SetResult(true);
        }

        public override void FailedToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
        {
            base.FailedToConnectPeripheral(central, peripheral, error);
            _centralManagerTaskCompletitionSource.ConnectTCS.SetException(new Exception("Error connection to device: " + peripheral?.Name + " Details: " + error.Description.ToString()));
        }
    }
}
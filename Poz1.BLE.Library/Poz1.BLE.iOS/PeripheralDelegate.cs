using CoreBluetooth;
using System.Collections.Generic;
using Foundation;
using Poz1.BLE.Core;

namespace Poz1.BLE.iOS
{
    class PeripheralDelegate : CBPeripheralDelegate
    {
        List<IBLEService> servicesList;
        List<IBLECharacteristic> characteristicList;

        DeviceTaskCompletitionSource _deviceTaskCompletitionSource;

        public PeripheralDelegate(DeviceTaskCompletitionSource deviceTaskCompletitionSource)
        {
            _deviceTaskCompletitionSource = deviceTaskCompletitionSource;
        
            servicesList = new List<IBLEService>();
        }
        public override void RssiRead(CBPeripheral peripheral, NSNumber rssi, NSError error)
        {
            base.RssiRead(peripheral, rssi, error);
            _deviceTaskCompletitionSource.RssiTCS.SetResult(rssi.Int32Value);
        }

        //TODO Submit Bug to change the methid name to "DiscoveredServices" as this method will fire only when all the Services are discovered
        public async override void DiscoveredService(CBPeripheral peripheral, NSError error)
        {
            base.DiscoveredService(peripheral, error);

            foreach (var item in peripheral.Services)
            {
                var service = new BLEService(item);

                if (peripheral.Delegate == null)
                    peripheral.Delegate = this;

                service.Characteristics = await service.GetCharacteristicsAsync(peripheral, _deviceTaskCompletitionSource.CharacteristicDiscoveryTCS);
                servicesList.Add(service);
            }

            _deviceTaskCompletitionSource.ServicesDiscoveryTCS.SetResult(servicesList);
        }

        //TODO Submit Bug to change the methid name to "DiscoveredCharacteristics" as this method will fire only when all the Characteristics are discovered
        public override void DiscoveredCharacteristic(CBPeripheral peripheral, CBService service, NSError error)
        {
            base.DiscoveredCharacteristic(peripheral, service, error);

            foreach (var item in service.Characteristics)
            {
                var characteristic = new BLECharacteristic(item);

                if (peripheral.Delegate == null)
                    peripheral.Delegate = this;

                characteristicList.Add(characteristic);
            }

            _deviceTaskCompletitionSource.CharacteristicDiscoveryTCS.SetResult(characteristicList);
        }

        public override void UpdatedCharacterteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, NSError error)
        {
            //Read
            base.UpdatedCharacterteristicValue(peripheral, characteristic, error);
        }
    }
}

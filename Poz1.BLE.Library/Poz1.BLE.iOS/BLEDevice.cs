using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Poz1.BLE.Core;
using CoreBluetooth;
using CoreFoundation;

namespace Poz1.BLE.iOS
{
    public class BLEDevice : IBLEDevice
    {
        private readonly CBCentralManager _centralManager;
        private readonly CentralManagerDelegate _centralManagerDelegate;

        private readonly PeripheralDelegate _peripheralDelegate;

        private readonly CBPeripheral _device;
        private DeviceTaskCompletitionSource deviceTaskCompletitionSource;

        public BLEDevice(CBPeripheral device, int RSSI)
        {
            deviceTaskCompletitionSource = new DeviceTaskCompletitionSource();
            _centralManager = new CBCentralManager(_centralManagerDelegate = new CentralManagerDelegate(deviceTaskCompletitionSource) , DispatchQueue.CurrentQueue);
            _peripheralDelegate = new PeripheralDelegate(deviceTaskCompletitionSource);
            _device = device;

            Guid = Guid.Parse(_device.Identifier?.AsString());
            Name = _device.Name?.ToString();
            Rssi = RSSI;
        }

        #region Properties

        public string Name { get; private set; }
        public string MacAddress { get; private set; }
        public Guid Guid { get; private set; }
        public int Rssi { get; private set; }
        public DeviceState State { get; private set; }
        public List<IBLEService> Services { get; private set; }

        #endregion

        public event EventHandler<CharacteristicEventArgs> CharacteristicChanged;
        public event EventHandler DeviceDisconnected;

        public Task ConnectAsync()
        {
            deviceTaskCompletitionSource.ConnectTCS = new TaskCompletionSource<object>();
            _centralManager.ConnectPeripheral(_device);
            return deviceTaskCompletitionSource.ConnectTCS.Task;
        }

        public void Disconnect()
        {
            _centralManager.CancelPeripheralConnection(_device);
        }

        public Task<int> GetRssiAsync()
        {
            deviceTaskCompletitionSource.RssiTCS = new TaskCompletionSource<int>();
            _device.ReadRSSI();
            return deviceTaskCompletitionSource.RssiTCS.Task;
        }

        public Task<IBLECharacteristic> ReadCharacteristicAsync(string serviceUUID, string characteristicUUID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SubscribeCharacteristic(string serviceUUID, string characteristicId, string descriptorGUID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnsubscribeCharacteristic(string serviceUUID, string characteristicId, string descriptorGUID)
        {
            throw new NotImplementedException();
        }

        public Task<IBLECharacteristic> WriteCharacteristicAsync(string serviceUUID, string characteristicUUID, byte[] valueToWrite)
        {
            throw new NotImplementedException();
        }

        public Task<List<IBLEService>> GetServicesAsync()
        {
            deviceTaskCompletitionSource.ServicesDiscoveryTCS = new TaskCompletionSource<List<IBLEService>>();
            _device.DiscoverServices();
            return deviceTaskCompletitionSource.ServicesDiscoveryTCS.Task;
        }

        public override string ToString()
        {
            return Name + " - " + Guid;
        }

        public Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IBLECharacteristic> WriteCharacteristicAsync(IBLECharacteristic characteristic, byte[] valueToWrite)
        {
            throw new NotImplementedException();
        }

        public Task<IBLECharacteristic> ReadCharacteristicAsync(IBLECharacteristic characteristic)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SubscribeCharacteristic(IBLECharacteristic characteristic, string descriptorGUID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnsubscribeCharacteristic(IBLECharacteristic characteristic, string descriptorGUID)
        {
            throw new NotImplementedException();
        }
    }
}
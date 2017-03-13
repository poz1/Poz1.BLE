using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Poz1.BLE.Core;
using Windows.Devices.Bluetooth;

namespace Poz1.BLE.UWP
{
    public class BLEDevice : IBLEDevice
    {
        #region Private Fields

        private readonly BluetoothLEDevice _nativeDevice;
        //private BluetoothManager _bluetoothManager;
        //private BluetoothGatt _gatt;

        private TaskCompletionSource<IBLECharacteristic> writeCharacteristicTCS;
        private TaskCompletionSource<IBLECharacteristic> readCharacteristicTCS;
        private TaskCompletionSource<List<IBLEService>> servicesDiscoveryTCS;
        private TaskCompletionSource<int> rssiTCS;
        private TaskCompletionSource<bool> subscribeCharacteristicTCS;
        private TaskCompletionSource<bool> unsubscribeCharacteristicTCS;

        #endregion

        public event EventHandler DeviceDisconnected;
        public event EventHandler<CharacteristicEventArgs> CharacteristicChanged;
        public BLEDevice(BluetoothLEDevice nativeDevice)
        {
            //_bluetoothManager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService);
            _nativeDevice = nativeDevice;
            Name = _nativeDevice.Name;
            MacAddress = _nativeDevice.BluetoothAddress.ToString();
            //_guid = Guid.Parse(_nativeDevice.DeviceId);
        }

        #region Properties

        public string Name { get; private set; }
        public string MacAddress { get; private set; }
        public Guid Guid { get; private set; }
        public DeviceState State { get; private set; }

        #endregion

        public Task ConnectAsync()
        {
            throw new NotImplementedException();
        }
        public Task<List<IBLEService>> GetServicesAsync()
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetRssiAsync()
        {
            throw new NotImplementedException();
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
        public override string ToString()
        {
            return Name + " - " + Guid;
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
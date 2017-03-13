using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Runtime;
using Java.Util;
using Poz1.BLE.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Poz1.BLE.Droid
{
    public class BLEDevice : BluetoothGattCallback, IBLEDevice
	{
        #region Private Fields

        private readonly BluetoothDevice _nativeDevice;
        private BluetoothManager _bluetoothManager;
        private BluetoothGatt _gatt;

        private TaskCompletionSource<bool> connectTCS;
        private TaskCompletionSource<bool> disconnectTCS;

        private TaskCompletionSource<IBLECharacteristic> writeCharacteristicTCS;
        private TaskCompletionSource<IBLECharacteristic> readCharacteristicTCS;
        private TaskCompletionSource<List<IBLEService>> servicesDiscoveryTCS;
        private TaskCompletionSource<int> rssiTCS;
        private TaskCompletionSource<bool> subscribeCharacteristicTCS;
        private TaskCompletionSource<bool> unsubscribeCharacteristicTCS;

        #endregion

        public event EventHandler DeviceDisconnected;
        public event EventHandler<CharacteristicEventArgs> CharacteristicChanged;        

        public BLEDevice(BluetoothDevice nativeDevice)
        {
            _bluetoothManager = (BluetoothManager)Application.Context.GetSystemService(Context.BluetoothService);
            _nativeDevice = nativeDevice;

            Name = _nativeDevice.Name;
            MacAddress = _nativeDevice.Address;
            Guid = DeviceIdFromAddress(_nativeDevice.Address);
        }

        #region Properties

        public string Name { get; private set; }
        public string MacAddress { get; private set; }
		public Guid Guid { get; private set; }
    	public DeviceState State { get; private set; }

        #endregion

        #region Public Methods

        public Task ConnectAsync()
        {
            connectTCS = new TaskCompletionSource<bool>();

            try
            {
                _gatt = _nativeDevice.ConnectGatt(Application.Context, false, this);
                var connectionState = _bluetoothManager.GetConnectionState(_nativeDevice, ProfileType.Gatt);

                if (connectionState != ProfileState.Connected)
                    _gatt.Connect();
            }
            catch(Exception e)
            {
                connectTCS.TrySetException(e);
            }

            return connectTCS.Task;
        }
        public Task DisconnectAsync()
        {
            disconnectTCS = new TaskCompletionSource<bool>();

            try
            {
                _gatt.Disconnect();
            }
            catch (Exception ex)
            {
                disconnectTCS.TrySetException(ex);
            }

            return disconnectTCS.Task;
        }

        public Task<IBLECharacteristic> WriteCharacteristicAsync(string serviceGUID, string characteristicGUID, byte[] valueToWrite)
        {
            writeCharacteristicTCS = new TaskCompletionSource<IBLECharacteristic>();

            try
            {
                if (_gatt == null)
                {
                    Debug.WriteLine("Connect to Bluetooth Device first");
                    writeCharacteristicTCS.TrySetException(new Exception("Connect to Bluetooth Device first"));
                }

                BluetoothGattCharacteristic characteristic;

                characteristic = _gatt.GetService(UUID.FromString(serviceGUID)).GetCharacteristic(UUID.FromString(characteristicGUID));

                if (characteristic == null)
                {
                    writeCharacteristicTCS.TrySetException(new Exception("Bluetooth Gatt Characteristic " + characteristicGUID + " Does Not Exist"));
                }

                characteristic.SetValue(valueToWrite);

                if (_gatt.WriteCharacteristic(characteristic) == false)
                {
                    writeCharacteristicTCS.TrySetException(new Exception("WriteCharacteristic was not initiated successfully"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                writeCharacteristicTCS.TrySetException(e);
            }

            return writeCharacteristicTCS.Task;
        }
        public Task<IBLECharacteristic> WriteCharacteristicAsync(IBLECharacteristic characteristic, byte[] valueToWrite)
        {
            return WriteCharacteristicAsync(characteristic.ServiceGuid.ToString(), characteristic.Guid.ToString(), valueToWrite);
        }
        public Task<IBLECharacteristic> ReadCharacteristicAsync(string serviceGUID, string characteristicGUID)
        {
            readCharacteristicTCS = new TaskCompletionSource<IBLECharacteristic>();

            try
            {
                if (_gatt == null)
                {
                    Debug.WriteLine("Connect to Bluetooth Device first");
                    readCharacteristicTCS.TrySetException(new Exception("Connect to Bluetooth Device first"));
                }

                BluetoothGattCharacteristic chara = _gatt.GetService(UUID.FromString(serviceGUID)).GetCharacteristic(UUID.FromString(characteristicGUID));

                if (null == chara)
                {
                    readCharacteristicTCS.TrySetException(new Exception("Bluetooth Gatt Characteristic " + characteristicGUID + " Does Not Exist"));
                }
                if (false == _gatt.ReadCharacteristic(chara))
                {
                    readCharacteristicTCS.TrySetException(new Exception("ReadCharacteristic was not initiated successfully"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                readCharacteristicTCS.TrySetException(new Exception(e.Message));
            }

            return readCharacteristicTCS.Task;
        }
        public Task<IBLECharacteristic> ReadCharacteristicAsync(IBLECharacteristic characteristic)
        {
            return ReadCharacteristicAsync(characteristic.ServiceGuid.ToString(), characteristic.Guid.ToString());
        }
        public Task<bool> SubscribeCharacteristic(string serviceGUID, string characteristicGUID, string descriptorGUID)
        {
            subscribeCharacteristicTCS = new TaskCompletionSource<bool>();

            try
            {
                if (_gatt == null)
                {
                    Debug.WriteLine("Connect to Bluetooth Device first");
                    subscribeCharacteristicTCS.TrySetException(new Exception("Connect to Bluetooth Device first"));
                }

                BluetoothGattCharacteristic chara = _gatt.GetService(UUID.FromString(serviceGUID)).GetCharacteristic(UUID.FromString(characteristicGUID));
                if (null == chara)
                {
                    subscribeCharacteristicTCS.TrySetException(new Exception("Characteristic Id: " + characteristicGUID + " Not Found in Service: " + serviceGUID));
                }

                _gatt.SetCharacteristicNotification(chara, true);

                BluetoothGattDescriptor descriptor = chara.GetDescriptor(UUID.FromString(descriptorGUID));
                descriptor.SetValue(BluetoothGattDescriptor.EnableNotificationValue.ToArray());
                _gatt.WriteDescriptor(descriptor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                subscribeCharacteristicTCS.TrySetException(new Exception(e.Message));
            }

            return subscribeCharacteristicTCS.Task;
        }
        public Task<bool> SubscribeCharacteristic(IBLECharacteristic characteristic, string descriptorGUID)
        {
            return SubscribeCharacteristic(characteristic.ServiceGuid.ToString(), characteristic.Guid.ToString(),descriptorGUID);
        }
        public Task<bool> UnsubscribeCharacteristic(string serviceGUID, string characteristicGUID, string descriptorGUID)
        {
            unsubscribeCharacteristicTCS = new TaskCompletionSource<bool>();

            try
            {
                if (_gatt == null)
                {
                    Debug.WriteLine("Connect to Bluetooth Device first");
                    unsubscribeCharacteristicTCS.TrySetException(new Exception("Connect to Bluetooth Device first"));
                }

                BluetoothGattCharacteristic chara = _gatt.GetService(UUID.FromString(serviceGUID)).GetCharacteristic(UUID.FromString(characteristicGUID));
                if (null == chara)
                {
                    subscribeCharacteristicTCS.TrySetException(new Exception("Characteristic Id: " + characteristicGUID + " Not Found in Service: " + serviceGUID));
                }

                _gatt.SetCharacteristicNotification(chara, false);

                BluetoothGattDescriptor descriptor = chara.GetDescriptor(UUID.FromString(descriptorGUID));
                descriptor.SetValue(BluetoothGattDescriptor.DisableNotificationValue.ToArray());
                _gatt.WriteDescriptor(descriptor);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                unsubscribeCharacteristicTCS.TrySetException(new Exception(e.Message));
            }

            return unsubscribeCharacteristicTCS.Task;
        }
        public Task<bool> UnsubscribeCharacteristic(IBLECharacteristic characteristic, string descriptorGUID)
        {
            return UnsubscribeCharacteristic(characteristic.ServiceGuid.ToString(), characteristic.Guid.ToString(), descriptorGUID);
        }

        public Task<int> GetRssiAsync()
        {
            rssiTCS = new TaskCompletionSource<int>();
            try
            {
                if (_gatt == null)
                {
                    Debug.WriteLine("Connect to Bluetooth Device first");
                    rssiTCS.TrySetException(new Exception("Connect to Bluetooth Device first"));
                }

                var successfullRequest = _gatt.ReadRemoteRssi();

                if (!successfullRequest)
                    rssiTCS.TrySetException(new Exception("Cannot request RSSI"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                rssiTCS.TrySetException(e);
            }

            return rssiTCS.Task;
        }
        public Task<List<IBLEService>> GetServicesAsync()
        {
            servicesDiscoveryTCS = new TaskCompletionSource<List<IBLEService>>();

            try
            {

                if (_gatt == null)
                {
                    Debug.WriteLine("Connect to Bluetooth Device first");
                    servicesDiscoveryTCS.TrySetException(new Exception("Connect to Bluetooth Device first"));
                }

               var x = _gatt.DiscoverServices();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                servicesDiscoveryTCS.TrySetException(e);
            }

            return servicesDiscoveryTCS.Task;
        }
        #endregion

        #region Private Methods
        private Guid DeviceIdFromAddress(string address)
        {
            var deviceGuid = new Byte[16];
            var macWithoutColons = address.Replace(":", "");
            var macBytes = Enumerable.Range(0, macWithoutColons.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(macWithoutColons.Substring(x, 2), 16))
                .ToArray();
            macBytes.CopyTo(deviceGuid, 10);

            return new Guid(deviceGuid);
        }  
        #endregion

        #region BluetoothGattCallback Implementation
        public override void OnConnectionStateChange(BluetoothGatt gatt, [GeneratedEnum] GattStatus status, [GeneratedEnum] ProfileState newState)
        {
            base.OnConnectionStateChange(gatt, status, newState);
            if (newState == ProfileState.Connected)
            {
                State = DeviceState.Connected;
                connectTCS.TrySetResult(true);
            }
            else if (newState == ProfileState.Disconnected)
            {
                gatt.Close();
                State = DeviceState.Disconnected;
                //The device can disconnect by itself, not only because we asked
                disconnectTCS?.TrySetResult(true);

                OnDeviceDisconnected();
            }
        }
        public override void OnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, [GeneratedEnum] GattStatus status)
        {
            base.OnCharacteristicRead(gatt, characteristic, status);

            if (GattStatus.Success == status)
            {
                readCharacteristicTCS.TrySetResult(new BLECharacteristic(characteristic));
            }
            else
            {
                Debug.WriteLine("onCharacteristicRead fail");
                readCharacteristicTCS.TrySetException(new Exception("onCharacteristicRead fail"));
            }
        }
        public override void OnCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, [GeneratedEnum] GattStatus status)
        {
            base.OnCharacteristicWrite(gatt, characteristic, status);
            if (GattStatus.Success == status)
            {
                writeCharacteristicTCS.TrySetResult(new BLECharacteristic(characteristic));
            }
            else
            {
                Debug.WriteLine("onCharacteristicWrite fail");
                writeCharacteristicTCS.TrySetException(new Exception("onCharacteristicWrite fail"));
            }
        }
        public override void OnCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            base.OnCharacteristicChanged(gatt, characteristic);
            OnCharacteristicChange(new BLECharacteristic(characteristic));
        }
        public override void OnReadRemoteRssi(BluetoothGatt gatt, int rssi, [GeneratedEnum] GattStatus status)
        {
            base.OnReadRemoteRssi(gatt, rssi, status);
            if (GattStatus.Success == status)
            {
                rssiTCS.TrySetResult(rssi);
            }
            else
            {
                Debug.WriteLine("onCharacteristicRead fail");
                rssiTCS.TrySetException(new Exception("onCharacteristicRead fail"));
            }
        }
        public override void OnServicesDiscovered(BluetoothGatt gatt, [GeneratedEnum] GattStatus status)
        {
            base.OnServicesDiscovered(gatt, status);
            if (GattStatus.Success == status)
            {
                var services = new List<IBLEService>();
                foreach (var item in gatt.Services)
                {
                    var service = new BLEService(item, this);
                    services.Add(service);
                }
                servicesDiscoveryTCS.TrySetResult(services);
            }
            else
            {
                Debug.WriteLine("onServicesDiscovered fail");
                servicesDiscoveryTCS.TrySetException(new Exception("onServicesDiscovered fail"));
            }
        }    
        public override void OnDescriptorWrite(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, [GeneratedEnum] GattStatus status)
        {
            base.OnDescriptorWrite(gatt, descriptor, status);
            if (GattStatus.Success == status)
            {
                try
                {
                    subscribeCharacteristicTCS.TrySetResult(true);
                }
                catch
                {
                    unsubscribeCharacteristicTCS.TrySetResult(true);
                }
            }
            else
            {
                Debug.WriteLine("onCharacteristicRead fail");
                try
                {
                    subscribeCharacteristicTCS.TrySetException(new Exception("OnDescriptorWrite fail"));
                }
                catch
                {
                    unsubscribeCharacteristicTCS.TrySetException(new Exception("OnDescriptorWrite fail"));
                }
            }
        }

        public void OnDeviceDisconnected()
        {
            DeviceDisconnected?.Invoke(null, null);
        }
        public void OnCharacteristicChange(IBLECharacteristic characteristic)
        {
            CharacteristicChanged?.Invoke(null, new CharacteristicEventArgs(characteristic));
        }
        #endregion

        public override string ToString()
        {
            return Name + " - " + Guid;
        }
    }
}


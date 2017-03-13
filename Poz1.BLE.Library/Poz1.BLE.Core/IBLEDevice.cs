using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poz1.BLE.Core
{
	/// <summary>
	/// The device interface.
	/// </summary>
	public interface IBLEDevice
	{
        event EventHandler DeviceDisconnected;
        event EventHandler<CharacteristicEventArgs> CharacteristicChanged;

        Guid Guid { get; }
        string MacAddress { get; }
        string Name { get; }
		DeviceState State { get; }
       

        Task ConnectAsync();
        Task DisconnectAsync();

        Task<int> GetRssiAsync();
        Task<List<IBLEService>> GetServicesAsync();

        Task<IBLECharacteristic> WriteCharacteristicAsync(string serviceUUID, string characteristicGUID, byte[] valueToWrite);
        Task<IBLECharacteristic> WriteCharacteristicAsync(IBLECharacteristic characteristic, byte[] valueToWrite);

        Task<IBLECharacteristic> ReadCharacteristicAsync(string serviceUUID, string characteristicGUID);
        Task<IBLECharacteristic> ReadCharacteristicAsync(IBLECharacteristic characteristic);

        Task<bool> SubscribeCharacteristic(string serviceUUID, string characteristicId, string descriptorGUID);
        Task<bool> SubscribeCharacteristic(IBLECharacteristic characteristic, string descriptorGUID);

        Task<bool> UnsubscribeCharacteristic(string serviceUUID, string characteristicId, string descriptorGUID);
        Task<bool> UnsubscribeCharacteristic(IBLECharacteristic characteristic, string descriptorGUID);
    }
}


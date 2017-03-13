using Android.Bluetooth;
using Poz1.BLE.Core;
using System;
using System.Collections.Generic;

namespace Poz1.BLE.Droid
{
    public class BLEService : IBLEService
	{
		private readonly BluetoothGattService _nativeService;
        private readonly IBLEDevice _device;
		public BLEService(BluetoothGattService nativeService, IBLEDevice device)
		{
            _nativeService = nativeService;
            _device = device;
            Characteristics = new List<IBLECharacteristic>();

            Guid = GuidHelper.FromUUID(_nativeService.Uuid);
            IsPrimary = (_nativeService.Type == GattServiceType.Primary);

            foreach (var item in _nativeService.Characteristics)
            {
                var characteristic = new BLECharacteristic(item);
                Characteristics.Add(characteristic);
            }
        }

		#region IService implementation

		public Guid Guid { get; private set; }
		public bool IsPrimary { get; private set; }
        public List<IBLECharacteristic> Characteristics { get; private set; }

        #endregion

        public override string ToString()
        {
            return "BLE Service - " + Guid.ToString();
        }
    }
}


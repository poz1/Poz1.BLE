using Android.Bluetooth;
using Poz1.BLE.Core;
using System;

namespace Poz1.BLE.Droid
{
    public class BLECharacteristic : IBLECharacteristic
	{
		private readonly BluetoothGattCharacteristic _nativeCharacteristic;

        public BLECharacteristic(BluetoothGattCharacteristic characteristic)
		{
			_nativeCharacteristic = characteristic;

            Value = _nativeCharacteristic.GetValue();
            Guid = GuidHelper.FromUUID(_nativeCharacteristic.Uuid);
            ServiceGuid = GuidHelper.FromUUID(_nativeCharacteristic.Service.Uuid);
            Properties = (CharacteristicPropertyType)(int)_nativeCharacteristic.Properties;
        }

        #region ICharacteristic implementation

		public Guid Guid { get; private set; }
        public byte[] Value { get; private set; }
		public CharacteristicPropertyType Properties { get; private set; }
        public Guid ServiceGuid { get; private set; }
        #endregion

        public override string ToString()
        {
            return "BLE Char. - " + Guid.ToString();
        }
    }
}


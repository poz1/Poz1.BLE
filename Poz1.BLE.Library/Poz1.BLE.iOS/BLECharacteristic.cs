using CoreBluetooth;
using Poz1.BLE.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poz1.BLE.iOS
{
    class BLECharacteristic : IBLECharacteristic
    {
        private readonly CBCharacteristic _nativeCharacteristic;
        public BLECharacteristic(CBCharacteristic characteristic)
        {
            _nativeCharacteristic = characteristic;

            Value = _nativeCharacteristic?.Value.ToArray();
            Guid = Guid.Parse(_nativeCharacteristic.UUID.ToString());
            //Properties = (CharacteristicPropertyType)(int)_nativeCharacteristic.Properties;
        }

        #region ICharacteristic implementation

        public Guid Guid { get; private set; }
        public byte[] Value { get; private set; }
        public CharacteristicPropertyType Properties { get; private set; }

        public Guid ServiceGuid
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}

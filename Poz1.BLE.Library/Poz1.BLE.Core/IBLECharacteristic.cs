using System;

namespace Poz1.BLE.Core
{
    public interface IBLECharacteristic
	{
		Guid Guid { get; }
        Guid ServiceGuid { get; }
        byte[] Value { get; }
        CharacteristicPropertyType Properties { get; }
	}

    public class CharacteristicEventArgs : EventArgs
    {
        public IBLECharacteristic Characteristic { get; private set; }

        public CharacteristicEventArgs(IBLECharacteristic characteristic)
        {
            Characteristic = characteristic;
        }
    }

    public class NotificationEventArgs : EventArgs
    {
        public string Notification { get; private set; }

        public NotificationEventArgs(string notification)
        {
            Notification = notification;
        }
    }
}


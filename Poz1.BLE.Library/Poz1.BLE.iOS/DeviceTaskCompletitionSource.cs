using Poz1.BLE.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poz1.BLE.iOS
{
    class DeviceTaskCompletitionSource
    {
        public TaskCompletionSource<IBLECharacteristic> WriteCharacteristicTCS;
        public TaskCompletionSource<IBLECharacteristic> ReadCharacteristicTCS;
        public TaskCompletionSource<List<IBLEService>> ServicesDiscoveryTCS;
        public TaskCompletionSource<List<IBLECharacteristic>> CharacteristicDiscoveryTCS;

        public TaskCompletionSource<IList<IBLEDevice>> ScanForDevicesTCS;
        public TaskCompletionSource<object> ConnectTCS;
        public TaskCompletionSource<int> RssiTCS;

        public TaskCompletionSource<bool> SubscribeCharacteristicTCS;
        public TaskCompletionSource<bool> UnsubscribeCharacteristicTCS;
    }
}
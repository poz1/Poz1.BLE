using CoreBluetooth;
using Poz1.BLE.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poz1.BLE.iOS
{
    class BLEService : IBLEService
    {
        private readonly CBService _nativeService;

        public BLEService(CBService nativeService)
        {
            _nativeService = nativeService;
            Characteristics = new List<IBLECharacteristic>();

            Guid = Guid.Parse(_nativeService.UUID.ToString());
            IsPrimary = _nativeService.Primary;

            foreach (var item in _nativeService.Characteristics)
            {
                var characteristic = new BLECharacteristic(item);
                Characteristics.Add(characteristic);
            }
        }

        internal Task<List<IBLECharacteristic>> GetCharacteristicsAsync(CBPeripheral peripheral, TaskCompletionSource<List<IBLECharacteristic>> characteristicDiscoveryTCS)
        {
            characteristicDiscoveryTCS = new TaskCompletionSource<List<IBLECharacteristic>>();
            peripheral.DiscoverCharacteristics(_nativeService);
            return characteristicDiscoveryTCS.Task;      
        }

        #region IService implementation

        public Guid Guid { get; private set; }
        public bool IsPrimary { get; private set; }
        public List<IBLECharacteristic> Characteristics { get; internal set; }

        #endregion
    }
}

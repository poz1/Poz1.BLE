using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poz1.BLE.Core
{
    public interface IBLEAdapter
	{
		bool IsScanning { get; }
        TimeSpan ScanTimeout { get; }
        Task<IList<IBLEDevice>> ScanForDevicesAsync();
	}
}


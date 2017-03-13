using System;
using System.Collections.Generic;

namespace Poz1.BLE.Core
{
    public interface IBLEService
	{
		Guid Guid { get; }
        bool IsPrimary { get; }
		List<IBLECharacteristic> Characteristics { get; }
	}
}


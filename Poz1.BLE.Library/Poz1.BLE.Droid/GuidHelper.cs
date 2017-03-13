using System;

namespace Poz1.BLE.Droid
{
    public static class GuidHelper
    {
        public static Guid FromUUID(Java.Util.UUID uuid)
        {
            byte[] uuidMostSignificantBytes = BitConverter.GetBytes(uuid.MostSignificantBits);
            byte[] uuidLeastSignificantBytes = BitConverter.GetBytes(uuid.LeastSignificantBits);

            byte[] guidBytes = new byte[16] 
            {
                uuidMostSignificantBytes[4],
                uuidMostSignificantBytes[5],
                uuidMostSignificantBytes[6],
                uuidMostSignificantBytes[7],
                uuidMostSignificantBytes[2],
                uuidMostSignificantBytes[3],
                uuidMostSignificantBytes[0],
                uuidMostSignificantBytes[1],
                uuidLeastSignificantBytes[7],
                uuidLeastSignificantBytes[6],
                uuidLeastSignificantBytes[5],
                uuidLeastSignificantBytes[4],
                uuidLeastSignificantBytes[3],
                uuidLeastSignificantBytes[2],
                uuidLeastSignificantBytes[1],
                uuidLeastSignificantBytes[0]
            };

            return new Guid(guidBytes);
        }
    }
}
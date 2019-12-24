using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace IPGeo.Services
{
    public static class IPHelper
    {
        public static bool IsCorrectIP(string ipString)
        {
            return IPAddress.TryParse(ipString, out IPAddress address);
        }

        public static UInt32 ConvertToIPNumber(string ipString)
        {
            var address = IPAddress.Parse(ipString);

            var bytes = address.GetAddressBytes();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }
    }
}

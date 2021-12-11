using System;
using System.Collections;

namespace LZ.Compressions.Core.Converters
{
    internal static class ValuesConverters
    {
        public static byte ToByte(this BitArray bits)
        {
            if (bits == null)
                throw new ArgumentNullException(nameof(bits));

            if (bits.Count > 8)
                throw new ArgumentException("More bits than 8");

            byte result = 0;
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    result |= (byte)(1 << i);
                }
            }

            return result;
        }
    }
}

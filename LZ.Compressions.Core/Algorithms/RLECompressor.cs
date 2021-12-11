using LZ.Compressions.Core.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LZ.Compressions.Core.Algorithms
{
    public class RLECompressor
    {
        public IReadOnlyCollection<byte> Compress(string uncompressed)
        {
            var uncompressedBytes = Encoding.UTF8.GetBytes(uncompressed);
            var unrepeats = 0;
            var unrepeatedBytes = new List<byte>();

            var repeats = 1;
            var compressedBytes = new List<byte>();

            for (int i = 0; i < uncompressedBytes.Length; i++)
            {
                if (i + 1 < uncompressedBytes.Length 
                    && uncompressedBytes[i] == uncompressedBytes[i + 1] 
                    && repeats <= 129)
                {
                    if (unrepeats > 0)
                    {
                        compressedBytes.Add((byte)(unrepeats - 1));
                        compressedBytes.AddRange(unrepeatedBytes);
                        unrepeatedBytes.Clear();
                        unrepeats = 0;
                    }

                    repeats++;
                }
                else
                {
                    if (repeats > 1)
                    {
                        var repeatsByte = (byte)(Convert.ToByte(repeats) - 2);
                        var repeatBits = new BitArray(new[] { repeatsByte });
                        repeatBits.Set(7, true);
                        compressedBytes.Add(repeatBits.ToByte());

                        var charBits = new BitArray(new[] { uncompressedBytes[i - 1] });
                        compressedBytes.Add(charBits.ToByte());

                        repeats = 1;
                    }
                    else
                    {
                        unrepeats++;
                        unrepeatedBytes.Add(uncompressedBytes[i]);
                    }
                }
            }

            return compressedBytes;
        }

        public string Decompress(byte[] bytes)
        {
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] > 127)
                {
                    var repeats = bytes[i] - 127 + 1;
                    stringBuilder.Append(new string((char)bytes[i + 1], repeats));
                    i++;
                }
                else
                {
                    var repeats = bytes[i] + 1;
                    var sub = bytes.Skip(i + 1).Take(repeats);
                    stringBuilder.Append(string.Join(string.Empty, sub.Select(x => (char)x)));
                    i += repeats;
                }
            }

            return stringBuilder.ToString();
        }
    }
}

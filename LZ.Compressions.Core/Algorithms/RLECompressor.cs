using LZ.Compressions.Core.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LZ.Compressions.Core.Algorithms
{
    public class RLECompressor : ITextCompressor, IReadableCompressor
    {
        private const string Delimiter = " ";

        public string Compress(string uncompressed)
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
                    && repeats < 129)
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

            if (unrepeatedBytes.Count != 0)
                compressedBytes.AddRange(unrepeatedBytes);

            return string.Join(Delimiter, compressedBytes);
        }

        public string Decompress(string compressed)
        {
            var bytes = compressed.Split(Delimiter).Select(x => Convert.ToByte(x)).ToArray();
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

        public string GetReadableView(string compressed)
        {
            var bytes = compressed.Split(Delimiter).Select(x => Convert.ToByte(x)).ToArray();

            if ((bytes.Length * 2) % 2 != 0)
                throw new Exception();

            var items = new List<string>();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] >= 128)
                {
                    var repeats = bytes[i] - 128 + 2;
                    var ch = Convert.ToChar(bytes[i + 1]);
                    items.Add($"{repeats}{ch}");
                    i++;
                }
                else
                {
                    var ch = Convert.ToChar(bytes[i]);
                    items.Add($"{ch}");
                }
            }

            return string.Join(Delimiter, items);
        }
    }
}

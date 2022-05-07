using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LZ.Compressions.Core.Algorithms
{
    public class LZ78Compressor : ITextCompressor
    {
        private const string Delimiter = " ";

        public string Compress(string uncompressed)
        {
            var items = new List<(int, int, char)>();
            var left = new StringBuilder();
            var right = new StringBuilder(uncompressed);

            items.Add((0, 0, uncompressed[0]));
            left.Append(uncompressed[0]);
            right.Remove(0, 1);

            for (int i = 1; i < uncompressed.Length; i++)
            {
                var str = string.Empty;
                var index = -1;
                var count = 0;

                for (int end = 0; end < right.Length; end++)
                {
                    str = right.ToString()[..(end + 1)];
                    var tempIndex = left.ToString().LastIndexOf(str);

                    if (tempIndex == -1)
                        break;

                    index = tempIndex;
                    count = str.Length;
                }

                if (index == -1)
                {
                    items.Add((0, 0, right[0]));
                    left.Append(right[0]);
                    right.Remove(0, 1);
                }
                else
                {
                    if (str == right.ToString())
                    {
                        items.Add((left.Length - index, count, ' '));
                    }
                    else
                    {
                        items.Add((left.Length - index, count, str[^1]));
                    }

                    left.Append(str);
                    right.Remove(0, Math.Min(right.Length, count + 1));
                    i += count;
                }
            }

            return string.Join(Delimiter, items.Select(x => $"{x.Item1},{x.Item2},{x.Item3}"));
        }

        public string Decompress(string compressed)
        {
            throw new NotImplementedException();
        }

        public void ValidateBeforeCompress(string input)
        {
            //throw new NotImplementedException();
        }

        public void ValidateBeforeDecompress(string input)
        {
            //throw new NotImplementedException();
        }
    }
}

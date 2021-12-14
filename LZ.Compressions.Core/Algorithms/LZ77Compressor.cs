using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LZ.Compressions.Core.Algorithms
{
    public class LZ77Compressor : ITextCompressor, IReadableCompressor
    {
        private const string Delimiter = " ";

        public string Compress(string uncompressed)
        {
            var items = new List<(int, int, char)>();
            var left = new StringBuilder();
            var right = new StringBuilder(uncompressed);

            for (int i = 0; i < uncompressed.Length; i++)
            {
                if (FindMaxPrefix(left.ToString(), right.ToString(), out (int, string) follow))
                {
                    left.Append(follow.Item2);
                    right.Remove(0, follow.Item2.Length);
                    items.Add((follow.Item1, follow.Item2.Length, right[0]));
                    left.Append(right[0]);
                    i += follow.Item2.Length;
                }
                else
                {
                    left.Append(uncompressed[i]);
                    items.Add((0, 0, uncompressed[i]));
                }
                right.Remove(0, 1);
            }

            return string.Join(Delimiter, items.Select(x => $"{x.Item1} {x.Item2} {(byte)x.Item3}"));
        }

        public string Decompress(string compressed)
        {
            throw new NotImplementedException();
        }

        public string GetReadableView(string compressed)
        {
            var str = new StringBuilder();
            var tuples = compressed.Split(Delimiter).ToArray();
            for (int i = 0; i < tuples.Length; i++)
            {
                var offset = int.Parse(tuples[i]);
                var length = int.Parse(tuples[i + 1]);
                var ch = Convert.ToChar(Convert.ToByte(tuples[i + 2]));
                str.AppendLine($"({offset},{length},{ch})");
                i += 2;
            }

            return str.ToString();
        }

        private bool FindMaxPrefix(string s1, string s2, out (int, string) follow)
        {
            follow = (-1, string.Empty);

            for (int i = 0; i < s2.Length; i++)
            {
                var substr = s2[0..(i + 1)];
                var index = s1.LastIndexOf(substr);

                if (index == -1)
                    break;

                follow = (s1.Length - index, substr);
            }

            if (follow.Item1 != -1)
                return true;

            return false;
        }
    }
}

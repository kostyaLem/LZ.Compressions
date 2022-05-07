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

            for (int i = 0; i < uncompressed.Length; i++)
            {
                if (FindMaxPrefix(left.ToString(), right.ToString(), out (int Index, string Str) follow))
                {
                    if (right.ToString() == follow.Str)
                    {
                        items.Add((left.Length - follow.Index, follow.Str.Length, ' '));
                        i += follow.Str.Length;
                    }
                    else
                    {
                        var newCh = right[follow.Str.Length];
                        items.Add((left.Length - follow.Index, follow.Str.Length, newCh));
                        left.Append(follow.Str + newCh);
                        right.Remove(0, follow.Str.Length + 1);
                        i += follow.Str.Length;
                    }
                }
                else
                {
                    items.Add((0, 0, right[0]));
                    left.Append(right[0]);
                    right.Remove(0, 1);
                }
            }

            return string.Join(Delimiter, items.Select(x => $"{x.Item1},{x.Item2},{x.Item3}"));
        }

        public string Decompress(string compressed)
        {
            throw new NotImplementedException();
        }

        private bool FindMaxPrefix(string left, string right, out (int Index, string Str) follow)
        {
            follow = (-1, string.Empty);

            for (int end = 0; end < right.Length; end++)
            {
                var tempStr = right.ToString()[..(end + 1)];
                var tempIndex = left.ToString().LastIndexOf(tempStr);

                if (tempIndex == -1)
                    break;

                follow.Str = tempStr;
                follow.Index = tempIndex;
            }

            return follow.Index != -1;
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

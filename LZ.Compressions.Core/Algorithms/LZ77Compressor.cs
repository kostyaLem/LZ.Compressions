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
                if (FindMaxPrefix(left.ToString(), right.ToString(), out var follow))
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

        public bool Validate(string input)
        {
            throw new NotImplementedException();
        }

        private bool FindMaxPrefix(string s1, string s2, out (int, string) follow)
        {
            follow = (-1, string.Empty);

            for (int index = s1.Length; index > 0; index--)
            {
                for (int subLength = 1; subLength <= index; subLength++)
                {
                    var substr = new string(s1.Reverse().Skip(s1.Length - index).Take(subLength).Reverse().ToArray()); //  s1[(s1.Length - subLength)..index];

                    for (int repeats = 1; repeats <= Math.Ceiling(s2.Length / (double)substr.Length); repeats++)
                    {
                        var repeatStr = new string(Enumerable.Repeat(substr, repeats).SelectMany(x => x).ToArray());

                        if (s2.StartsWith(repeatStr))
                        {
                            follow = (s1.Length - index + 1, repeatStr);

                            if (repeatStr == s2)
                            {
                                follow = (s1.Length - index + 1, repeatStr[..^1]);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (follow.Item1 == -1)
                        break;
                }
            }

            if (follow.Item1 != -1)
                return true;

            return false;
        }

        private bool PreInit(string uncompressed)
        {
            if (uncompressed.Length == 2 && uncompressed.Distinct().Count() == 1)
            {
                return true;
            }

            return false;
        }
    }
}

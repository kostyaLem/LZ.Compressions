using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LZ.Compressions.Core.Algorithms
{
    public class LZ78Compressor : ITextCompressor
    {
        private const string Delimiter = " ";
        private const string Num1GroupName = "d1";
        private const string Num2GroupName = "d2";
        private const string LetterGroupName = "l";
        private readonly string PairPattern = $@"(?'{Num1GroupName}'\d+)?,(?'{Num2GroupName}'\d+)?,(?'{LetterGroupName}'.)?\s?";

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
                        items.Add((follow.Index, follow.Str.Length, ' '));
                    }
                    else
                    {
                        var newCh = right[follow.Str.Length];
                        items.Add((follow.Index, follow.Str.Length, newCh));
                        left.Append(follow.Str + newCh);
                        right.Remove(0, follow.Str.Length + 1);
                    }
                    i += follow.Str.Length;
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
            var strBuilder = new StringBuilder();

            var matches = Regex.Matches(compressed, PairPattern);
            foreach (Match match in matches)
            {
                // ValidateMatch(match);

                var num1 = int.Parse(match.Groups[Num1GroupName].Value);
                var num2 = int.Parse(match.Groups[Num2GroupName].Value);
                var letter = match.Groups[LetterGroupName].Value;

                if (num1 == 0 && num2 == 0)
                {
                    strBuilder.Append(letter);
                }
                else
                {
                    strBuilder.Append(strBuilder.ToString().Substring(num1, num2));
                    strBuilder.Append(letter);
                }
            }

            return strBuilder.ToString();
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

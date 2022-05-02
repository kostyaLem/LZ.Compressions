using LZ.Compressions.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LZ.Compressions.Core.Algorithms
{
    public class LZ77Compressor : ITextCompressor
    {
        private const string Delimiter = " ";
        private const string Num1GroupName = "d1";
        private const string Num2GroupName = "d2";
        private const string LetterGroupName = "l";
        private readonly string PairPattern = $@"(?'{Num1GroupName}'\d)? (?'{Num2GroupName}'\d)? (?'{LetterGroupName}'.)?\s?";

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

            return string.Join(Delimiter, items.Select(x => $"{x.Item1} {x.Item2} {x.Item3}"));
        }

        public string Decompress(string compressed)
        {
            var strBuilder = new StringBuilder();

            var matches = Regex.Matches(compressed, PairPattern);
            foreach (Match match in matches)
            {
                ValidateMatch(match);

                var num1 = int.Parse(match.Groups[Num1GroupName].Value);
                var num2 = int.Parse(match.Groups[Num2GroupName].Value);
                var letter = match.Groups[LetterGroupName].Value;

                if (num1 == 0 && num2 == 0)
                {
                    strBuilder.Append(letter);
                }
                else if (num1 + num2 > strBuilder.Length)
                {
                    strBuilder.Append(strBuilder[strBuilder.Length - num1], num2);
                    strBuilder.Append(letter);
                }
                else
                {
                    var lastStr = strBuilder.ToString()[(strBuilder.Length - num1)..num2];
                    strBuilder.Append(lastStr);
                }
            }

            return strBuilder.ToString();
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

        private static void ValidateMatch(Match match)
        {
            if (!match.Groups[Num1GroupName].Success
                || !match.Groups[Num2GroupName].Success
                || !match.Groups[LetterGroupName].Success)
            {
                throw new InputStringValidateException($"Ошибка при разборе строки: {match.Value}");
            }

        }

        public bool ValidateBeforeCompress(string input) => true;
        public bool ValidateBeforeDecompress(string input) => true;
    }
}

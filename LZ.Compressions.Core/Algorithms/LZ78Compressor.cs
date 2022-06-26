using LZ.Compressions.Core.Exceptions;
using LZ.Compressions.Core.Models;
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
        private const string LettersGroupName = "l";
        private readonly string PairPattern = $@"(?'{Num1GroupName}'\d+)?,(?'{Num2GroupName}'\d+)?,(?'{LettersGroupName}'.)?\s?";

        public CompressResult Compress(string uncompressed)
        {
            var items = new List<(int, int, char)>();
            var left = new StringBuilder();
            var right = new StringBuilder(uncompressed);

            for (int i = 0; i < uncompressed.Length; i++)
            {
                // Найти повторяющиеся символы из правой части в левой части строки
                if (FindMaxPrefix(left.ToString(), right.ToString(), out (int Index, string Str) follow))
                {
                    if (right.ToString() == follow.Str)
                    {
                        // Добавить повторение с пустым символом в конце, если строка закончена
                        items.Add((follow.Index, follow.Str.Length, ' '));
                    }
                    else
                    {
                        // Выделить символ для добавления в конец
                        var newCh = right[follow.Str.Length];
                        // Добавить повторение {индекс, длина сдвига, новый символ}
                        items.Add((follow.Index, follow.Str.Length, newCh));
                        left.Append(follow.Str + newCh);
                        right.Remove(0, follow.Str.Length + 1);
                    }
                    i += follow.Str.Length;
                }
                else
                {
                    // Добавить 1 символ, если повторение не было найдено
                    items.Add((0, 0, right[0]));
                    left.Append(right[0]);
                    right.Remove(0, 1);
                }
            }

            // Соединям повторения в группы и разделяем пробелами
            var compressedStr = string.Join(Delimiter, items.Select(x => $"{x.Item1},{x.Item2},{x.Item3}"));
            var compressedLength = items.Count * 3;

            return new CompressResult(compressedStr, compressedLength);
        }

        public string Decompress(string compressed, IDictionary<int, string> initialDictionary = null)
        {
            var strBuilder = new StringBuilder();

            var matches = Regex.Matches(compressed, PairPattern);
            foreach (Match match in matches)
            {
                // ValidateMatch(match);

                var num1 = int.Parse(match.Groups[Num1GroupName].Value);
                var num2 = int.Parse(match.Groups[Num2GroupName].Value);
                var letter = match.Groups[LettersGroupName].Value;

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

        private static bool FindMaxPrefix(string left, string right, out (int Index, string Str) follow)
        {
            follow = (-1, string.Empty);

            for (int end = 0; end < right.Length; end++)
            {
                // Выделить подстроку из правой части от 0 до end
                var tempStr = right.ToString()[..(end + 1)];
                // Найти выделенную подстроку в левой части
                var tempIndex = left.ToString().LastIndexOf(tempStr);

                if (tempIndex == -1)
                    break;

                // Вернуть индекс и найденную строку
                follow.Str = tempStr;
                follow.Index = tempIndex;
            }

            return follow.Index != -1;
        }

        public void ValidateBeforeCompress(string input) { }

        public void ValidateBeforeDecompress(string input)
        {
            var matches = Regex.Matches(input, PairPattern);

            foreach (Match match in matches)
            {
                var groups = match.Groups;

                if (!groups[LettersGroupName].Success
                    || !groups[Num1GroupName].Success
                    || !groups[Num2GroupName].Success)
                {
                    throw new InputStringValidateException($"Ошибка чтения закодированной строки: {match.Value}");
                }
            }
        }
    }
}

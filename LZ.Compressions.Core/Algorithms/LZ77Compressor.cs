using LZ.Compressions.Core.Exceptions;
using LZ.Compressions.Core.Models;
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
        private const string LettersGroupName = "l";
        private readonly string PairPattern = $@"(?'{Num1GroupName}'\d+)? (?'{Num2GroupName}'\d+)? (?'{LettersGroupName}'.)?\s?";

        public CompressResult Compress(string uncompressed)
        {
            var items = new List<(int, int, char)>();
            var left = new StringBuilder();
            var right = new StringBuilder(uncompressed);

            for (int i = 0; i < uncompressed.Length; i++)
            {
                // Находим повторяющиеся символы из правой части строки в левой части
                if (FindMaxPrefix(left.ToString(), right.ToString(), out var follow))
                {
                    left.Append(follow.Item2);
                    right.Remove(0, follow.Item2.Length);
                    // Добавить повторение {индекс, длина повторение, следующий символ}
                    items.Add((follow.Item1, follow.Item2.Length, right[0]));
                    left.Append(right[0]);
                    i += follow.Item2.Length;
                }
                else
                {
                    // Добавляем 1 символ из правой строки, если не нашли повторение
                    left.Append(uncompressed[i]);
                    items.Add((0, 0, uncompressed[i]));
                }
                right.Remove(0, 1);
            }

            // Соединям повторения в группы и разделяем пробелами
            var compressedStr = string.Join(Delimiter, items.Select(x => $"{x.Item1} {x.Item2} {x.Item3}"));
            var compressedLength = items.Count * 3;

            return new CompressResult(compressedStr, compressedLength);
        }

        public string Decompress(string compressed)
        {
            var strBuilder = new StringBuilder();

            // Разобрать сжатую строку в группы {число, число, символ}
            var matches = Regex.Matches(compressed, PairPattern);
            foreach (Match match in matches)
            {
                // Проверка сжатой строки на валидный формат
                ValidateMatch(match);

                var num1 = int.Parse(match.Groups[Num1GroupName].Value);
                var num2 = int.Parse(match.Groups[Num2GroupName].Value);
                var letter = match.Groups[LettersGroupName].Value;

                if (num1 == 0 && num2 == 0)
                {
                    // Добавить 1 символ, если нет повторений
                    strBuilder.Append(letter);
                }
                else if (num1 + num2 > strBuilder.Length)
                {
                    // Добавить n символов, превыщающие левое окно
                    strBuilder.Append(strBuilder[strBuilder.Length - num1], num2);
                    strBuilder.Append(letter);
                }
                else
                {
                    // Добавить n символов
                    var lastStr = strBuilder.ToString()[(strBuilder.Length - num1)..num2];
                    strBuilder.Append(lastStr);
                }
            }

            return strBuilder.ToString();
        }

        private static bool FindMaxPrefix(string s1, string s2, out (int, string) follow)
        {
            follow = (-1, string.Empty);

            // Проходимся по левой части строки
            for (int index = s1.Length; index > 0; index--)
            {
                // Перебираем строки разной длины в левой части строки
                for (int subLength = 1; subLength <= index; subLength++)
                {
                    var substr = new string(s1.Reverse().Skip(s1.Length - index).Take(subLength).Reverse().ToArray());

                    // Находим повторение s2 в s1
                    for (int repeats = 1; repeats <= Math.Ceiling(s2.Length / (double)substr.Length); repeats++)
                    {
                        // Создаём повторение подстроки для поиска в s2
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

                    // Если не нашли повторение
                    if (follow.Item1 == -1)
                        break;
                }
            }

            // Если не нашли повторение
            if (follow.Item1 != -1)
                return true;

            return false;
        }

        private static void ValidateMatch(Match match)
        {
            if (!match.Groups[Num1GroupName].Success
                || !match.Groups[Num2GroupName].Success
                || !match.Groups[LettersGroupName].Success)
            {
                throw new InputStringValidateException($"Ошибка при разборе строки: {match.Value}");
            }
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

using LZ.Compressions.Core.Exceptions;
using LZ.Compressions.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LZ.Compressions.Core.Algorithms
{
    public class LZWCompressor : ITextCompressor
    {
        private const char Delimiter = ' ';

        public CompressResult Compress(string uncompressed)
        {
            // Определяем словарь символов
            var dictionary = new Dictionary<string, int>();
            // Предзаполняем словарь дефолтными символами
            var dist = uncompressed.Distinct().ToList();
            for (int i = 0; i < dist.Count; i++)
                dictionary.Add((dist[i]).ToString(), i);

            var w = string.Empty;
            var compressed = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    // Записываем подстроку в результат
                    compressed.Add(dictionary[w]);
                    // Добавляем wv в словарь
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // Записываем оставшуюся часть в результат
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            // Разделяем сжатую строку пробелами
            var compressedStr = string.Join(Delimiter, compressed);
            var compressedLength = compressed.Count;

            return new CompressResult(compressedStr, compressedLength, dictionary.Keys.ToList());
        }

        public string Decompress(string compressed, IDictionary<int, string> initialDictionary)
        {
            var parsedData = compressed.Split(Delimiter)
                .Select(int.Parse)
                .ToArray();

            // Определяем словарь символов            
            var dictionary = initialDictionary;

            var w = dictionary[parsedData[0]];
            var decompressed = new StringBuilder(w);

            // Разбираем сжатую строку, доставая из словаря нужные символы
            foreach (int k in parsedData.Skip(1))
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                // Добавить новое повторение в словарь для использования
                // в последующих циклах распаковки
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }

        public void ValidateBeforeCompress(string input)
        {
            if (input.Any(x => x > 256))
            {
                throw new InputStringValidateException("Входная строка содержит недопустимый символ. Допустимые сиволы от 0 до 255.");
            }
        }

        public void ValidateBeforeDecompress(string input)
        {
            if (!input.Where(x => x != ' ').All(char.IsDigit))
            {
                throw new InputStringValidateException("Входная строка содержит не число.");
            }

            if (!input.Split(' ').All(x => int.TryParse(x, out var res)))
            {
                throw new InputStringValidateException("Не удалось преобразовать символ в число.");
            }
        }
    }
}

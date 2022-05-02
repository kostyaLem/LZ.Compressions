using LZ.Compressions.Core.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LZ.Compressions.Core.Algorithms
{
    public class LZWCompressor : ITextCompressor
    {
        private const char Delimiter = ' ';

        public string Compress(string uncompressed)
        {
            var compressedData = CompressData(uncompressed);

            return string.Join(Delimiter, compressedData);
        }

        public string Decompress(string compressed)
        {
            var parsedData = compressed.Split(Delimiter)
                .Select(int.Parse)
                .ToArray();

            return DecompressData(parsedData);
        }

        public static IReadOnlyList<int> CompressData(string uncompressed)
        {
            // build the dictionary
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            string w = string.Empty;
            List<int> compressed = new List<int>();

            foreach (char c in uncompressed)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    // write w to output
                    compressed.Add(dictionary[w]);
                    // wc is a new sequence; add it to the dictionary
                    dictionary.Add(wc, dictionary.Count);
                    w = c.ToString();
                }
            }

            // write remaining output if necessary
            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return compressed;
        }

        public static string DecompressData(IReadOnlyList<int> compressed)
        {
            // build the dictionary
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            string w = dictionary[compressed[0]];
            StringBuilder decompressed = new StringBuilder(w);

            foreach (int k in compressed.Skip(1))
            {
                string entry = null;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressed.Append(entry);

                // new sequence; add it to the dictionary
                dictionary.Add(dictionary.Count, w + entry[0]);

                w = entry;
            }

            return decompressed.ToString();
        }

        public bool Validate(string input)
        {
            if (input.Any(x => x < 0 || x > 256))
                throw new InputStringValidateException("Входная строка имеет недопустимые символы");

            return true;
        }
    }
}

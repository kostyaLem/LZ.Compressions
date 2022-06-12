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

            var compressedStr = string.Join(Delimiter, compressed);
            var compressedLength = compressed.Count;

            return new CompressResult(compressedStr, compressedLength);
        }

        public string Decompress(string compressed)
        {
            var parsedData = compressed.Split(Delimiter)
                .Select(int.Parse)
                .ToArray();

            // build the dictionary
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            string w = dictionary[parsedData[0]];
            StringBuilder decompressed = new StringBuilder(w);

            foreach (int k in parsedData.Skip(1))
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

        public void ValidateBeforeCompress(string input) { }
        public void ValidateBeforeDecompress(string input)
        {
            if (!input.Where(x => x != ' ').All(char.IsDigit))
            {
                throw new InputStringValidateException();
            }

            if (!input.Split(' ').All(x => int.TryParse(x, out var res)))
            {
                throw new InputStringValidateException();
            }
        }
    }
}

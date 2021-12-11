using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LZ.Compressions.Core.Algorithms
{
    public class LZWCompressor : ITextCompressor
    {
        private const char Delimiter = ',';

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

        private IReadOnlyList<int> CompressData(string uncompressed)
        {
            var dictionary = new Dictionary<string, int>();
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

            if (!string.IsNullOrEmpty(w))
                compressed.Add(dictionary[w]);

            return compressed;
        }

        private string DecompressData(IReadOnlyList<int> compressed)
        {
            var dictionary = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            var w = dictionary[compressed[0]];
            var decompressed = new StringBuilder(w);

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
    }
}

using LZ.Compressions.Core.Exceptions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LZ.Compressions.Core.Algorithms
{
    public class RLECompressor : ITextCompressor
    {
        private const string DigitsGroupName = "digits";
        private const string LettersGroupName = "letters";
        private readonly string PairPattern = @$"(?'{DigitsGroupName}'[a-zA-Z]+)?(?'{LettersGroupName}'\d+)?";

        public string Compress(string uncompressed)
        {
            var strBuilder = new StringBuilder();

            for (int i = 0; i < uncompressed.Length;)
            {
                var repeats = 1;

                for (int j = i + 1; j < uncompressed.Length; j++)
                {
                    if (uncompressed[j] == uncompressed[i])
                        repeats++;

                    if (repeats == 1)
                        break;
                }

                strBuilder.Append(repeats + uncompressed[i].ToString());
                i += repeats;
            }

            return strBuilder.ToString();
        }

        public string Decompress(string compressed)
        {
            var strBuilder = new StringBuilder();
            var matches = Regex.Matches(compressed, PairPattern);
            foreach (Match match in matches)
            {
                var (ch, repeats) = GetPair(match);
                strBuilder.Append(ch, repeats);
            }

            return strBuilder.ToString();
        }

        public void ValidateBeforeCompress(string input)
        {
            if (input.Any(char.IsLetter))
                throw new InputStringValidateException("Входная строка содержит не буквы");
        }

        public void ValidateBeforeDecompress(string input)
        {
            var matches = Regex.Matches(input, PairPattern);

            foreach (Match match in matches)
            {
                var groups = match.Groups;

                if (!groups[LettersGroupName].Success || !groups[DigitsGroupName].Success)
                {
                    throw new InputStringValidateException($"Ошибка чтения закодированной строки: {match.Value}");
                }
            }
        }

        private static (char ch, int repeats) GetPair(Match match)
        {
            var letters = match.Groups[1].Value;
            if (letters.Length > 1)
                throw new InputStringValidateException($"Пара имеет более одного символа для повторения: {match.Value}");

            return (letters[0], int.Parse(match.Value[^1..]));
        }
    }
}

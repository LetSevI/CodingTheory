using System;
using System.Collections.Generic;

namespace CodingTheory
{
    public class LZ77 : IAlgorithm
    {
        private const int BufferSize = 6;

        private double _compressionRatio = -1;
        private List<DictionaryItem> _dictionary; 

        public string GetName()
        {
            return "Словарный метод сжатия LZ77";
        }

        public string Encode(string input)
        {
            string encodedString = string.Empty;
            _dictionary = new List<DictionaryItem>();
            int offset = 0;
            string buffer = (input.Length <= BufferSize) ? input.Substring(1, input.Length - 1) : input.Substring(1, BufferSize);
            _dictionary.Add(new DictionaryItem(0, 0, input[0]));
            while (buffer.Length != 0)
            {
                string substring = input.Substring(0, offset + 1);
                for (int i = buffer.Length; i >= 0; i--)
                {
                    int ind = substring.IndexOf(buffer.Substring(0, i), StringComparison.Ordinal);
                    if (i != 0 && ind == -1) continue;
                    offset += (i == 0) ? 1 : i + 1;
                    _dictionary.Add(new DictionaryItem((i > 0) ? ind : 0, i, (offset) >= input.Length ? 'ソ' : input[offset]));
                    if (offset + 1 < input.Length)
                    {
                        string leftover = input.Substring(offset + 1, input.Length - offset - 1);
                        buffer = (leftover.Length < BufferSize) ? leftover : leftover.Substring(0, BufferSize);
                    }
                    else
                    {
                        buffer = "";
                    }
                    break;
                }
            }

            foreach (DictionaryItem item in _dictionary)
            {
                encodedString += string.Format("{0}{1}{2}", item.Offset, item.Length, (item.Symbol != 'ソ') ? item.Symbol.ToString() : "EOL");
            }

            _compressionRatio = (double)encodedString.Replace("EOL", "").Length / input.Length;

            encodedString += "\n";

            foreach (DictionaryItem item in _dictionary)
            {
                encodedString += string.Format("({0},{1},{2})", item.Offset, item.Length, (item.Symbol != 'ソ') ? item.Symbol.ToString() : "EOL");
            }

            return encodedString;
        }

        public string Decode(string input)
        {
            string decodedString = string.Empty;
            foreach (DictionaryItem item in _dictionary)
            {
                if (item.Length == 0)
                {
                    decodedString += (item.Symbol != 'ソ') ? item.Symbol.ToString() : "";
                }
                else
                {
                    decodedString += decodedString.Substring(item.Offset, item.Length) + ((item.Symbol != 'ソ') ? item.Symbol.ToString() : "");
                }
            }
            return decodedString;
        }

        public double GetCompressionRatio()
        {
            return _compressionRatio;
        }
    }

    public struct DictionaryItem
    {
        public int Offset { get; }
        public int Length { get; }
        public char Symbol { get; }

        public DictionaryItem(int offset, int length, char symbol) : this()
        {
            Offset = offset;
            Length = length;
            Symbol = symbol;
        }
    }
}
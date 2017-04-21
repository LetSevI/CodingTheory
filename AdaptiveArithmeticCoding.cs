using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodingTheory
{
    public class AdaptiveArithmeticCoding : IAlgorithm
    {
        private Dictionary<char, decimal> _weights = new Dictionary<char, decimal>();
        private double _compressionRatio = -1;

        public string GetName()
        {
            return "Адаптивное арифметическое кодирование";
        }

        public string Encode(string input)
        {
            _weights = new Dictionary<char, decimal>();
            _compressionRatio = -1;
            input += 'ソ';

            foreach (char ch in input)
            {
                if (_weights.ContainsKey(ch)) continue;
                decimal frequency = 1;
                if (_weights.Count > 0)
                {
                    frequency += _weights.Max(x => x.Value);
                }
                _weights.Add(ch, frequency);
            }

            decimal low = 0.0m;
            decimal high = 1.0m;
            decimal sumOfWeights = _weights.Count;

            for (int i = 0; i < input.Length; i++)
            {
                decimal range = high - low;
                high = low + range * _weights[input[i]]/sumOfWeights;
                if (_weights[input[i]] != _weights.Min(x => x.Value))
                {
                    low = low + range * _weights
                        .Where(x => x.Value < _weights[input[i]])
                        .Max(x => x.Value)/ sumOfWeights;
                }
                _weights[input[i]]++;
                sumOfWeights++;
            }

            string encodedString = low.ToString(CultureInfo.CurrentCulture);
            encodedString = encodedString.Substring(2, encodedString.Length - 2);

            _compressionRatio = (double)(encodedString.Length * 4) / (input.Length * 8);

            return encodedString;
        }

        public string Decode(string input)
        {
            return "";
        }

        public double GetCompressionRatio()
        {
            return _compressionRatio;
        }
    }
}
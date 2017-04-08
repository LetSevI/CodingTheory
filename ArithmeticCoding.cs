﻿using System.Collections.Generic;
using System.Linq;

namespace CodingTheory
{
    public class ArithmeticCoding : IAlgorithm
    {
        private Dictionary<char, decimal> _frequencies = new Dictionary<char, decimal>();
        private double _compressionRatio = -1;

        public string GetName()
        {
            return "Арифметическое кодирование";
        }
        public string Encode(string input)
        {
            _frequencies = new Dictionary<char, decimal>();
            _compressionRatio = -1;

            foreach (char ch in input)
            {
                if (_frequencies.ContainsKey(ch)) continue;
                decimal frequency = (decimal) input.Count(x => x == ch)/input.Length;
                if (_frequencies.Count > 0)
                {
                    frequency += _frequencies.Max(x => x.Value);
                }
                _frequencies.Add(ch, frequency);
            }

            decimal low = 0.0m;
            decimal high = 1.0m;

            for (int i = 0; i < input.Length; i++)
            {
                decimal range = high - low;
                high = low + range * _frequencies[input[i]];
                if (_frequencies[input[i]] != _frequencies.Min(x => x.Value))
                {
                    low = low + range * _frequencies
                        .Where(x => x.Value < _frequencies[input[i]])
                        .Max(x => x.Value);
                }
            }

            string encodedString = low.ToString();
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
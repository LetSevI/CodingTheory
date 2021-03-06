﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodingTheory
{
    public class AdaptiveArithmeticCoding : IAlgorithm
    {
        private Dictionary<char, decimal> _frequencies = new Dictionary<char, decimal>();
        private double _compressionRatio = -1;

        public string GetName()
        {
            return "Адаптивное арифметическое кодирование";
        }

        public string Encode(string input)
        {
            _frequencies = new Dictionary<char, decimal>();
            _compressionRatio = -1;
            input += 'ソ';

            foreach (char ch in input)
            {
                if (_frequencies.ContainsKey(ch)) continue;
                decimal frequency = 1.0m;
                if (_frequencies.Count > 0)
                {
                    frequency += _frequencies.Max(x => x.Value);
                }
                _frequencies.Add(ch, frequency);
            }
            var symbols = _frequencies.Keys.ToList();
            decimal sumOfFrequencies = symbols.Count;
            var startFrequencies = new Dictionary<char, decimal>(_frequencies);

            decimal low = 0.0m;
            decimal high = 1.0m;

            for (int i = 0; i < input.Length; i++)
            {
                decimal range = high - low;
                high = low + range * _frequencies[input[i]] / sumOfFrequencies;
                if (_frequencies[input[i]] != _frequencies.Min(x => x.Value))
                {
                    low = low + range * _frequencies
                        .Where(x => x.Value < _frequencies[input[i]])
                        .Max(x => x.Value) / sumOfFrequencies;
                }

                for (var j = 0; j < symbols.Count; j++)
                    if (_frequencies[symbols[j]] >= _frequencies[input[i]])
                        _frequencies[symbols[j]]++;
                sumOfFrequencies++;
            }

            string encodedString = low.ToString(CultureInfo.CurrentCulture);
            encodedString = encodedString.Substring(2, encodedString.Length - 2);

            _compressionRatio = (double)(encodedString.Length * 4) / (input.Length * 8);

            _frequencies = startFrequencies;
            return encodedString;
        }

        public string Decode(string input)
        {
            decimal encodedMessage = decimal.Parse(
                string.Format("0{0}{1}", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator, input));

            var symbols = _frequencies.Keys.ToList();
            decimal sumOfFrequencies = symbols.Count;

            decimal low = 0.0m;
            decimal high = 1.0m;
            string decodedString = string.Empty;

            while (decodedString == "" || decodedString[decodedString.Length - 1] != 'ソ')
            {
                decimal range = high - low;

                high = low + range * _frequencies
                        .Where(x => low + x.Value * range / sumOfFrequencies > encodedMessage)
                        .Min(x => x.Value)/sumOfFrequencies;

                char nextChar = _frequencies.FirstOrDefault(x => low + x.Value* range / sumOfFrequencies > encodedMessage).Key;
                decodedString += nextChar;

                if (_frequencies[nextChar] != _frequencies.Min(x => x.Value))
                    low = low + range * _frequencies.Where(x => low + x.Value  * range / sumOfFrequencies <= encodedMessage)
                          .Max(x => x.Value)/sumOfFrequencies;

                for (var i = 0; i < symbols.Count; i++)
                    if (_frequencies[symbols[i]] >= _frequencies[nextChar])
                        _frequencies[symbols[i]]++;
                sumOfFrequencies++;
            }

            return decodedString.Substring(0, decodedString.Length - 1);
        }

        public double GetCompressionRatio()
        {
            return _compressionRatio;
        }
    }
}
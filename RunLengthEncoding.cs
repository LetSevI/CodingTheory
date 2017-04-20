using System;

namespace CodingTheory
{
    public class RunLengthEncoding : IAlgorithm
    {
        private string _transformedString = string.Empty;
        private int _transformedStringNum = -1;
        private double _compressionRatio = -1;

        public string GetName()
        {
            return "Кодирование длин серий (RLE)";
        }

        public string Encode(string input)
        {
            SetDefaultValues();

            string output = string.Empty;

            BurrowsWheelerTransform(input);

            char currentChar = _transformedString[0];
            int count = 1;
            for (int i = 1; i < _transformedString.Length; i++)
            {
                if (_transformedString[i] == currentChar)
                {
                    count++;
                    continue;
                }
                output += string.Format("{0}{1}", count, currentChar);
                currentChar = _transformedString[i];
                count = 1;
            }
            output += string.Format("{0}{1}", count, currentChar);

            _compressionRatio = (double) output.Length / input.Length;

            return output;
        }

        public string Decode(string input)
        {
            string stringCount = string.Empty;
            string burrowsString = string.Empty;

            foreach (char ch in input)
            {
                if (char.IsDigit(ch))
                {
                    stringCount += ch.ToString();
                    continue;
                }

                int n = int.Parse(stringCount);
                for (int j = 0; j < n; j++)
                {
                    burrowsString += ch.ToString();
                }
                stringCount = "";
            }

            return BurrowsWheelerDecode(burrowsString);
        }

        public double GetCompressionRatio()
        {
            return _compressionRatio;
        }

        private void SetDefaultValues()
        {
            _transformedString = string.Empty;
            _transformedStringNum = -1;
            _compressionRatio = -1;
        }

        private void BurrowsWheelerTransform(string input)
        {
            string[] vars = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                vars[i] = input.Substring(i, input.Length - i) + input.Substring(0, i);
            }

            Array.Sort(vars);

            for (int i = 0; i < vars.Length; i++)
            {
                _transformedString += vars[i][vars[i].Length - 1];
                if (vars[i] == input)
                {
                    _transformedStringNum = i;
                }
            }
        }

        private string BurrowsWheelerDecode(string input)
        {
            string[] vars = new string[input.Length];

            foreach (char ch in input)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    vars[j] = input[j] + vars[j];
                }
                Array.Sort(vars);
            }

            return vars[_transformedStringNum];
        }
    }
}

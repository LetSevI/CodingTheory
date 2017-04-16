using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTheory
{
    public class RunLengthEncoding : IAlgorithm
    {
        private string transformedString = string.Empty;
        private int transformedStringNum = -1;
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

            char currentChar = transformedString[0];
            int count = 1;
            for (int i = 1; i < transformedString.Length; i++)
            {
                if (transformedString[i] == currentChar)
                {
                    count++;
                    continue;
                }
                output += string.Format("{0}{1}", count, currentChar);
                currentChar = transformedString[i];
                count = 1;
            }

            _compressionRatio = (double) output.Length / input.Length;

            return output;
        }

        public string Decode(string input)
        {
            return "";
        }

        public double GetCompressionRatio()
        {
            return _compressionRatio;
        }

        private void SetDefaultValues()
        {
            transformedString = string.Empty;
            transformedStringNum = -1;
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
                transformedString += vars[i][vars[i].Length - 1];
                if (vars[i] == input)
                {
                    transformedStringNum = i;
                }
            }
        }
    }
}

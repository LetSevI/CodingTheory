using System.Collections.Generic;
using System.Linq;

namespace CodingTheory
{
    public class LZ78 : IAlgorithm
    {
        private double _compressionRatio = -1;
        private Dictionary<string, int> dictionary = new Dictionary<string, int>();

        public string GetName()
        {
            return "Словарный метод сжатия LZ78";
        }

        public string Encode(string input)
        {
            _compressionRatio = -1;
            dictionary = new Dictionary<string, int>();

            string output = string.Empty;
            int nextNumber = 1;
            int inputLength = input.Length;

            while (input != string.Empty)
            {
                string substr = string.Empty;
                for (var i = 0; i < input.Length; i++)
                {
                    substr += input[i];
                    if (!dictionary.ContainsKey(substr))
                        break;
                }

                if (!dictionary.ContainsKey(substr))
                    dictionary.Add(substr, nextNumber++);
                
                input = input.Substring(substr.Length, input.Length - substr.Length);

                if (substr.Length == 1)
                    output += '0' + substr;
                else
                {
                    int number = dictionary[substr.Substring(0, substr.Length - 1)];
                    output += number.ToString() + substr[substr.Length - 1];
                }
            }

            _compressionRatio = (double)output.Length / inputLength;
            dictionary = new Dictionary<string, int>();
            return output;
        }

        public string Decode(string input)
        {
            string stringCount = string.Empty;
            string output = string.Empty;
            int nextNumber = 1;

            foreach (char ch in input)
            {
                if (char.IsDigit(ch))
                {
                    stringCount += ch.ToString();
                    continue;
                }

                int n = int.Parse(stringCount);
                string substr;

                if (n == 0)
                    substr = string.Format("{0}", ch);
                else
                    substr = dictionary.FirstOrDefault(x => x.Value == n).Key + ch;

                if (!dictionary.ContainsKey(substr))
                    dictionary.Add(substr, nextNumber++);
                output += substr;
                stringCount = "";
            }
            return output;
        }

        public double GetCompressionRatio()
        {
            return _compressionRatio;
        }
    }
}
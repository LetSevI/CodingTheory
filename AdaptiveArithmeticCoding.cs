namespace CodingTheory
{
    public class AdaptiveArithmeticCoding : IAlgorithm
    {
        private double _compressionRatio = -1;

        public string GetName()
        {
            return "Адаптивное арифметическое кодирование";
        }

        public string Encode(string input)
        {
            return "";
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
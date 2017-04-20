namespace CodingTheory
{
    public class LZ78 : IAlgorithm
    {
        private double _compressionRatio = -1;

        public string GetName()
        {
            return "Словарный метод сжатия LZ78";
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
namespace CodingTheory
{
    public interface IAlgorithm
    {
        string Encode(string input);
        string Decode(string input);
        double GetCompressionRatio();
    }
}
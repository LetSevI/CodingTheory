namespace CodingTheory
{
    public interface IAlgorithm
    {
        string GetName();
        string Encode(string input);
        string Decode(string input);
        double GetCompressionRatio();
    }
}
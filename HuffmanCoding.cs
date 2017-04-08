using System.Collections.Generic;
using System.Linq;

namespace CodingTheory
{
    public class HuffmanCoding : IAlgorithm
    {
        private List<Node> _nodes = new List<Node>();
        private Dictionary<char, int> _frequencies = new Dictionary<char, int>();
        private double _compressionRatio = -1;

        public Node Root { get; set; }

        public string GetName()
        {
            return "Кодирование Хаффмана";
        }

        public string Encode(string input)
        {
            Build(input);

            List<bool> encodedSource = new List<bool>();

            if (Root.IsLeaf)
            {
                for (int i = 0; i < Root.Frequency; i++)
                {
                    encodedSource.Add(false);
                }
            }
            else
            {
                for (int i = 0; i < input.Length; i++)
                {
                    List<bool> encodedSymbol = this.Root.Traverse(input[i], new List<bool>());
                    encodedSource.AddRange(encodedSymbol);
                }
            }

            _compressionRatio = (double)encodedSource.Count / (input.Length * 8);

            return encodedSource.Aggregate("", (current, i) => current + (i ? "1" : "0"));
        }

        public string Decode(string input)
        {
            List<bool> bits = input.Select(i => i == '1').ToList();

            Node current = this.Root;
            string decoded = "";

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (!current.IsLeaf) continue;
                decoded += current.Symbol;
                current = this.Root;
            }

            return decoded;
        }

        public double GetCompressionRatio()
        {
            return _compressionRatio;
        }

        private void Build(string source)
        {
            _nodes = new List<Node>();
            _frequencies = new Dictionary<char, int>();
            _compressionRatio = -1;

            for (int i = 0; i < source.Length; i++)
            {
                if (!_frequencies.ContainsKey(source[i]))
                {
                    _frequencies.Add(source[i], 0);
                }

                _frequencies[source[i]]++;
            }

            foreach (KeyValuePair<char, int> symbol in _frequencies)
            {
                _nodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            if (_nodes.Count == 1)
            {
                this.Root = _nodes.FirstOrDefault();
                return;
            }

            while (_nodes.Count > 1)
            {
                List<Node> orderedNodes = _nodes.OrderBy(node => node.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    List<Node> taken = orderedNodes.Take(2).ToList();

                    Node parent = new Node()
                    {
                        Symbol = '*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    _nodes.Remove(taken[0]);
                    _nodes.Remove(taken[1]);
                    _nodes.Add(parent);
                }

                this.Root = _nodes.FirstOrDefault();
            }
        }
    }
}
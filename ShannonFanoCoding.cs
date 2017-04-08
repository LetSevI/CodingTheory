using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingTheory
{
    public class ShannonFanoCoding : IAlgorithm
    {
        private List<Node> _nodes = new List<Node>();
        private Dictionary<char, int> _frequencies = new Dictionary<char, int>();
        private double _compressionRatio = -1;

        public Node Root { get; set; }

        public string GetName()
        {
            return "Кодирование Шеннона-Фано";
        }

        public string Encode(string input)
        {
            _nodes = new List<Node>();
            _frequencies = new Dictionary<char, int>();
            _compressionRatio = -1;

            for (int i = 0; i < input.Length; i++)
            {
                if (!_frequencies.ContainsKey(input[i]))
                {
                    _frequencies.Add(input[i], 0);
                }

                _frequencies[input[i]]++;
            }

            int totalFrequency = 0;
            foreach (KeyValuePair<char, int> symbol in _frequencies)
            {
                _nodes.Add(new Node() { Symbol = symbol.Key, Frequency = symbol.Value });
                totalFrequency += symbol.Value;
            }
            Root = new Node() { Symbol = '*', Frequency = totalFrequency };

            Build(_nodes, Root, totalFrequency);

            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < input.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.Traverse(input[i], new List<bool>());
                if (encodedSymbol.Count == 0)
                    encodedSymbol = new List<bool> { false };
                encodedSource.AddRange(encodedSymbol);
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

        private void Build(List<Node> nodes, Node current, int totalFrequency)
        {
            if (nodes.Count == 1)
            {
                current.Symbol = nodes[0].Symbol;
                current.Frequency = nodes[0].Frequency;
                return;
            }

            List<Node> orderedNodes = nodes.OrderByDescending(node => node.Frequency).ToList();
            int frequency1=0, frequency2=totalFrequency, minDifference = int.MaxValue, i;
            for (i=0; i< orderedNodes.Count; i++)
            {
                frequency2 -= orderedNodes[i].Frequency;
                frequency1 += orderedNodes[i].Frequency;

                if (Math.Abs(frequency2 - frequency1) < minDifference)
                    minDifference = Math.Abs(frequency2 - frequency1);
                else
                {
                    i--;
                    break;
                }
            }

            Node leftNode = new Node() { Symbol = '*', Frequency = frequency1 };
            Node rightNode = new Node() { Symbol = '*', Frequency = frequency2 };
            current.Left = leftNode;
            current.Right = rightNode;
            Build(orderedNodes.GetRange(0, i+1),leftNode, frequency1);
            Build(orderedNodes.GetRange(i+1, orderedNodes.Count-i-1), rightNode, frequency2);
            }
        }
}
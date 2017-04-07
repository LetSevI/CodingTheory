using System.Collections.Generic;

namespace CodingTheory
{
    public class Node
    {
        public char Symbol { get; set; }
        public int Frequency { get; set; }
        public Node Right { get; set; }
        public Node Left { get; set; }
        public bool IsLeaf
        {
            get
            {
                return (Left == null && Right == null);
            }

        }

        public List<bool> Traverse(char symbol, List<bool> data)
        {
            if (Right == null && Left == null)
            {
                return symbol.Equals(this.Symbol) ? data : null;
            }

            List<bool> left = null;
            List<bool> right = null;

            if (this.Left != null)
            {
                List<bool> leftPath = new List<bool>();
                leftPath.AddRange(data);
                leftPath.Add(false);

                left = this.Left.Traverse(symbol, leftPath);
            }

            if (this.Right != null)
            {
                List<bool> rightPath = new List<bool>();
                rightPath.AddRange(data);
                rightPath.Add(true);

                right = this.Right.Traverse(symbol, rightPath);
            }

            return left ?? right;
        }
    }
}

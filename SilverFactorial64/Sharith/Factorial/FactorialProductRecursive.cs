/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

namespace Sharith.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;

    public class ProductRecursive : IFactorialFunction 
    {
        public ProductRecursive() { }

        public string Name
        {
            get { return "ProductRecursive    "; }
        }

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            if (1 < n)
            {
                return RecProduct(1, n);
            }

            return XInt.One;
        }

        private XInt RecProduct(int n, int len)
        {
            if (1 < len)
            {
                int l = len >> 1;
                return RecProduct(n, l) * RecProduct(n + l, len - l);
            }

            return new XInt(n);
        }
    }
}   // endOfFactorialProductRecursive

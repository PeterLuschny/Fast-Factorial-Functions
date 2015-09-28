// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using XInt = Arithmetic.XInt;

    public class ProductRecursive : IFactorialFunction 
    {
        public string Name => "ProductRecursive    ";

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            if (1 < n)
            {
                return this.RecProduct(1, n);
            }

            return XInt.One;
        }

        private XInt RecProduct(int n, int len)
        {
            if (1 < len)
            {
                var l = len >> 1;
                return this.RecProduct(n, l) * this.RecProduct(n + l, len - l);
            }

            return new XInt(n);
        }
    }
} // endOfFactorialProductRecursive

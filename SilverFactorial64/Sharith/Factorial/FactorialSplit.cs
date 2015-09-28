// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using XInt = Arithmetic.XInt;
    using XMath = MathUtils.XMath;

    public class Split : IFactorialFunction 
    {
        public string Name => "Split               ";

        XInt currentN;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            if (n < 2) return XInt.One;

            var p = XInt.One;
            var r = XInt.One;
            this.currentN = XInt.One;

            int h = 0, shift = 0, high = 1;
            var log2N = XMath.FloorLog2(n);

            while (h != n)
            {
                shift += h;
                h = n >> log2N--;
                var len = high;
                high = (h - 1) | 1;
                len = (high - len) / 2;

                if (len > 0)
                {
                    p *= this.Product(len);
                    r *= p;
                }
            }

            return r << shift;
        }

        private XInt Product(int n)
        {
            var m = n / 2;
            if (m == 0) return this.currentN += 2;
            if (n == 2) return (this.currentN += 2) * (this.currentN += 2);
            return this.Product(n - m) * this.Product(m);
        }
    }
} // endOfFactorialBinSplit

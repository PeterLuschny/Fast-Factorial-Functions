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

    public class BoitenSplit : IFactorialFunction 
    {
        public string Name => "BoitenSplit         ";

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

            int h = 0, shift = 0, k = 1;
            var log2N = XMath.FloorLog2(n);

            while (h != n)
            {
                shift += h;
                h = n >> log2N--;
                var high = (h & 1) == 1 ? h : h - 1;

                while (k != high)
                {
                    k += 2;
                    p *= k;
                }
                r *= p;
            }

            return r << shift;
        }
    }
} // endOfFactorialBoitenSplit

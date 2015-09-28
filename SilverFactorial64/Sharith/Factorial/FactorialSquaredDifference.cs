// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using XInt = Arithmetic.XInt;

    public class SquaredDifference : IFactorialFunction 
    {
        public string Name => "SquaredDifference   ";

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                          this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            if (n < 2)
            {
                return XInt.One;
            }

            long h = n / 2;
            var q = h * h;
            var r = (n & 1) == 1 ? 2 * q * n : 2 * q;
            var f = new XInt(r);

            for (var d = 1; d < n - 2; d += 2)
            {
                f *= q -= d;
            }

            return f;
        }
    }
} // endOfFactorialSquaredDifference

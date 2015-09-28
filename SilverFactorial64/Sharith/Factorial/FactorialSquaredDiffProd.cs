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

    public class SquaredDiffProd : IFactorialFunction 
    {
        public string Name => "SquaredDiffProduct  ";

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                          this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            if (n < 7)
            {
                return (XInt)(new[] { 1, 1, 2, 6, 24, 120, 720 })[n];
            }

            long h = n / 2;
            var q = h * h;
            var f = new long[(int)h];
            f[0] = (n & 1) == 1 ? 2 * q * n : 2 * q;
            var i = 1;

            for (var d = 1; d < n - 2; d += 2)
            {
                f[i++] = q -= d;
            }

            return XMath.Product(f, f.Length);
        }
    }
} // endOfFactorialSquaredDiffProd

// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
    using System.Threading.Tasks;

    using XInt = Arithmetic.XInt;
    using XMath = MathUtils.XMath;

    public class CrandallPomerance : IFactorialFunction
    {
        public string Name => "CrandallPomerance   ";

        private static XInt CPpoly(XInt x)
        {
            // Implemented after Fredrik Johansson's arb-function
            // rising_fmprb_ui_bsplit_eight (please speak aloud).
            // x(x+1)...(x+7) = (28+98x+63x^2+14x^3+x^4)^2-16(7+2x)^2 

            // t = x^2, v = x^3, u = x^4 
            var t = x * x;
            var v = x * t;
            var u = t * t;

            // u = (28 + 98x + 63x^2 + 14x^3 + x^4)^2
            u += v * 14u;
            u += t * 63u;
            u += x * 98u;
            u += 28;
            u *= u;

            // 16 (7+2x)^2 = 784 + 448x + 64x^2 
            u -= 784u;
            u -= x * 448u;
            u -= t << 6;
            return u ;
        }

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            int r = n % 8, s = n / 8 + 1;
            var rf = (long)(new int[] { 1, 1, 2, 6, 24, 120, 720, 5040 })[r];

            if (n < 8)
            {
                return rf;
            }

            var factors = new XInt[s];
            factors[s - 1] = rf;

            Parallel.For(0, s - 1, i =>
            {
                factors[i] = CPpoly(i * 8 + r + 1);
            });

            return XMath.Product(factors, 0, s);
        }
    }
} // endOfCrandallPomerance

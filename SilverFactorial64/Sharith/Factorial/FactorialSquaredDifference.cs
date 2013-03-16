/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

namespace Sharith.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;

    public class SquaredDifference : IFactorialFunction 
    {
        public SquaredDifference() { }

        public string Name
        {
            get { return "SquaredDifference   "; }
        }                

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            if (n < 2)
            {
                return XInt.One;
            }

            long h = n / 2;
            long q = h * h;
            long r = (n & 1) == 1 ? 2 * q * n : 2 * q;
            var f = new XInt(r);

            for (int d = 1; d < n - 2; d += 2)
            {
                f *= q -= d;
            }

            return f;
        }
    }
} //endOfFactorialSquaredDifference

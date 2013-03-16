/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2013-02-18

namespace Sharith.Math.Factorial
{
    using XInt = Sharith.Arithmetic.XInt;
    using XMath = Sharith.Math.MathUtils.XMath;

    public class Balkan : IFactorialFunction
    {
        public Balkan() { }

        public string Name
        {
            get { return "Balkan              "; }
        }

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    "n", Name + ": n >= 0 required, but was " + n);
            }

            if (n < 7)
            {
                return (XInt)(new int[] { 1, 1, 2, 6, 24, 120, 720 })[n];
            }

            int i = 1, loop = n / 2;
            var f = new long[loop + (n & 1)];

            f[0] = loop;
            if ((n & 1) == 1) { f[loop] = n; }

            long s = loop, t;

            for (int inc = loop - 1; inc > 0; inc--)
            {
                s += inc;
                t = s;

                while ((t & 1) == 0)
                {
                    t /= 2;
                    loop++;
                }
                f[i++] = t;
            }
            return XMath.Product(f) << loop;

        }
    } // endOfFactorialBalkan
}

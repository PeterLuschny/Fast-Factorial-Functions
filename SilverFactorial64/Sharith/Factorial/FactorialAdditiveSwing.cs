/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

namespace Sharith.Math.Factorial
{
    using XInt = Sharith.Arithmetic.XInt;
    
    public class AdditiveSwing : IFactorialFunction 
    {
        public AdditiveSwing() { }

        public string Name
        {
            get { return "AdditiveSwing       "; }
        }

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            return RecFactorial(n);
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            return XInt.Pow(RecFactorial(n / 2), 2) * Swing(n);
        }

        private static XInt Swing(int n)
        {
            XInt w = XInt.One;

            if (n > 1)
            {
                n = n + 2;
                var s = new XInt[n + 1];

                s[0] = s[1] = XInt.Zero;
                s[2] = w;

                for (int m = 3; m <= n; m++)
                {
                    s[m] = s[m - 2];
                    for (int k = m; k >= 2; k--)
                    {
                        s[k] += s[k - 2];
                        if ((k & 1) == 1) // if k is odd
                        {
                            s[k] += s[k - 1];
                        }
                    }
                }
                w = s[n];
            }
            return w;
        }
    }
} // endOfFactorialAdditiveSwing

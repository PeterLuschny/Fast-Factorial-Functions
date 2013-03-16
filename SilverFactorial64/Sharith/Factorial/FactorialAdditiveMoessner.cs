/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

namespace Sharith.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;

    public class AdditiveMoessner : IFactorialFunction 
    {
        public AdditiveMoessner() { }

        public string Name
        {
            get { return "AdditiveMoessner    "; }
        }

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            XInt[] s = new XInt[n + 1];
            s[0] = XInt.One;

            for (int m = 1; m <= n; m++)
            {
                s[m] = XInt.Zero;
                for (int k = m; k >= 1; k--)
                {
                    for (int i = 1; i <= k; i++)
                    {
                        s[i] += s[i - 1];
                    }
                }
            }
            return s[n];
        }
    }
} // endOfFactorialAdditiveMoessner

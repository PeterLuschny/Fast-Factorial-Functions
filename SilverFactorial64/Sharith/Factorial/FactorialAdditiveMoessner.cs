// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using XInt = Arithmetic.XInt;

    public class AdditiveMoessner : IFactorialFunction 
    {
        public string Name => "AdditiveMoessner    ";

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            var s = new XInt[n + 1];
            s[0] = XInt.One;

            for (var m = 1; m <= n; m++)
            {
                s[m] = XInt.Zero;
                for (var k = m; k >= 1; k--)
                {
                    for (var i = 1; i <= k; i++)
                    {
                        s[i] += s[i - 1];
                    }
                }
            }
            return s[n];
        }
    }
} // endOfFactorialAdditiveMoessner

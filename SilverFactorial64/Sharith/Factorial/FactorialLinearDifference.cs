// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using XInt = Arithmetic.XInt;

    public class LinearDifference : IFactorialFunction 
    {
        public string Name => "LinearDifference    ";

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            if (n < 2) return XInt.One;

            XInt f;

            switch (n % 4)
            {
                case 1: f = (XInt) n; break;
                case 2: f = (XInt)((long)n * (n - 1)); break;
                case 3: f = (XInt)((long)n * (n - 1) * (n - 2)); break;
                default: f = XInt.One; break;
            }

            long prod = 24;
            long diff1 = 1656;
            long diff2 = 8544;
            long diff3 = 13056;

            var i = n / 4;
            while (i-- > 0)
            {
                f = f * prod; 
                prod += diff1;
                diff1 += diff2;
                diff2 += diff3;
                diff3 += 6144;
            }
            return f;
        }
    }
}  // endOfFactorialLinearDifference

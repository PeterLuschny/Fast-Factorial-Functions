// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

#if(MPIR)
namespace SharithMP.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;
#else
    namespace Sharith.Math.Factorial {
    using XInt = System.Numerics.BigInteger;
#endif

    public class SwingRational : IFactorialFunction 
    {
        public SwingRational() { }
        
        public string Name
        {
            get { return "SwingRational       "; }
        }

        private long den, num, h;
        private int i;

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

        private XInt Swing(int n)
        {
            switch (n % 4)
            {
                case 1: h = n / 2 + 1; break;
                case 2: h = 2; break;
                case 3: h = 2 * (n / 2 + 2); break;
                default: h = 1; break;
            }

            num = 2 * (n + 2 - ((n + 1) & 1));
            den = 1;
            i = n / 4;

            return Product(i + 1).Numerator;
        }

        private Rational Product(int l)
        {
            if (l > 1)
            {
                int m = l / 2;
                return Product(m) * Product(l - m);
            }

            if (i-- > 0)
            {
                return new Rational(num -= 4, den++);
            }

            return new Rational(h, 1);
        }

        //-------------------------------------------------------------
        // A minimalistic rational arithmetic *only* for the use of
        // SwingRationalDouble. The sole purpose for inclusion
        // here is to make the description of the algorithm more
        // independent and more easy to port.
        //-------------------------------------------------------------
        private class Rational
        {
            private XInt num; // Numerator
            private XInt den; // Denominator

            public XInt Numerator
            {
                get
                {
                    XInt cd = XInt.GreatestCommonDivisor(num, den);
                    return num / cd;
                }
            }

            public Rational(long _num, long _den)
            {
                long cd = Gcd(_den, _num);
                num = new XInt(_num / cd);
                den = new XInt(_den / cd);
            }

            public Rational(XInt _num, XInt _den)
            {
                // If (and only if) the arithmetic supports a
                // *real* fast Gcd this would lead to a speed up:
                // XInt cd = XInt.Gcd(_num, _den);
                // num = new XInt(_num / cd);
                // den = new XInt(_den / cd);
                num = _num;
                den = _den;
            }

            public static Rational operator *(Rational a, Rational r)
            {
                return new Rational(a.num * r.num, a.den * r.den);
            }

            private static long Gcd(long a, long b)
            {
                long x, y;

                if (a < b) { x = b; y = a; }
                else { x = a; y = b; }

                while (y != 0)
                {
                    long t = x % y; x = y; y = t;
                }
                return x;
            }
        } // endOfRational
    }
} // endOfSwingRational

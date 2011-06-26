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

    public class SwingRationalDouble : IFactorialFunction 
    {
        public SwingRationalDouble() { }

        public string Name
        {
            get { return "SwingRationalDouble "; }
        }                

        private long den, num, g, h;
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

            return XInt.Pow(RecFactorial(n / 2),2) * Swing(n);
        }

        private XInt Swing(int n)
        {
            bool oddN = (n & 1) == 1;
            bool div = false;
            h = n / 2;

            switch ((n / 2) % 4)
            {
                case 0: h = oddN ? h + 1 : 1; break;
                case 1: h = oddN ? 2 * (h + 2) : 2; break;
                case 2: h = oddN ? 2 * (h + 1) * (h + 3) : 2 * (h + 1);
                    div = n > 7; break;
                case 3: h = oddN ? 4 * (h + 2) * (h + 4) : 4 * (h + 2);
                    div = n > 7; break;
            }

            g = div ? n / 4 : 1;
            num = 2 * (n + 3 + (n & 1));
            den = -1;
            i = n / 8;

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
                num -= 8;
                den += 2;
                return new Rational(num * (num - 4), den * (den + 1));
            }

            return new Rational(h, g);
        }

        //----------------------------------------------------------
        // A minimalistic rational arithmetic *only* for the use of
        // SwingRational. The sole purpose for inclusion
        // here is to make the description of the algorithm more
        // independent and more easy to port.
        //---------------------------------------------------------
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
                long g = Gcd(_num, _den);
                num = new XInt(_num / g);
                den = new XInt(_den / g);
            }

            public Rational(XInt _num, XInt _den)
            {
                //  If (and only if) the arithmetic supports a
                //  *real* fast Gcd this would lead to a speed up:
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

                if (a >= b) { x = a; y = b; }
                else { x = b; y = a; }

                while (y != 0)
                {
                    long t = x % y; x = y; y = t;
                }
                return x;
            }
        } // endOfRational
    }

} // endOfSwingRationalDouble

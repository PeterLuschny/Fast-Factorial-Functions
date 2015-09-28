// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using XInt = Arithmetic.XInt;

    public class SwingRationalDouble : IFactorialFunction 
    {
        public string Name => "SwingRationalDouble ";
        long den, num, g, h;
        int i;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            return this.RecFactorial(n);
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            return XInt.Pow(this.RecFactorial(n / 2),2) * this.Swing(n);
        }

        private XInt Swing(int n)
        {
            bool oddN = (n & 1) == 1;
            bool div = false;
            this.h = n / 2;

            switch ((n / 2) % 4)
            {
                case 0: this.h = oddN ? this.h + 1 : 1; break;
                case 1: this.h = oddN ? 2 * (this.h + 2) : 2; break;
                case 2: this.h = oddN ? 2 * (this.h + 1) * (this.h + 3) : 2 * (this.h + 1);
                        div = n > 7; break;
                case 3: this.h = oddN ? 4 * (this.h + 2) * (this.h + 4) : 4 * (this.h + 2);
                        div = n > 7; break;
            }

            this.g = div ? n / 4 : 1;
            this.num = 2 * (n + 3 + (n & 1));
            this.den = -1;
            this.i = n / 8;

            return this.Product(this.i + 1).Numerator;
        }

        private Rational Product(int l)
        {
            if (l > 1)
            {
                var m = l / 2;
                return this.Product(m) * this.Product(l - m);
            }

            if (this.i-- > 0)
            {
                this.num -= 8;
                this.den += 2;
                return new Rational(this.num * (this.num - 4), this.den * (this.den + 1));
            }

            return new Rational(this.h, this.g);
        }

        //----------------------------------------------------------
        // A minimalistic rational arithmetic *only* for the use of
        // SwingRational. The sole purpose for inclusion
        // here is to make the description of the algorithm more
        // independent and more easy to port.
        //---------------------------------------------------------
        private class Rational
        {
            readonly XInt num; // Numerator
            readonly XInt den; // Denominator

            public XInt Numerator
            {
                get
                {
                    var cd = XInt.GreatestCommonDivisor(this.num, this.den);
                    return this.num / cd;
                }
            }

            public Rational(long _num, long _den)
            {
                long g = Gcd(_num, _den);
                this.num = new XInt(_num / g);
                this.den = new XInt(_den / g);
            }

            public Rational(XInt _num, XInt _den)
            {
                //  If (and only if) the arithmetic supports a
                //  *real* fast Gcd this would lead to a speed up:
                this.num = _num;
                this.den = _den;
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

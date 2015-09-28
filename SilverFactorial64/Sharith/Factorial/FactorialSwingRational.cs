// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using XInt = Arithmetic.XInt;

    public class SwingRational : IFactorialFunction 
    {
        public string Name => "SwingRational       ";

        long den, num, h;
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

            return XInt.Pow(this.RecFactorial(n / 2), 2) * this.Swing(n);
        }

        private XInt Swing(int n)
        {
            switch (n % 4)
            {
                case 1: this.h = n / 2 + 1; break;
                case 2: this.h = 2; break;
                case 3: this.h = 2 * (n / 2 + 2); break;
                default: this.h = 1; break;
            }

            this.num = 2 * (n + 2 - ((n + 1) & 1));
            this.den = 1;
            this.i = n / 4;

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
                return new Rational(this.num -= 4, this.den++);
            }

            return new Rational(this.h, 1);
        }

        //-------------------------------------------------------------
        // A minimalistic rational arithmetic *only* for the use of
        // SwingRationalDouble. The sole purpose for inclusion
        // here is to make the description of the algorithm more
        // independent and more easy to port.
        //-------------------------------------------------------------
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
                long cd = Gcd(_den, _num);
                this.num = new XInt(_num / cd);
                this.den = new XInt(_den / cd);
            }

            public Rational(XInt _num, XInt _den)
            {
                // If (and only if) the arithmetic supports a
                // *real* fast Gcd this would lead to a speed up:
                // XInt cd = XInt.Gcd(_num, _den);
                // num = new XInt(_num / cd);
                // den = new XInt(_den / cd);
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

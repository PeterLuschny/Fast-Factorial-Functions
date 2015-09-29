// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialSwingRationalDouble implements IFactorialFunction {

    public FactorialSwingRationalDouble() {
    }
    private long D, N, g, h;
    private int i;

    @Override
    public String getName() {
        return "SwingRationalDbl  ";
    }

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        return recFactorial(n);
    }

    private Xint recFactorial(int n) {
        if (n < 2) {
            return Xint.ONE;
        }

        return recFactorial(n / 2).square().multiply(swing(n));
    }

    private Xint swing(int n) {
        boolean oddN = (n & 1) == 1;
        boolean div = false;
        h = n / 2;

        switch ((n / 2) % 4) {
            case 0:
                h = oddN ? h + 1 : 1;
                break;
            case 1:
                h = oddN ? 2 * (h + 2) : 2;
                break;
            case 2:
                h = oddN ? 2 * (h + 1) * (h + 3) : 2 * (h + 1);
                div = n > 7;
                break;
            case 3:
                h = oddN ? 4 * (h + 2) * (h + 4) : 4 * (h + 2);
                div = n > 7;
                break;
            // Need no default case here.    
        }

        g = div ? n / 4 : 1;
        N = 2 * (n + 3 + (n & 1));
        D = -1;
        i = n / 8;

        return product(i + 1).getNumerator();
    }

    private Rational product(int l) {
        if (l > 1) {
            int m = l / 2;
            return product(m).multiply(product(l - m));
        }

        if (i-- > 0) {
            N -= 8;
            D += 2;
            return new Rational(N * (N - 4), D * (D + 1));
        }

        return new Rational(h, g);
    }

    // --------------------------------------------------------
    // A minimalistic rational arithmetic *only* for the use of
    // FactorialSwingRational. The sole purpose for inclusion
    // here is to make the description of the algorithm more
    // independent and more easy to port.
    // ---------------------------------------------------------
    private class Rational {

        private final Xint num; // Numerator
        private final Xint den; // Denominator

        public Rational(long _num, long _den) {
            long c = gcd(_num, _den);
            num = Xint.valueOf(_num / c);
            den = Xint.valueOf(_den / c);
        }

        public Rational(Xint _num, Xint _den) {
            // If the arithmetic supports a *real* fast gcd
            // this would lead to a speed up:
            // Xint cd = _num.gcd(_den);
            // num = _num.divide(cd);
            // den = _den.divide(cd);
            num = _num;
            den = _den;
        }

        public Xint getNumerator() {
            Xint cd = num.gcd(den);
            return num.divide(cd);
        }

        public Rational multiply(Rational r) {
            return new Rational(num.multiply(r.num), den.multiply(r.den));
        }

        private long gcd(long a, long b) {
            long x, y;

            if (a >= b) {
                x = a;
                y = b;
            } else {
                x = b;
                y = a;
            }

            while (y != 0) {
                long t = x % y;
                x = y;
                y = t;
            }
            return x;
        }
    } // endOfRational
} // endOfSwingRational

// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialSwingRational implements IFactorialFunction {

    private long D, N, h;
    private int i;

    public FactorialSwingRational() {
    }

    @Override
    public String getName() {
        return "SwingRational     ";
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
        switch (n % 4) {
            case 1:
                h = n / 2 + 1;
                break;
            case 2:
                h = 2;
                break;
            case 3:
                h = 2 * (n / 2 + 2);
                break;
            default:
                h = 1;
                break;
        }

        N = 2 * (n + 2 - ((n + 1) & 1));
        D = 1;
        i = n / 4;

        return product(i + 1).getNumerator();
    }

    private Rational product(int l) {
        if (l > 1) {
            int m = l / 2;
            return product(m).multiply(product(l - m));
        }

        if (i-- > 0) {
            return new Rational(N -= 4, D++);
        }

        return new Rational(h, 1);
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

        public Xint getNumerator() {
            Xint cd = num.gcd(den);
            return num.divide(cd);
        }

        public Rational(long _num, long _den) {
            long g = gcd(_num, _den);
            num = Xint.valueOf(_num / g);
            den = Xint.valueOf(_den / g);
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

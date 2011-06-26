// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialSquaredDiff implements IFactorialFunction {

    public FactorialSquaredDiff() {
    }

    @Override
    public String getName() {
        return "SquaredDiff       ";
    }

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        if (n < 2) {
            return Xint.ONE;
        }

        long h = n / 2, q = h * h;
        long r = (n & 1) == 1 ? 2 * q * n : 2 * q;
        Xint f = Xint.valueOf(r);

        for (int d = 1; d < n - 2; d += 2) {
            f = f.multiply(q -= d);
        }

        return f;
    }
} // endOfFactorialSqrDiff

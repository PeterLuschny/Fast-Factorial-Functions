// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialSquaredDiffProd implements IFactorialFunction {

    public FactorialSquaredDiffProd() {
    }

    @Override
    public String getName() {
        return "SquaredDiffProduct";
    }

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        if (n < 4) {
            return Xint.valueOf(n < 2 ? 1 : n == 2 ? 2 : 6);
        }

        long h = n / 2, q = h * h;
        long[] f = new long[(int) h];
        f[0] = (n & 1) == 1 ? 2 * q * n : 2 * q;
        int i = 1;

        for (int d = 1; d < n - 2; d += 2) {
            f[i++] = q -= d;
        }

        return Xint.product(f);
    }
} // endOfFactorialSquaredrDiffProd

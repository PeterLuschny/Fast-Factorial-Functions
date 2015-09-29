// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialAdditiveSwing implements IFactorialFunction {

    public FactorialAdditiveSwing() {
    }

    @Override
    public String getName() {
        return "AdditiveSwing       ";
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
        Xint w = Xint.ONE;

        if (n > 1) {
            n = n + 2;
            Xint[] s = new Xint[n + 1];

            s[0] = s[1] = Xint.ZERO;
            s[2] = w;

            for (int m = 3; m <= n; m++) {
                s[m] = s[m - 2];
                for (int k = m; k >= 2; k--) {
                    s[k] = s[k].add(s[k - 2]);
                    if ((k & 1) == 1) // if k is odd
                    {
                        s[k] = s[k].add(s[k - 1]);
                    }
                }
            }
            w = s[n];
        }
        return w;
    }
} // endOfFactorialAdditiveSwing

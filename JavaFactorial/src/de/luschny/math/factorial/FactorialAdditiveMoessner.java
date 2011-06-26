// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialAdditiveMoessner implements IFactorialFunction {

    public FactorialAdditiveMoessner() {
    }

    @Override
    public String getName() {
        return "AdditiveMoessner    ";
    }

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        Xint[] s = new Xint[n + 1];
        s[0] = Xint.ONE;

        for (int m = 1; m <= n; m++) {
            s[m] = Xint.ZERO;
            for (int k = m; k >= 1; k--) {
                for (int i = 1; i <= k; i++) {
                    s[i] = s[i].add(s[i - 1]);
                }
            }
        }
        return s[n];
    }
} // endOfFactorialMoessner

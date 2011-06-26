// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialHyper implements IFactorialFunction {

    public FactorialHyper() {
    }

    @Override
    public String getName() {
        return "Hyper             ";
    }
    private boolean nostart;
    private long S, K, A;

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        nostart = false;
        int h = n / 2;
        S = h + 1;
        K = S + h;
        A = (n & 1) == 1 ? K : 1;
        if ((h & 1) == 1) {
            A = -A;
        }
        K += 4;

        return hyperFact(h + 1).shiftLeft(h);
    }

    private Xint hyperFact(int l) {
        if (l > 1) {
            int m = l / 2;
            return hyperFact(m).multiply(hyperFact(l - m));
        }

        if (nostart) {
            S -= K -= 4;
            return Xint.valueOf(S);
        }

        nostart = true;
        return Xint.valueOf(A);
    }
} // endOfFactorialHyper

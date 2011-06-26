// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialSplit implements IFactorialFunction {

    public FactorialSplit() {
    }

    @Override
    public String getName() {
        return "Split             ";
    }
    private long N;

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        if (n < 2) {
            return Xint.ONE;
        }

        Xint p = Xint.ONE;
        Xint r = Xint.ONE;
        N = 1;

        // log2n = floor(log2(n));
        int log2n = 31 - Integer.numberOfLeadingZeros(n);
        int h = 0, shift = 0, high = 1;

        while (h != n) {
            shift += h;
            h = n >>> log2n--;
            int len = high;
            high = (h & 1) == 1 ? h : h - 1;
            len = (high - len) / 2;

            if (len > 0) {
                p = p.multiply(product(len));
                r = r.multiply(p);
            }
        }
        return r.shiftLeft(shift);
    }

    private Xint product(int n) {
        int m = n / 2;
        if (m == 0) {
            return Xint.valueOf(N += 2);
        }
        if (n == 2) {
            return Xint.valueOf((N += 2) * (N += 2));
        }
        return product(n - m).multiply(product(m));
    }
} // endOfFactorialSplitRecursive

// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialBoitenSplit implements IFactorialFunction {

    public FactorialBoitenSplit() {
    }

    @Override
    public String getName() {
        return "BoitenSplit       ";
    }

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

        // log2n = floor(log2(n));
        int log2n = 31 - Integer.numberOfLeadingZeros(n);
        int h = 0, shift = 0, k = 1;

        while (h != n) {
            shift += h;
            h = n >>> log2n--;
            int high = (h & 1) == 1 ? h : h - 1;

            while (k != high) {
                k += 2;
                p = p.multiply(k);
            }
            r = r.multiply(p);
        }
        return r.shiftLeft(shift);
    }
} // endOfFactorialSplitBoiten

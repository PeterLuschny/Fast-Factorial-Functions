// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialProductRecursive implements IFactorialFunction {

    public FactorialProductRecursive() {
    }

    @Override
    public String getName() {
        return "ProductRecursive  ";
    }

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        if (1 < n) {
            return recProduct(1, n);
        }

        return Xint.ONE;
    }

    private Xint recProduct(int n, int len) {
        if (1 < len) {
            int l = len / 2;
            return recProduct(n, l).multiply(recProduct(n + l, len - l));
        }

        return Xint.valueOf(n);
    }
} // endOfFactorialProductRecursive

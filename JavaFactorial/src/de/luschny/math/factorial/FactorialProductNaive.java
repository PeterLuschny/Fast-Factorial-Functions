// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialProductNaive implements IFactorialFunction {

    public FactorialProductNaive() {
    }

    @Override
    public String getName() {
        return "ProductNaive        ";
    }

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        Xint nFact = Xint.ONE;

        for (int i = 2; i <= n; i++) {
            nFact = nFact.multiply(i);
        }

        return nFact;
    }
} // endOfFactorialProductNaive

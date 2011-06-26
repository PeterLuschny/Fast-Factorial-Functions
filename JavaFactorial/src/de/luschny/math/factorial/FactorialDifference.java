// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialDifference implements IFactorialFunction {

    public FactorialDifference() {
    }

    @Override
    public String getName() {
        return "Difference          ";
    }

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        if (n < 2) {
            return Xint.ONE;
        }

        Xint f;

        switch (n % 4) {
            case 1:
                f = Xint.valueOf(n);
                break;
            case 2:
                f = Xint.valueOf((long) n * (n - 1));
                break;
            case 3:
                f = Xint.valueOf((long) n * (n - 1) * (n - 2));
                break;
            default:
                f = Xint.ONE;
        }

        long prod = 24;
        long diff1 = 1656;
        long diff2 = 8544;
        long diff3 = 13056;

        int i = n / 4;
        while (i-- > 0) {
            f = f.multiply(Xint.valueOf(prod));
            prod += diff1;
            diff1 += diff2;
            diff2 += diff3;
            diff3 += 6144;
        }

        return f;
    }
} // endOfFactorialDifference

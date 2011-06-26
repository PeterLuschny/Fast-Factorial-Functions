// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialSwing implements IFactorialFunction {

    public FactorialSwing() {
    }

    @Override
    public String getName() {
        return "Swing             ";
    }
    private Xint ndiv4OddFact, ndiv2OddFact;

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        ndiv4OddFact = ndiv2OddFact = Xint.ONE;

        return oddFactorial(n).shiftLeft(n - Integer.bitCount(n));
    }

    private Xint oddFactorial(int n) {
        Xint oddFact;
        if (n < 17) {
            oddFact = Xint.valueOf(smallOddFactorial[n]);
        } else {
            oddFact = oddFactorial(n / 2).square().multiply(oddSwing(n));
        }

        ndiv4OddFact = ndiv2OddFact;
        ndiv2OddFact = oddFact;
        return oddFact;
    }

    private Xint oddSwing(int n) {
        if (n < 33) {
            return Xint.valueOf(smallOddSwing[n]);
        }

        int len = (n - 1) / 4;
        if ((n % 4) != 2) {
            len++;
        }
        int high = n - ((n + 1) & 1);

        int ndiv4 = n / 4;
        Xint oddFact = ndiv4 < 17 ? Xint.valueOf(smallOddFactorial[ndiv4]) : ndiv4OddFact;

        return product(high, len).divide(oddFact);
    }

    private static Xint product(int m, int len) {
        if (len == 1) {
            return Xint.valueOf(m);
        }
        if (len == 2) {
            return Xint.valueOf((long) m * (m - 2));
        }

        int hlen = len >>> 1;
        return product(m - hlen * 2, len - hlen).multiply(product(m, hlen));
    }
    private static int[] smallOddSwing = {1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231, 3003, 429, 6435, 6435, 109395, 12155, 230945, 46189, 969969,
        88179, 2028117, 676039, 16900975, 1300075, 35102025, 5014575, 145422675, 9694845, 300540195, 300540195};
    private static int[] smallOddFactorial = {1, 1, 1, 3, 3, 15, 45, 315, 315, 2835, 14175, 155925, 467775, 6081075, 42567525, 638512875, 638512875};
} // endOfFactorialSwing

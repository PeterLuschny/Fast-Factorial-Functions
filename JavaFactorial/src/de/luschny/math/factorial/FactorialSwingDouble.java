// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialSwingDouble implements IFactorialFunction {

    public FactorialSwingDouble() {
    }

    @Override
    public String getName() {
        return "SwingDouble       ";
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
        boolean oddN = (n & 1) == 1;
        boolean div = false;
        long h = n / 2;

        switch ((n / 2) % 4) {
            case 0:
                h = oddN ? h + 1 : 1;
                break;
            case 1:
                h = oddN ? 2 * (h + 2) : 2;
                break;
            case 2:
                h = oddN ? 2 * (h + 1) * (h + 3) : 2 * (h + 1);
                div = n > 7;
                break;
            case 3:
                h = oddN ? 4 * (h + 2) * (h + 4) : 4 * (h + 2);
                div = n > 7;
                break;
            // We do not need a default case here.    
        }

        Xint b = Xint.valueOf(h);

        long D = 1, N = oddN ? 2 * (long)n : 2 * (long)(n - 1);

        for (int i = n / 8; i > 0; --i) {
            long num = N * (N - 4), g = num;
            long den = D * (D + 1), f = den;

            N -= 8;
            D += 2;

            while (f != 0) {
                long t = g % f;
                g = f;
                f = t;
            }

            b = b.multiply(num / g).divide(den / g);
        }

        if (div) {
            b = b.divide(n / 4);
        }

        return b;
    }
} // endOfSwingDouble

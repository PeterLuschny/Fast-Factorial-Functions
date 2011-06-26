// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

public class FactorialSwingSimple implements IFactorialFunction {

    public FactorialSwingSimple() {
    }

    @Override
    public String getName() {
        return "SwingSimple         ";
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
        int z;

        switch (n % 4) {
            case 1:
                z = n / 2 + 1;
                break;
            case 2:
                z = 2;
                break;
            case 3:
                z = 2 * (n / 2 + 2);
                break;
            default:
                z = 1;
                break;
        }

        Xint b = Xint.valueOf(z);
        z = 2 * (n - ((n + 1) & 1));

        for (int i = 1; i <= n / 4; i++, z -= 4) {
            b = b.multiply(z).divide(i);
        }

        return b;
    }
} // endOfFactorialSwing

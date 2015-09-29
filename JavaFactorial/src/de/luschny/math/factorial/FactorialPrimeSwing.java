// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.Xmath;
import de.luschny.math.arithmetic.Xint;
import de.luschny.math.primes.IPrimeIteration;
import de.luschny.math.primes.PrimeSieve;

public class FactorialPrimeSwing implements IFactorialFunction {

    private PrimeSieve sieve;
    private int[] primeList;

    public FactorialPrimeSwing() {
    }

    @Override
    public String getName() {
        return "PrimeSwing        ";
    }

    @Override
    public Xint factorial(int n) {
        // For very small n the 'NaiveFactorial' is OK.
        if (n < 20) {
            return Xmath.smallFactorial(n);
        }

        int pLen = (int) (2.0 * Math.floor(Math.sqrt(n) + n / (Xmath.log2(n) - 1)));
        primeList = new int[pLen];
        sieve = new PrimeSieve(n);

        return recFactorial(n).shiftLeft(n - Integer.bitCount(n));
    }

    private Xint recFactorial(int n) {
        if (n < 2) {
            return Xint.ONE;
        }
        return recFactorial(n / 2).square().multiply(swing(n));
    }

    private Xint swing(int n) {
        if (n < 33) {
            return Xint.valueOf(smallOddSwing[n]);
        }

        // sieve and primeList initialized in function 'factorial'!
        int sqrtN = (int) Math.floor(Math.sqrt(n));
        IPrimeIteration pIter0 = sieve.getIteration(3, sqrtN);
        IPrimeIteration pIter1 = sieve.getIteration(sqrtN + 1, n / 3);

        int count = 0;

        for (int prime : pIter0) {
            int q = n, p = 1;

            while ((q /= prime) > 0) {
                if ((q & 1) == 1) {
                    p *= prime;
                }
            }

            if (p > 1) {
                primeList[count++] = p;
            }
        }

        for (int prime : pIter1) {
            if (((n / prime) & 1) == 1) {
                primeList[count++] = prime;
            }
        }

        Xint prod = sieve.getPrimorial(n / 2 + 1, n);
        return prod.multiply(primeList, count);
    }
    private static final int[] smallOddSwing = {1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231, 3003, 429,
        6435, 6435, 109395, 12155, 230945, 46189, 969969, 88179,
        2028117, 676039, 16900975, 1300075, 35102025, 5014575,
        145422675, 9694845, 300540195, 300540195};
} // endOfFactorialPrimeSwingLuschny
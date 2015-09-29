// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.Xmath;
import de.luschny.math.arithmetic.Xint;
import de.luschny.math.primes.IPrimeIteration;
import de.luschny.math.primes.IPrimeSieve;
import de.luschny.math.primes.PrimeSieve;

public class FactorialPrimeBorwein implements IFactorialFunction {

    private int[] primeList;
    private int[] multiList;

    public FactorialPrimeBorwein() {
    }

    @Override
    public String getName() {
        return "PrimeBorwein      ";
    }

    @Override
    public Xint factorial(int n) {
        // For very small n the 'NaiveFactorial' is ok.
        if (n < 20) {
            return Xmath.smallFactorial(n);
        }

        int lgN = 31 - Integer.numberOfLeadingZeros(n);
        int piN = 2 + (15 * n) / (8 * (lgN - 1));

        primeList = new int[piN];
        multiList = new int[piN];

        int len = primeFactors(n);
        return repeatedSquare(len, 1).shiftLeft(n - Integer.bitCount(n));
    }

    private Xint repeatedSquare(int len, int k) {
        if (len == 0) {
            return Xint.ONE;
        }

        // multiList and primeList initialized in function 'factorial'!
        int i = 0, mult = multiList[0];

        while (mult > 1) {
            if ((mult & 1) == 1) // is mult odd ?
            {
                primeList[len++] = primeList[i];
            }

            multiList[i++] = mult / 2;
            mult = multiList[i];
        }
        return Xint.product(primeList, i, len - i).toPowerOf(k).multiply(repeatedSquare(i, 2 * k));
    }

    private int primeFactors(int n) {
        IPrimeSieve sieve = new PrimeSieve(n);
        IPrimeIteration pIter = sieve.getIteration(3, n);

        int maxBound = n / 2, count = 0;

        for (int prime : pIter) {
            int m = prime > maxBound ? 1 : 0;

            if (prime <= maxBound) {
                int q = n;
                while (q >= prime) {
                    m += q /= prime;
                }
            }

            // multiList and primeList initialized in function 'factorial'!
            primeList[count] = prime;
            multiList[count++] = m;
        }
        return count;
    }
} // endOfFactorialPrimeBorwein

// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.Xmath;
import de.luschny.math.arithmetic.Xint;
import de.luschny.math.primes.IPrimeIteration;
import de.luschny.math.primes.PrimeSieve;

public class FactorialPrimeLeenstra implements IFactorialFunction {

    public FactorialPrimeLeenstra() {
    }

    @Override
    public String getName() {
        return "PrimeLeenstra     ";
    }

    @Override
    public Xint factorial(int n) {
        // For very small n the 'NaiveFactorial' is OK.
        if (n < 20) {
            return Xmath.Factorial(n);
        }

        int rootN = (int) Math.floor(Math.sqrt(n));
        int log2N = 31 - Integer.numberOfLeadingZeros(n);
        Xint[] expBit = new Xint[log2N + 1];

        for (int j = 0; j < expBit.length; j++) {
            expBit[j] = Xint.ONE;
        }

        PrimeSieve sieve = new PrimeSieve(n);
        IPrimeIteration pIter = sieve.getIteration(3, rootN);

        for (int prime : pIter) {
            int k = 0, m = 0, q = n;

            do {
                m += q /= prime;

            } while (q >= 1);

            while (m > 0) {
                if ((m & 1) == 1) {
                    expBit[k] = expBit[k].multiply(prime);
                }
                m = m / 2;
                k++;
            }
        }

        int j = 2, low = n, high;

        while (low != rootN) {
            high = low;
            low = n / j++;

            if (low < rootN) {
                low = rootN;
            }

            Xint primorial = sieve.getPrimorial(low + 1, high);

            if (!primorial.isONE()) {
                int k = 0, m = j - 2;

                while (m > 0) {
                    if ((m & 1) == 1) {
                        expBit[k] = expBit[k].multiply(primorial);
                    }
                    m = m / 2;
                    k++;
                }
            }
        }

        Xint fact = expBit[log2N];
        for (int i = log2N - 1; i >= 0; --i) {
            fact = fact.square().multiply(expBit[i]);
        }

        return fact.shiftLeft(n - Integer.bitCount(n));
    }
} // endOfFactorialPrimeLeenstra

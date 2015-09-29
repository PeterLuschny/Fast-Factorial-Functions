// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.Xmath;
import de.luschny.math.arithmetic.Xint;
import de.luschny.math.primes.IPrimeIteration;
import de.luschny.math.primes.PrimeSieve;

import java.util.HashMap;

public class FactorialPrimeSwingCache implements IFactorialFunction {

    private PrimeSieve sieve;
    private int[] primeList;
    private HashMap<Integer, CachedPrimorial> cache;

    public FactorialPrimeSwingCache() {
    }

    @Override
    public String getName() {
        return "PrimeSwingCache   ";
    }

    @Override
    public Xint factorial(int n) {
        // For very small n the 'NaiveFactorial' is OK.
        if (n < 20) {
            return Xmath.smallFactorial(n);
        }

        cache = new HashMap<>();
        sieve = new PrimeSieve(n);
        int piN = sieve.getIteration().getNumberOfPrimes();
        primeList = new int[piN];

        return recFactorial(n).shiftLeft(n - Integer.bitCount(n));
    }

    private Xint recFactorial(int n) {
        if (n < 2) {
            return Xint.ONE;
        }
        return swing(n).multiply(recFactorial(n / 2).square());
    }

    private Xint swing(int n) {
        if (n < 33) {
            return Xint.valueOf(smallOddSwing[n]);
        }

        int rootN = (int) Math.floor(Math.sqrt(n));
        int count = 0, j = 1, low, high;

        Xint prod = Xint.ONE;

        while (true) {
            high = n / j++;
            low = n / j++;

            if (low < rootN) {
                low = rootN;
            }
            if (high - low < 32) {
                break;
            }

            Xint primorial = getPrimorial(low + 1, high);

            if (!primorial.isONE()) {
                prod = prod.multiply(primorial);
            }
        }

        // multiList and primeList initialized in function 'factorial'!
        IPrimeIteration pIter = sieve.getIteration(3, high);

        for (int prime : pIter) {
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

        return prod.multiply(primeList, count);
    }

    private Xint getPrimorial(int low, int high) {
        // System.out.print("Primorial [" + low + "," + high + "] " );
        Xint primorial;
        CachedPrimorial cachedPrimorial = cache.get(low);

        if (null != cachedPrimorial) {
            // System.out.println(" <- from Cache");
            int low1 = cachedPrimorial.high + 1;
            Xint p = sieve.getPrimorial(low1, high);
            primorial = p.multiply(cachedPrimorial.value);
        } else {
            // System.out.println(" <- from Sieve");
            primorial = sieve.getPrimorial(low, high);
        }

        cache.put(low, new CachedPrimorial(high, primorial));

        return primorial;
    }
    private static final int[] smallOddSwing = {1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231, 3003, 429, 6435,
        6435, 109395, 12155, 230945, 46189, 969969, 88179, 2028117, 676039,
        16900975, 1300075, 35102025, 5014575, 145422675, 9694845,
        300540195, 300540195};
}

class CachedPrimorial {

    public final int high;
    public final Xint value;

    CachedPrimorial(int highBound, Xint val) {
        high = highBound;
        value = val;
    }
}
// endOfFactorialPrimeSwingCache
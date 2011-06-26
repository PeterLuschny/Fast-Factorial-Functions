// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.Xmath;
import de.luschny.math.arithmetic.Xint;
import de.luschny.math.primes.IPrimeIteration;
import de.luschny.math.primes.PrimeSieve;

public class FactorialPrimeVardi implements IFactorialFunction {

    private PrimeSieve sieve;

    public FactorialPrimeVardi() {
    }

    @Override
    public final String getName() {
        return "PrimeVardi        ";
    }

    @Override
    public Xint factorial(int n) {
        // For very small n the 'NaiveFactorial' is ok.
        if (n < 20) {
            return Xmath.Factorial(n);
        }

        sieve = new PrimeSieve(n);

        return recFactorial(n);
    }

    private Xint recFactorial(int n) {
        if (n < 2) {
            return Xint.ONE;
        }

        if ((n & 1) == 1) {
            return recFactorial(n - 1).multiply(n);
        }

        return middleBinomial(n).multiply(recFactorial(n / 2).square());
    }

    private Xint middleBinomial(int n) // assuming n = 2k
    {
        if (n < 50) {
            return Xint.valueOf(binom[n / 2]);
        }

        int k = n / 2, pc = 0, pp = 0, e;
        int rootN = (int) Math.floor(Math.sqrt(n));

        Xint bigPrimes = sieve.getPrimorial(k + 1, n);
        Xint smallPrimes = sieve.getPrimorial(k / 2 + 1, n / 3);

        IPrimeIteration pIter = sieve.getIteration(rootN + 1, n / 5);
        int[] primeList = new int[pIter.getNumberOfPrimes()];

        for (int prime : pIter) {
            if ((n / prime & 1) == 1) // if n/prime is odd...
            {
                primeList[pc++] = prime;
            }
        }
        Xint prodPrimes = Xint.product(primeList, 0, pc);

        pIter = sieve.getIteration(1, rootN);
        Xint[] primePowerList = new Xint[pIter.getNumberOfPrimes()];

        for (int prime : pIter) {
            if ((e = expSum(prime, n)) > 0) {
                primePowerList[pp++] = Xint.valueOf(prime).toPowerOf(e);
            }
        }
        Xint powerPrimes = Xint.product(primePowerList, 0, pp);

        return bigPrimes.multiply(smallPrimes).multiply(prodPrimes).multiply(powerPrimes);
    }

    private int expSum(int p, int n) {
        int exp = 0, q = n / p;

        while (0 < q) {
            exp += q & 1;
            q /= p;
        }

        return exp;
    }
    private static long[] binom = {1, 2, 6, 20, 70, 252, 924, 3432, 12870, 48620, 184756, 705432, 2704156,
        10400600, 40116600, 155117520, 601080390, 2333606220L, 9075135300L,
        35345263800L, 137846528820L, 538257874440L, 2104098963720L, 8233430727600L,
        32247603683100L};
} // endOfFactorialPrimeVardi

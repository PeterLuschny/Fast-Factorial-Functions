// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.Xmath;
import de.luschny.math.arithmetic.Xint;
import de.luschny.math.primes.IPrimeIteration;
import de.luschny.math.primes.PrimeSieve;

public class FactorialPrimeSwingList implements IFactorialFunction {

    private int[][] listPrime;
    private int[] listLength;
    private int[] tower;
    private int[] bound;

    public FactorialPrimeSwingList() {
    }

    @Override
    public final String getName() {
        return "PrimeSwingList    ";
    }

    @Override
    public Xint factorial(int n) {
        // For very small n the 'NaiveFactorial' is OK.
        if (n < 20) {
            return Xmath.Factorial(n);
        }

        // log2n = floor(log2(n));
        int log2n = 31 - Integer.numberOfLeadingZeros(n);
        int j = log2n, hN = n;

        this.listPrime = new int[log2n][];
        listLength = new int[log2n];
        bound = new int[log2n];
        tower = new int[log2n + 1];

        while (true) {
            tower[j] = hN;
            if (hN == 1) {
                break;
            }
            bound[--j] = hN / 3;
            listPrime[j] = new int[omegaSwingHighBound(hN)];
            hN /= 2;
        }
        tower[0] = 2;

        primeFactors(n);
        return iterQuad().shiftLeft(n - Integer.bitCount(n));
    }

    private Xint iterQuad() {
        int init = listLength[0] == 0 ? 1 : 3;
        Xint fact = Xint.valueOf(init);

        int listLen = listPrime.length;

        for (int i = 1; i < listLen; i++) {
            fact = (fact.square()).multiply(listPrime[i], listLength[i]);
        }
        return fact;
    }

    private void primeFactors(int n) {
        int maxBound = n / 3;
        int lastList = listPrime.length - 1;
        int start = tower[1] == 2 ? 1 : 0;

        PrimeSieve sieve = new PrimeSieve(n);

        for (int list = start; list < listPrime.length; list++) {
            IPrimeIteration pIter = sieve.getIteration(tower[list] + 1, tower[list + 1]);

            for (int prime : pIter) {
                listPrime[list][listLength[list]++] = prime;
                if (prime > maxBound) {
                    continue;
                }

                int np = n;
                do {
                    int k = lastList;
                    int q = np /= prime;

                    do {
                        if ((q & 1) == 1) {
                            listPrime[k][listLength[k]++] = prime;
                        }
                    } while (((q /= 2) > 0) && (prime <= bound[--k]));

                } while (prime <= np);
            }
        }
    }

    private int omegaSwingHighBound(int n) {
        return n < 4 ? 6 : (int) (2.0 * (Math.floor(Math.sqrt(n) + n / (Math.log(n) * 1.4426950408889634 - 1.0))));
    }
} // endOfFactorialPrimeSwingList

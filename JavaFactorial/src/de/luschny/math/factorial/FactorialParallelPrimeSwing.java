// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.Xmath;
import de.luschny.math.arithmetic.Xint;
import de.luschny.math.primes.IPrimeIteration;
import de.luschny.math.primes.PrimeSieve;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.*;

public class FactorialParallelPrimeSwing implements IFactorialFunction {

    // Same algorithm as FactorialPrimeSwing
    // but computing swing(n) concurrently
    public FactorialParallelPrimeSwing() {
    }

    @Override
    public String getName() {
        return "ParallelPrimeSwing";
    }
    private List<Future<Xint>> swings;
    private int taskCounter;

    @Override
    public Xint factorial(int n) {
        // For very small n the 'NaiveFactorial' is OK.
        if (n < 20) {
            return Xmath.smallFactorial(n);
        }

        int proc = Runtime.getRuntime().availableProcessors();
        ExecutorService poolExe = Executors.newFixedThreadPool(proc);

        PrimeSieve sieve = new PrimeSieve(n);

        int log2n = 31 - Integer.numberOfLeadingZeros(n);
        ArrayList<Callable<Xint>> swingTasks = new ArrayList<>(log2n);

        taskCounter = 0;
        int N = n;

        // -- It is more efficient to add the big swings
        // -- first and the small ones later!
        while (N > 32) {
            swingTasks.add(new Swing(sieve, N));
            N >>= 1;
            taskCounter++;
        }

        Xint fact = null;
        try {
            swings = poolExe.invokeAll(swingTasks);
            fact = recFactorial(n).shiftLeft(n - Integer.bitCount(n));
        } catch (Throwable ignored) {
        }

        poolExe.shutdownNow();
        return fact;
    }

    private Xint recFactorial(int n) throws Throwable {
        if (n < 2) {
            return Xint.ONE;
        }

        Xint recFact = recFactorial(n / 2).square();
        Xint swing;

        if (n <= 32) {
            swing = Xint.valueOf(smallOddSwing[n]);
        } else {
            // swings is initialized in function 'factorial'.
            swing = swings.get(--taskCounter).get();
        }

        return recFact.multiply(swing);
    }
    private static final int[] smallOddSwing = {1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231, 3003, 429,
        6435, 6435, 109395, 12155, 230945, 46189, 969969, 88179,
        2028117, 676039, 16900975, 1300075, 35102025, 5014575,
        145422675, 9694845, 300540195, 300540195};

    // -- Concurrent execution of this Class should not fail.
    static class Swing implements Callable<Xint> {

        private final PrimeSieve Sieve;
        private final int N;

        public Swing(PrimeSieve sieve, int n) {
            Sieve = sieve;
            N = n;
        }

        // -- Returns swing(n)
        @Override
        public Xint call() throws Exception {
            FutureTask<Xint> Primorial = new FutureTask<>(() -> Sieve.getPrimorial(N / 2 + 1, N));

            new Thread(Primorial).start();
            Xint primeProduct = lowSwing();
            return primeProduct.multiply(Primorial.get());
        }

        private Xint lowSwing() {
            int sqrtN = (int) Math.floor(Math.sqrt(N));
            IPrimeIteration pIter0 = Sieve.getIteration(3, sqrtN);
            IPrimeIteration pIter1 = Sieve.getIteration(sqrtN + 1, N / 3);

            int piN = pIter0.getNumberOfPrimes() + pIter1.getNumberOfPrimes();
            final int[] primeList = new int[piN];
            int count = 0;

            for (int prime : pIter0) {
                int q = N, p = 1;

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
                if (((N / prime) & 1) == 1) {
                    primeList[count++] = prime;
                }
            }

            return Xint.product(primeList, 0, count);

        } // endOfLowSwing
    } // endOfSwing
} // endOfFactorialPrimeSwingConcurrent


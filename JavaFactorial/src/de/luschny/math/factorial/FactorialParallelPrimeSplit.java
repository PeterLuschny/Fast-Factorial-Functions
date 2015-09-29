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

public class FactorialParallelPrimeSplit implements IFactorialFunction {

    public FactorialParallelPrimeSplit() {
    }

    @Override
    public String getName() {
        return "ParallelPrimeSplit";
    }

    @Override
    public Xint factorial(int n) {
        // For very small n the 'NaiveFactorial' is OK.
        if (n < 20) {
            return Xmath.smallFactorial(n);
        }

        PrimeSieve sieve = new PrimeSieve(n);

        int proc = Runtime.getRuntime().availableProcessors();
        int log2n = 31 - Integer.numberOfLeadingZeros(n);

        ExecutorService exe = Executors.newFixedThreadPool(proc);
        ArrayList<Callable<Xint>> swingTasks = new ArrayList<>(log2n);

        Xint r = Xint.ONE, p = Xint.ONE;
        Xint rl = Xint.ONE, t;

        int h = 0, shift = 0;

        while (h != n) {
            shift += h;
            h = n >> log2n--;
            if (h > 2) {
                swingTasks.add(new Swing(sieve, h));
            }
        }

        try {
            List<Future<Xint>> swings = exe.invokeAll(swingTasks);
            int taskCounter = swings.size();

            for (Future<Xint> swing : swings) {
                t = rl.multiply(swing.get());
                p = p.multiply(t);
                rl = r;
                r = r.multiply(p);
            }
        } catch (Throwable ignored) {
        }

        exe.shutdownNow();
        return r.shiftLeft(shift);
    }
    
    private static final int[] smallOddSwing = {1, 1, 1, 3, 3, 15, 5, 35, 35, 315,
        63, 693, 231, 3003, 429, 6435, 6435, 109395, 12155, 230945, 46189, 
        969969, 88179, 2028117, 676039, 16900975, 1300075, 35102025, 5014575, 
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
            if (N < 33) {
                return Xint.valueOf(smallOddSwing[N]);
            }

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
} // endOfFactorial

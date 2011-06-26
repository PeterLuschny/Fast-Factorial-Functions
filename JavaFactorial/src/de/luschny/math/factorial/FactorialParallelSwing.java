// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import java.util.ArrayList;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

import de.luschny.math.arithmetic.Xint;

public class FactorialParallelSwing implements IFactorialFunction {

    private static int SMALLSWING = 33;
    private static int SMALLFACT = 17;
    private ExecutorService exe;
    private Xint oddFactNdiv4, oddFactNdiv2;
    private ArrayList<Future<Xint>> tasks;
    private int taskCounter;

    public FactorialParallelSwing() {
        int proc = Runtime.getRuntime().availableProcessors();
        exe = Executors.newFixedThreadPool(proc);
    }

    @Override
    public final String getName() {
        return "ParallelSwing     ";
    }

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        if (n < SMALLFACT) {
            return Xint.valueOf(smallOddFactorial[n]).shiftLeft(n - Integer.bitCount(n));
        }

        oddFactNdiv4 = Xint.ONE;
        oddFactNdiv2 = Xint.ONE;

        // -- log2n = floor(log2(n));
        int log2n = 31 - Integer.numberOfLeadingZeros(n);
        tasks = new ArrayList<Future<Xint>>(log2n);

        for (int swn = n; swn >= SMALLSWING; swn >>= 1) {
            tasks.add(runNewOddSwingTask(swn));
            taskCounter++;
        }

        return oddFactorial(n).shiftLeft(n - Integer.bitCount(n));
    }

    private Xint oddFactorial(int n) {
        Xint oddFact, oddSwing;

        if (n < SMALLFACT) {
            return Xint.valueOf(smallOddFactorial[n]);
        }

        Xint sqrOddFact = oddFactorial(n / 2).square();

        if (n < SMALLSWING) {
            oddSwing = Xint.valueOf(smallOddSwing[n]);
        } else {
            int ndiv4 = n / 4;
            Xint oddFactNd4 = ndiv4 >= SMALLFACT ? 
                    oddFactNdiv4 : Xint.valueOf(smallOddFactorial[ndiv4]);
            oddSwing = getTaskResult(--taskCounter).divide(oddFactNd4);
        }

        oddFact = sqrOddFact.multiply(oddSwing);

        oddFactNdiv4 = oddFactNdiv2;
        oddFactNdiv2 = oddFact;
        return oddFact;
    }

    private Future<Xint> runNewOddSwingTask(final int n) {
        return exe.submit(new Callable<Xint>() {

            private Xint product(int m, int len) {
                if (len == 1) {
                    return Xint.valueOf(m);
                }
                if (len == 2) {
                    return Xint.valueOf((long) m * (m - 2));
                }

                return product(m - (len >> 1) * 2, len - (len >> 1)).
                        multiply(product(m, len >> 1));
            }

            @Override
            public Xint call() {
                int len = (n - 1) / 4;
                if ((n % 4) != 2) {
                    len++;
                }

                // -- if type(n,odd) then high=n; else high=n-1;
                int high = n - ((n + 1) & 1);

                return product(high, len);
            }
        });
    }

    private Xint getTaskResult(final int n) {
        try {
            return (tasks.get(n)).get();
        } catch (Exception ex) {
            return Xint.ZERO;
        }
    }
    private static int[] smallOddSwing = {1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 
        693, 231, 3003, 429, 6435, 6435, 109395, 12155, 230945, 46189, 969969, 
        88179, 2028117, 676039, 16900975, 1300075, 35102025, 5014575, 145422675,
        9694845, 300540195, 300540195};
    private static int[] smallOddFactorial = {1, 1, 1, 3, 3, 15, 45, 315, 315, 
        2835, 14175, 155925, 467775, 6081075, 42567525, 638512875, 638512875};
} // endOfFactorialParallelSwing

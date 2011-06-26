// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.arithmetic.Xint;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class FactorialParallelSplit implements IFactorialFunction {
    // Same algorithm as Split
    // but computing products concurrently.

    public FactorialParallelSplit() {
    }

    @Override
    public String getName() {
        return "ParallelSplit     ";
    }

    @Override
    public Xint factorial(int n) {
        if (n < 0) {
            throw new ArithmeticException("Factorial: n has to be >= 0, but was " + n);
        }

        if (n < 2) {
            return Xint.ONE;
        }

        // -- log2n = floor(log2(n));
        int log2n = 31 - Integer.numberOfLeadingZeros(n);
        int proc = Runtime.getRuntime().availableProcessors();

        ExecutorService exe = Executors.newFixedThreadPool(proc);
        ArrayList<Callable<Xint>> tasks = new ArrayList<Callable<Xint>>(log2n);

        int high = n, low = n >>> 1, shift = low, taskCounter = 0;

        // -- It is more efficient to add the big intervals
        // -- first and the small ones later!
        while ((low + 1) < high) {
            tasks.add(new Product(low + 1, high));
            high = low;
            low >>= 1;
            shift += low;
            taskCounter++;
        }

        Xint p = Xint.ONE, r = Xint.ZERO;

        try {
            List<Future<Xint>> products = exe.invokeAll(tasks);

            Future<Xint> R = exe.submit(new Callable<Xint>() {

                @Override
                public Xint call() {
                    return Xint.ONE;
                }
            });

            while (--taskCounter >= 0) {
                p = p.multiply(products.get(taskCounter).get());
                R = exe.submit(new Multiply(R.get(), p));
            }

            r = R.get();
        } catch (Throwable e) {
        }

        exe.shutdownNow();
        return r.shiftLeft(shift);
    }

    final class Multiply implements Callable<Xint> {

        private final Xint a, b;

        public Multiply(Xint a, Xint b) {
            this.a = a;
            this.b = b;
        }

        @Override
        public Xint call() {
            return a.multiply(b);
        }
    }
} // endOfFactorialSplitRecursive

final class Product implements Callable<Xint> {

    private final int n, m;

    public Product(int n, int m) {
        this.n = n;
        this.m = m;
    }

    @Override
    public Xint call() {
        return product(n, m);
    }

    public static Xint product(int n, int m) {
        n = n | 1; // Round n up to the next odd number
        m = (m - 1) | 1; // Round m down to the next odd number

        if (m == n) {
            return Xint.valueOf(m);
        }
        if (m == (n + 2)) {
            return Xint.valueOf((long) n * m);
        }

        int k = (n + m) >>> 1;
        return product(n, k).multiply(product(k + 1, m));
    }
}

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

import java.util.concurrent.Callable;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class FactorialXPrimeSchoenhage implements IFactorialFunction {

    private int[] primeList;
    private int[] multiList;
    static ExecutorService exec = Executors.newCachedThreadPool();

    public FactorialXPrimeSchoenhage() {
    }

    @Override
    public String getName() {
        return "XPrimeSchoenhage  ";
    }

    @Override
    public Xint factorial(int n) {
        // For very small n the 'NaiveFactorial' is ok.
        if (n < 20) {
            return Xmath.Factorial(n);
        }

        int log2n = 31 - Integer.numberOfLeadingZeros(n);
        int piN = 2 + (15 * n) / (8 * (log2n - 1));

        primeList = new int[piN];
        multiList = new int[piN];

        int len = primeFactors(n);
        return nestedSquare(len).shiftLeft(n - Integer.bitCount(n));
    }

    private Xint nestedSquare(int len) {
        
        if (len == 0) {
            return Xint.ONE;
        }

        int i = 0, mult = multiList[0];

        while (mult > 1) {
            if ((mult & 1) == 1) // is mult odd ?
            {
                primeList[len++] = primeList[i];
            }

            multiList[i++] = mult / 2;
            mult = multiList[i];
        }

        if (len <= i) {
            return nestedSquare(i).square();
        }

        Xint a = Xint.product(primeList, i, len - i);
        Xint b = nestedSquare(i).square();
        
        // return Xint.product(primeList, i, len - i).multiply(nestedSquare(i).square());
        
        Future<Xint> L = exec.submit(new XProduct(primeList, i, len - i));
        Future<Xint> R = exec.submit(new Square(nestedSquare(i)));
                
        Xint ret = Xint.ZERO;
        try {
            Xint A = L.get();
            Xint B = R.get();
            ret = A.multiply(B); 
             // ret = (L.get()).multiply(R.get());
        } catch (InterruptedException ex) {
           // Logger.getLogger(FactorialXPrimeSchoenhage.class.getName()).log(Level.SEVERE, null, ex);
        } catch (ExecutionException ex) {
           // Logger.getLogger(FactorialXPrimeSchoenhage.class.getName()).log(Level.SEVERE, null, ex);
        }
        return ret;
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

            primeList[count] = prime;
            multiList[count++] = m;
        }
        return count;
    }
}

final class Square implements Callable<Xint> {

    private final Xint a;

    public Square(Xint a) {
        this.a = a;
    }

    @Override
    public Xint call() {
        return a.square();
    }
}

final class XProduct implements Callable<Xint> {

    private final int n, m;
    private final int[] s;

    public XProduct(int[] s, int n, int m) {
        this.s = s;
        this.n = n;
        this.m = m;
    }

    @Override
    public Xint call() {
        // return product(s, n, m);
        return Xint.product(s, n, m);
    }

    private static Xint Yproduct(int[] s, int n, int m) {
        if (n > m) {
            return Xint.ONE;
        }
        if (n == m) {
            return Xint.valueOf(s[n]);
        }

        int k = (n + m) >> 1;
        return Yproduct(s, n, k).multiply(Yproduct(s, k + 1, m));
    }
}

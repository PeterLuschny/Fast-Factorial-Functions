// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.factorial;

import de.luschny.math.primes.IPrimeIteration;
import de.luschny.math.primes.IPrimeSieve;
import de.luschny.math.primes.PrimeSieve;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;

public class FactorialFactors {

    public FactorialFactors() {
    }
    private int[] primeList;
    private int[] multiList;
    private int n, card;

    public FactorialFactors(int n) {
        this.n = n;
        card = primeFactors(n);
    }

    public void saveToFile(String fileName) {
        try {
            PrintWriter primeReport = new PrintWriter(new BufferedWriter(new FileWriter(fileName)));

            writeFactors(primeReport);
            primeReport.close();
        } catch (IOException e) {
            System.err.println(e.toString());
        }
    }

    public void writeFactors(PrintWriter file) {
        file.println("The prime factors of " + this.n + "! ");

        int sum = n - Integer.bitCount(n);
        file.print("2^" + sum);

        for (int p = 0; p < this.card; p++) {
            int f = primeList[p], m = multiList[p];
            sum += m;

            if (m > 1) {
                file.print("*" + f + "^" + m);
            } else {
                file.print("*" + f);
            }
        }

        file.println();
        file.println("Number of different factors: " + this.card);
        file.println("Number of all factors: " + sum);
    }

    private int primeFactors(int k) {
        IPrimeSieve sieve = new PrimeSieve(k);
        IPrimeIteration pIter = sieve.getIteration(2, k);

        int piN = pIter.getNumberOfPrimes();

        primeList = new int[piN];
        multiList = new int[piN];

        int maxBound = k / 2, count = 0;

        for (int prime : pIter) {
            int m = prime > maxBound ? 1 : 0;

            if (prime <= maxBound) {
                int q = k;
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
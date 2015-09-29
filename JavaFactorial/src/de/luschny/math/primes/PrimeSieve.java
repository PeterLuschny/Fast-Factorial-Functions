// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.primes;

import de.luschny.math.arithmetic.Xint;
import de.luschny.math.util.PositiveRange;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.Iterator;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.logging.Logger;

/**
 * Sieve.
 * 
 * @author Peter Luschny
 * @version 2004-10-20
 */
public class PrimeSieve implements IPrimeSieve {

    private final int[] primes;
    private final PositiveRange sieveRange;
    private final PositiveRange primeRange;
    private static Logger logger;

    /**
     * Constructs a prime sieve for the integer range [1,n].
     * 
     * @param n
     *            The upper bound of the range to be sieved.
     */
    public PrimeSieve(int n) {
        primes = new int[getPiHighBound(n)];
        // Note: This forces n>=1
        sieveRange = new PositiveRange(1, n);

        int numberOfPrimes = makePrimeList(n);
        primeRange = new PositiveRange(1, numberOfPrimes);
    }

    /**
     * Enables logging of this component if the logger object is not null.
     * 
     * @param log
     *            The Logger used to log messages for this component.
     */
    @Override
    public void enableLogging(Logger log) {
        logger = log;
    }

    /**
     * Prime number sieve, Eratosthenes (276-194 b. t.) This implementation
     * considers only multiples of primes greater than 3, so the smallest value
     * has to be mapped to 5.
     * <p/>
     * Note: There is no multiplication operation in this function.
     * 
     * @param composite
     *        After execution of the function this boolean array includes
     *        all composite numbers in [5,n] disregarding multiples of 2 and 3.
     */
    private static void sieveOfEratosthenes(final boolean[] composite) {
        int d1 = 8;
        int d2 = 8;
        int p1 = 3;
        int p2 = 7;
        int s1 = 7;
        int s2 = 3;
        int n = 0;
        int len = composite.length;
        boolean toggle = false;

        while (s1 < len) // -- scan sieve
        {
            if (!composite[n++]) // -- if a prime is found
            { // -- cancel its multiples
                int inc = p1 + p2;

                for (int k = s1; k < len; k += inc) {
                    composite[k] = true;
                }

                for (int k = s1 + s2; k < len; k += inc) {
                    composite[k] = true;
                }
            }

            if (toggle = !toggle) // Never mind, it's ok.
            {
                s1 += d2;
                d1 += 16;
                p1 += 2;
                p2 += 2;
                s2 = p2;
            } else {
                s1 += d1;
                d2 += 8;
                p1 += 2;
                p2 += 6;
                s2 = p1;
            }
        }
    }

    /**
     * Transforms the sieve of Eratosthenes into the sequence of prime numbers.
     * 
     * @param n
     *            Upper bound of the sieve.
     * @return Number of primes found.
     */
    private int makePrimeList(int n) {
        boolean[] composite = new boolean[n / 3];

        sieveOfEratosthenes(composite);

        int[] prime = this.primes; // -- on stack for eff.
        boolean toggle = false;
        int p = 5, i = 0, j = 2;

        prime[0] = 2;
        prime[1] = 3;

        while (p <= n) {
            if (!composite[i++]) {
                prime[j++] = p;
            }
            // -- never mind, it's ok.
            p += (toggle = !toggle) ? 2 : 4;
        }

        return j; // number of primes
    }

    /**
     * Get a high bound for pi(n), the number of primes less or equal n.
     * 
     * @param n
     *            Bound of the primes.
     * @return A simple estimate of the number of primes <= n.
     */
    private static int getPiHighBound(long n) {
        if (n < 17) {
            return 6;
        }
        return (int) Math.floor(n / (Math.log(n) - 1.5));
    }

    /**
     * Get the n-th prime number.
     * 
     * @param n
     *            The index of the prime number.
     * @return The n-th prime number.
     */
    @Override
    public int getNthPrime(int n) {
        primeRange.containsOrFail(n);
        return primes[n - 1];
    }

    /**
     * Checks if a given number is prime.
     * 
     * @param cand
     *            The number to be checked.
     * @return True if and only if the given number is prime.
     */
    @Override
    public boolean isPrime(int cand) {
        // The candidate is interpreted as an one point interval!
        int primeMax = primeRange.getMax();
        int start = PrimeIteration.indexOf(primes, cand - 1, 0, primeMax);
        int end = primes[start] == cand ? start + 1 : start;
        return start < end;
    }

    /**
     * The default iteration of the full sieve.
     * 
     * @return An iteration over all prime numbers found by the sieve.
     */
    @Override
    public IPrimeIteration getIteration() {
        return new PrimeIteration(this);
    }

    /**
     * Gives the iteration of the prime numbers in the given interval.
     * 
     * @param low
     *            Lower bound of the iteration.
     * @param high
     *            Upper bound of the iteration.
     * @return An iteration of the prime numbers which are in the interval [low,
     *         high].
     */
    @Override
    public IPrimeIteration getIteration(int low, int high) {
        return new PrimeIteration(this, new PositiveRange(low, high));
    }

    /**
     * Gives the iteration of the prime numbers in the given range.
     * 
     * @param range
     *            The range of the iteration.
     * @return The prime number iteration.
     */
    @Override
    public IPrimeIteration getIteration(PositiveRange range) {
        return new PrimeIteration(this, range);
    }

    /**
     * Gives the product of the prime numbers in the given interval.
     * 
     * @param low
     *            Lower bound of the iteration.
     * @param high
     *            Upper bound of the iteration.
     * @return The product of the prime numbers which are in the interval [low,
     *         high].
     */
    public Xint getPrimorial(int low, int high) {
        return new PrimeIteration(this, new PositiveRange(low, high)).primorial();
    }

    /**
     * Gives the product of the prime numbers in the given range.
     * 
     * @param range
     *            The range of the iteration.
     * @return The product of the prime numbers in the range.
     */
    public Xint getPrimorial(PositiveRange range) {
        return new PrimeIteration(this, range).primorial();
    }

    // ----------------------- inner class --------------------------
    /**
     * PrimeIteration.
     * 
     * @author Peter Luschny
     * @version 2004-09-12
     */
    private static class PrimeIteration implements IPrimeIteration {

        private final PrimeSieve sieve;
        private final PositiveRange sieveRange;
        private final PositiveRange primeRange;
        private final int start;
        private final int end;
        private final AtomicInteger state;
        private int current;

        /**
         * Constructs the iteration for the passed sieve.
         * 
         * @param sieve
         *            The sieve which is to be enumerated.
         */
        PrimeIteration(final PrimeSieve sieve) {
            this.sieve = sieve;
            current = start = 0;
            end = sieve.primeRange.getMax();
            sieveRange = sieve.sieveRange;
            primeRange = sieve.primeRange;
            state = new AtomicInteger(0);
        }

        /**
         * Constructs an iteration over a subrange of the range of the sieve.
         * 
         * @param sieve
         *            Prime number sieve to be used.
         * @param sieveRange
         *            Range of iteration.
         */
        PrimeIteration(final PrimeSieve sieve, final PositiveRange sieveRange) {
            this.sieve = sieve;
            this.sieveRange = sieveRange;

            sieve.sieveRange.containsOrFail(sieveRange);

            int sieveMax = sieve.primeRange.getMax();
            int primeMin = indexOf(sieve.primes, sieveRange.getMin() - 1, 0, sieveMax - 1);
            int primeMax = indexOf(sieve.primes, sieveRange.getMax(), primeMin, sieveMax);

            if (primeMin == primeMax) // there are no primes in this range
            {
                start = end = 0;
                primeRange = new PositiveRange(0, 0);
            } else {
                start = primeMin;
                end = primeMax;
                primeRange = new PositiveRange(primeMin + 1, primeMax);
            }

            current = primeMin;
            state = new AtomicInteger(0);
        }

        /**
         * Provides an iterator over the current prime number list. This
         * iterator is thread save.
         * 
         * @return An iterator over the current prime number list.
         */
        @Override
        public Iterator<Integer> iterator() {
            PrimeIteration result = this;

            if (0 != state.getAndIncrement()) {
                result = new PrimeIteration(sieve, sieveRange);
            }

            result.current = result.start;

            return result;
        }

        /**
         * Returns The next prime number in the iteration.
         * 
         * @return The next prime number in the iteration.
         */
        @Override
        public Integer next() {
            return sieve.primes[current++];
        }

        /**
         * Checks the current status of the finite iteration.
         * 
         * @return True iff there are more prime numbers to be enumerated.
         */
        @Override
        public boolean hasNext() {
            return current < end;
        }

        /**
         * The (optional operation) to remove from the underlying collection the
         * last element returned by the iterator is not supported.
         * 
         * @throws UnsupportedOperationException
         */
        @Override
        public void remove() {
            throw new UnsupportedOperationException();
        }

        /**
         * Identifies the index of a prime number. Uses a (modified!) binary
         * search.
         * 
         * @param data
         *            List of prime numbers.
         * @param value
         *            The prime number given.
         * @param low
         *            Lower bound for the index.
         * @param high
         *            Upper bound for the index.
         * @return The index of the prime number.
         */
        static int indexOf(final int[] data, int value, int low, int high) {
            while (low < high) {
                // int mid = low + ((high - low) / 2);
                // Probably faster, and arguably as clear is:
                // int mid = (low + high) >>> 1;
                // In C and C++ (where you don't have
                // the >>> operator), you can do this:
                // mid = ((unsigned) (low + high)) >> 1;

                int mid = (low + high) >>> 1;

                if (data[mid] < value) {
                    low = mid + 1;
                } else {
                    high = mid;
                }
            }

            if (low >= data.length) {
                return low;
            }

            if (data[low] == value) {
                low++;
            }

            return low;
        }

        /**
         * Computes the number of primes in the iteration range.
         * 
         * @return Cardinality of primes in iteration range.
         */
        @Override
        public int getNumberOfPrimes() {
            if (0 == primeRange.getMax()) // If primeRange is empty...
            {
                return 0;
            }

            return primeRange.size();
        }

        /**
         * Computes the density of primes in the iteration.
         * 
         * @return Ratio of the number of primes relative to the number of
         *         integers in this iteration.
         */
        @Override
        public double getPrimeDensity() {
            // Note: By construction sieveRange.size is always != 0.
            return (double) primeRange.size() / (double) sieveRange.size();
        }

        /**
         * Gives the interval [a,b] of the sieve.
         * 
         * @return sieved interval.
         */
        @Override
        public PositiveRange getSieveRange() {
            return (PositiveRange) sieveRange.clone();
        }

        /**
         * Gives the range of the indices of the prime numbers in the
         * enumeration.
         * 
         * @return Range of indices.
         */
        @Override
        public PositiveRange getPrimeRange() {
            return (PositiveRange) primeRange.clone();
        }

        /**
         * Gives the prime numbers in the iteration as an array.
         * 
         * @return An array of prime numbers representing the iteration.
         */
        @Override
        public int[] toArray() {
            int primeCard = primeRange.size();
            int[] primeList = new int[primeCard];

            System.arraycopy(sieve.primes, start, primeList, 0, primeCard);

            return primeList;
        }

        /**
         * Computes the product of all primes in the range of this iteration.
         * 
         * @return The product of all primes in the range of the iteration.
         */
        public Xint primorial() {
            if (0 == primeRange.getMax()) // if primeRange is empty...
            {
                return Xint.ONE;
            }
            return Xint.product(sieve.primes, start, primeRange.size());
        }

        /**
         * Writes the primes iteration to a file.
         * 
         * @param fileName
         *            Name of the file to write to.
         * @param format
         *            The print-format requested.
         */
        @Override
        public void toFile(String fileName, PrintOption format) {
            try {
                PrintWriter primeReport = new PrintWriter(new BufferedWriter(new FileWriter(fileName)));

                switch (format) {
                    case CommaSeparated:
                        writeComma(primeReport);
                        break;
                    case FormattedText:
                        writeFormatted(primeReport);
                        break;
                    case Xml:
                        writeXml(primeReport);
                        break;
                    default:
                        writeComma(primeReport);
                        break;
                }

                primeReport.close();
            } catch (IOException e) {
                if (null != logger) {
                    logger.severe(e.toString());
                }
            }
        }

        /**
         * Prints the list of prime numbers in a formatted way to a file.
         * 
         * @param file
         *            The file to be printed to.
         */
        private void writeFormatted(PrintWriter file) {
            file.println("SieveRange   " + sieveRange.toString() + " : " + sieveRange.size());
            file.println("PrimeRange   " + primeRange.toString() + " : " + primeRange.size());
            file.println("PrimeDensity " + getPrimeDensity());

            int primeOrdinal = start;
            int primeLow = sieve.primes[start];
            int lim = (primeLow / 100) * 100;

            for (int prime : this) {
                primeOrdinal++;

                if (prime >= lim) {
                    lim += 100;

                    file.println();
                    file.print('<');
                    file.print(primeOrdinal);
                    file.print(".> ");
                }

                file.print(prime);
                file.print(' ');
            }

            file.println();
        }

        /**
         * Prints the list of primes in a comma separated format to a file.
         * 
         * @param file
         *            The file to be printed to.
         */
        private void writeComma(PrintWriter file) {
            for (int prime : this) {
                file.print(prime);
                file.print(',');
            }
        }

        /**
         * Prints the list of prime numbers in XML format to a file.
         * 
         * @param file
         *            The file to be printed to.
         */
        private void writeXml(PrintWriter file) {
            int primeOrdinal = start;

            file.println("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            file.println("<primes>");

            for (int prime : this) {
                primeOrdinal++;
                file.print("<ol>(");
                file.print(primeOrdinal);
                file.print(',');
                file.print(prime);
                file.println(")</ol>");
            }

            file.println("</primes>");
        }
    } // endOfPrimeIteration
} // endOfPrimeSieve

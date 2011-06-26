// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.primes;

import de.luschny.math.util.PositiveRange;

import java.util.Iterator;

/**
 * An interface for iterating over a prime number sieve or over a subrange of
 * the sieve.
 * <p/>
 * Typical use is: <blockquote>
 * 
 * <pre>
 * IPrimeSieve sieve = new PrimeSieve(sieveLength);
 * IPrimeIteration primes = sieve.getIteration();
 * &lt;p/&gt;
 *   // Java 1.5 Tiger-style
 *   for (int prime : primes)
 *   {
 *      System.out.print(prime + &quot;,&quot;);
 *   }
 * </pre>
 * 
 * </blockquote>
 * <p/>
 * Caveat: IPrimeIteration inherits from java.util.Iterator the method 'remove'.
 * This (optional operation) is usually not supported by an implementation of
 * IPrimeIteration.
 * 
 * @author Peter Luschny
 * @version 2004-09-12
 */
public interface IPrimeIteration extends Iterable<Integer>, Iterator<Integer> {

    /**
     * Counts the number of primes in the iteration.
     * 
     * @return The number of primes in the iteration.
     */
    int getNumberOfPrimes();

    /**
     * Gives the interval [a,b] proceeded by the sieve. We call this interval
     * the SieveRange of the iteration.
     * 
     * @return The range of integers, in which the prime numbers are searched.
     * @see IPrimeIteration#getPrimeRange()
     */
    PositiveRange getSieveRange();

    /**
     * Gives the range of the indices of the prime numbers in the SieveRange of
     * the iteration. We call this range the PrimeRange of the iteration.
     * <p/>
     * To understand the difference between "SieveRange" and "PrimeRange"
     * better, let us consider the following example:
     * <p/>
     * <blockquote>
     * 
     * <pre>
     * If the SieveRange is 10..20, then the PrimeRange is 5..8,
     * because of the following table
     * &lt;p/&gt;
     *                    &lt;-     SieveRange           -&gt;
     * 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23
     * x,1,2,x,3,x,4,x,x, x, 5, x, 6, x, x, x, 7, x, 8, x, x, x, 9
     *                    &lt;-     PrimeRange           -&gt;
     * &lt;p/&gt;
     * Thus the smallest prime in the range 10..20 is the 5-th prime
     * and the largest prime in this range is the 8-th prime.
     * </pre>
     * 
     * </blockquote>
     * 
     * @return The range of indices of the primes.
     */
    PositiveRange getPrimeRange();

    /**
     * Computes the density of primes in the iteration.
     * 
     * @return Ratio of the number of primes relative to the number of all
     *         integers in this iteration.
     */
    double getPrimeDensity();

    /**
     * Extracts the primes in the SieveRange of the iteration to an array.
     * 
     * @return An array containing the prime numbers in the iteration.
     */
    int[] toArray();

    /**
     * Three options for formatting the list of primes when printing them to a
     * file.
     */
    enum PrintOption {

        /**
         * A comma-separated list of the prime numbers in the iteration.
         * <blockquote>
         * 
         * <pre>
         *   Example output:
         *   2,3,5,7,...
         * </pre>
         * 
         * </blockquote>
         */
        CommaSeparated,
        /**
         * A formated table of the prime numbers in the iteration.
         * <p/>
         * <blockquote>
         * 
         * <pre>
         * Example output:
         * &lt;p/&gt;
         * SieveRange   [100,333] : 234
         * PrimeRange   [ 26, 67] : 42
         * PrimeDensity 0.1794871794871795
         * &lt;p/&gt;
         * &lt;26.&gt; 101 103 107 109 113 127 131 137 139 149 151
         *       157 163 167 173 179 181 191 193 197 199
         * &lt;47.&gt; 211 223 227 229 233 239 241 251 257 263 269
         *       271 277 281 283 293
         * &lt;63.&gt; 307 311 313 317 331
         * </pre>
         * 
         * </blockquote>
         */
        FormattedText,
        /**
         * The prime numbers as pairs (ordinal, prime) in Xml-format.
         * <p/>
         * <blockquote>
         * 
         * <pre>
         * Example output:
         * (26,101)
         * (27,103)
         * (28,107)
         * </pre>
         * 
         * </blockquote>
         */
        Xml
    }

    /**
     * Saves the prime numbers in the SieveRange of the iteration to a file.
     * 
     * @param fileName
     *            The name of the file the prime numbers are written to.
     * @param format
     *            Sets the type of formatting the output.
     */
    void toFile(String fileName, PrintOption format);
}

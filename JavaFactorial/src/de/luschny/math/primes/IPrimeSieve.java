// Copyright (C) 2004-2009 Peter Luschny, MIT License applies.
// See http://en.wikipedia.org/wiki/MIT_License
// Visit http://www.luschny.de/math/factorial/FastFactorialFunctions.htm
// Comments mail to: peter(at)luschny.de
package de.luschny.math.primes;

import de.luschny.math.util.ILoggable;
import de.luschny.math.util.PositiveRange;

/**
 * An interface for a prime number sieve.
 * 
 * @author Peter Luschny
 * @version 2004-09-12
 */
public interface IPrimeSieve extends ILoggable {

    /**
     * Checks if a given number is prime.
     * 
     * @param i
     *            The number the primality of which is to be checked.
     * @return True if the given number is prime, false otherwise.
     */
    boolean isPrime(int i);

    /**
     * Get the n-th prime number. (The first prime number is 2, the second 3,
     * the third 5, ...)
     * 
     * @param n
     *            The index of the prime number searched.
     * @return The n-th prime number.
     */
    int getNthPrime(int n);

    /**
     * The default iteration of the full sieve. Thus it gives the iteration of
     * the prime numbers in the range [1, n], where n is the upper bound of the
     * sieve, which is an argument to the constructor of the sieve.
     * 
     * @return An iteration of the prime numbers in the range of the sieve.
     */
    IPrimeIteration getIteration();

    /**
     * Gives the iteration of the prime numbers in the range [low, high].
     * <p/>
     * Note: If the range is not in the range of the sieve, the function throws
     * an IllegalArgumentException.
     * 
     * @param low
     *            The lower bound of the range to be sieved.
     * @param high
     *            The upper bound of the range to be sieved.
     * @return An iteration of the prime numbers in the range [low, high].
     */
    IPrimeIteration getIteration(int low, int high);

    /**
     * Gives the iteration of the prime numbers in the given range.
     * <p/>
     * Note: If the range is not in the range of the sieve, the function throws
     * an IllegalArgumentException.
     * 
     * @param range
     *            The range of integers to be sieved.
     * @return An iteration of the prime numbers over the given range.
     */
    IPrimeIteration getIteration(PositiveRange range);
}

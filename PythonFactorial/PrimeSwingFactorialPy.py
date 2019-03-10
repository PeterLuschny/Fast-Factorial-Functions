#!/usr/bin/env python3

# psw_factorial, an implementation of the prime swing factorial.
# The claim is that this is the fastest known algorithm for
# computing the factorial. It is based on the prime factorization of n!.
# See Peter Luschny, https://oeis.org/A000142/a000142.pdf
# Also http://www.luschny.de/math/factorial/SwingFactorialPy.html
# http://luschny.de/math/factorial/FastFactorialFunctions.htm

from bisect import bisect_left as bisectleft
import math


def size_hint(n):
    return int(1.5 * (isqrt(n) + n / (math.log2(n) - 1.0)))


def Primes(n):
    """
    Return the primes in the interval 1..n (n inclusive).
    """
    # --- Sieve the primes à la Eratosthenes.
    lim, tog = n // 3, False
    composite = [False]*lim

    d1 = 8; d2 = 8; p1 = 3; p2 = 7; s1 = 7; s2 = 3; m = -1

    while s1 < lim:           # --  scan the sieve
        m += 1                # --  if a prime is found
        if not composite[m]:  # --  cancel its multiples
            inc = p1 + p2
            for k in range(s1     , lim, inc): composite[k] = True
            for k in range(s1 + s2, lim, inc): composite[k] = True

            tog = not tog
            if tog: s1 += d2; d1 += 16; p1 += 2; p2 += 2; s2 = p2
            else:   s1 += d1; d2 +=  8; p1 += 2; p2 += 6; s2 = p1

    # --- Collect the primes.
    primes = [0]*size_hint(n)
    primes[0] = 2; primes[1] = 3

    m, k, p, tog = 1, 0, 5, False
    while p <= n:
        if not composite[k]:
            m += 1
            primes[m] = p

        k += 1
        tog = not tog
        p += 2 if tog else 4

    return primes[0:m+1]


def isqrt(x):
    """
    Return the integer square root of x.
    """
    if x < 0:
        raise ValueError('square root not def. for negative numbers')
    n = int(x)
    if n == 0: return 0
    a, b = divmod(n.bit_length(), 2)
    x = 2 ** (a + b)
    while True:
        y = (x + n // x) // 2
        if y >= x: return x
        x = y


def product(A):
    """
    Return the accumulated product of an array.
    """
    def prod(a, b):
        n = b - a
        if n < 24:
            p = 1
            for k in range(a, b + 1):
                p *= A[k]
            return p
        m = (a + b) // 2
        return prod(a, m) * prod(m + 1, b)
    return prod(0, len(A) - 1)


def psw_factorial(n):
    """
    Return the factorial of n (using the prime-swing algorithm).
    """
    small_swing = [1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231, 3003,
        429, 6435, 6435, 109395, 12155, 230945, 46189, 969969, 88179,
        2028117, 676039, 16900975, 1300075, 35102025, 5014575, 145422675,
        9694845, 300540195, 300540195]

    def swing(m, primes):
        if m < 33: return small_swing[m]

        s = bisectleft(primes, 1 + isqrt(m))
        d = bisectleft(primes, 1 + m // 3)
        e = bisectleft(primes, 1 + m // 2)
        g = bisectleft(primes, 1 + m)

        factors = primes[e:g]
        factors += filter(lambda x: (m // x) & 1 == 1, primes[s:d])
        for prime in primes[1:s]:
            p, q = 1, m
            while True:
                q //= prime
                if q == 0: break
                if q & 1 == 1:
                    p *= prime
            if p > 1: factors.append(p)

        return product(factors)

    def odd_factorial(n, primes):
        if n < 2: return 1
        tmp = odd_factorial(n // 2, primes)
        return (tmp * tmp) * swing(n, primes)

    def eval(n):
        if n < 0:
            raise ValueError('factorial not def. for negative numbers')

        if n == 0: return 1
        if n < 20: return product(range(2, n + 1))

        N, bits = n, n
        while N != 0:
            bits -= N & 1
            N >>= 1

        primes = Primes(n)
        return odd_factorial(n, primes) << bits

    return eval(n)


import time

def main():
    """
    Test and benchmark:
    We are faster than math.factorial!
    """
    for n in range(1000):
        mf = math.factorial(n)
        psw = psw_factorial(n)
        if mf != psw: print("Error at", n)

    n = 1000; elapsed_last = 0
    while n < 10000000:
        print("Test n = {}".format(n), end='', flush=True)
        start = time.time()
        psw_factorial(n)
        # math.factorial(n)
        end = time.time()
        elapsed = end - start
        q = elapsed/elapsed_last if elapsed_last > 0 else 0
        print(", elapsed={:1.3f}s, quot={:1.3f}".format(elapsed, q))
        elapsed_last = elapsed
        n *= 4
        # Very roughly: if n is increased by a factor of 4
        # then the elapsed time increases by a factor of 10.


if __name__ == '__main__':
    main()

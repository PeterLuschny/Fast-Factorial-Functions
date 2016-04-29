#!/usr/bin/env python3.5
# -*- coding: utf-8 -*-
import bisect
import gmpy2
import primesieve


def factorial(n):
    def product(s, n, m):
        if (n > m):
            return 1
        if (n == m):
            return s[n]
        k = (n + m) // 2
        return product(s, n, k) * product(s, k + 1, m)

    def swing(m, primes):
        if m < 4:
            return [1, 1, 1, 3][m]
        s = bisect.bisect_left(primes, 1 + gmpy2.isqrt(m))
        d = bisect.bisect_left(primes, 1 + m // 3)
        e = bisect.bisect_left(primes, 1 + m // 2)
        g = bisect.bisect_left(primes, 1 + m)

        factors = primes[e:g]
        factors += filter(lambda x: (m // x) & 1 == 1, primes[s:d])
        for prime in primes[1:s]:
            p, q = 1, m
            while True:
                q //= prime
                if q == 0:
                    break
                if q & 1 == 1:
                    p *= prime
            if p > 1:
                factors.append(p)
        return product(factors, 0, len(factors) - 1)

    def odd_factorial(n, primes):
        if n < 2:
            return 1
        return((odd_factorial(n // 2, primes)**2) * swing(n, primes))

    def eval(n):
        if n < 10:
            return product(range(2, n + 1), 0, n - 2)
        bits = n - gmpy2.popcount(n)
        primes = primesieve.generate_primes(2, n + 1)
        return odd_factorial(n, primes) * 2 ** bits
    return eval(n)

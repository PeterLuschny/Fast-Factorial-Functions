# This is a performance optimized version of Peter Luschny's example on
# http://www.luschny.de/math/factorial/SwingFactorialPy.html
# Tested with Python 3.10.4 and gmpy 2.1.2 on windows

from math import isqrt
from bisect import bisect_left
from typing import Iterator
from gmpy2 import mpz, xmpz, mul
#from gmpy2 import xmpz as gmpy2_xmpz


def Primes(limit: int) -> Iterator:
    '''Returns a generator that yields the prime numbers up to limit.
       https://gmpy2.readthedocs.io/en/latest/advmpz.html'''

    # Increment by 1 to account for the fact that slices  do not include
    # the last index value but we do want to include the last value for
    # calculating a list of primes.
    sieve_limit = isqrt(limit) + 1
    limit += 1

    # Mark bit positions 0 and 1 as not prime.
    bitmap = xmpz(3)

    # Process 2 separately. This allows us to use p+p for the step size
    # when sieving the remaining primes.
    bitmap[4 : limit : 2] = -1

    # Sieve the remaining primes.
    for p in bitmap.iter_clear(3, sieve_limit):
        bitmap[p * p : limit : p + p] = -1

    return bitmap.iter_clear(2, limit)


def product(s: list[int], n: int, m: int) -> mpz:
    if n > m:
        return mpz(1)
    if n == m:
        return mpz(s[n])
    k: int = (n + m) // 2
    return mul(product(s, n, k), product(s, k + 1, m))


def primeswing_factorial(n: int) -> mpz:

    small_swing: list[int] = [1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231,
        3003, 429, 6435, 6435, 109395, 12155, 230945, 46189, 969969, 88179,
        2028117, 676039, 16900975, 1300075, 35102025, 5014575, 145422675,
        9694845, 300540195, 300540195]

    def swing(m: int, primes: list[int]) -> mpz:
        if m < 33:
            return mpz(small_swing[m])

        s: int = bisect_left(primes, 1 + isqrt(m))
        d: int = bisect_left(primes, 1 + m // 3)
        e: int = bisect_left(primes, 1 + m // 2)
        g: int = bisect_left(primes, 1 + m)

        factors: list[int] = primes[e:g] + [x for x in primes[s:d] if (m // x) & 1 == 1]

        for prime in primes[1:s]:
            p: int = 1
            q: int = m
            while True:
                q //= prime
                if q == 0:
                    break
                if q & 1 == 1:
                    p *= prime
            if p > 1:
                factors.append(p)

        return product(factors, 0, len(factors) - 1)

    def odd_factorial(n: int, primes: list[int]) -> mpz:
        if n < 2:
            return mpz(1)
        tmp: mpz = odd_factorial(n // 2, primes)
        return mul(mul(tmp, tmp), swing(n, primes))

    def eval(n: int) -> mpz:
        if n < 0:
            raise ValueError("factorial not defined for negative numbers")

        if n == 0 or n == 1:
            return mpz(1)
        if n < 20:
            return product(list(range(2, n + 1)), 0, n - 2)

        bits: int = n - n.bit_count()

        primes: list[int] = list(Primes(n))
        return mul(odd_factorial(n, primes), 2 ** bits)

    return eval(n)


if __name__ == "__main__":

    from math import factorial as math_factorial
    import time

    def test(lg: int) -> None:
        for n in range(lg):
            mf: int = math_factorial(n)
            psw: int = int(primeswing_factorial(n))
            if mf != psw:
                print("Error at", n)
        print("Test passed!")

    def bench(ex: int) -> None:
        """
        We are faster than math.factorial (almost always)!
        """
        elapsed: float = 0.0
        n: int = 1000
        while n < 11 ** ex:
            elapsed_last: float = elapsed
            print(f'Test n = {n:7d}', end='', flush=True)
            start: float = time.time()
            primeswing_factorial(n)
            # math_factorial(n)
            end: float = time.time()
            elapsed = end - start
            q: float = elapsed / elapsed_last if elapsed_last > 0 else 0
            print(f' elapsed:{elapsed:1.3f}s, quot:{q:1.1f}')
            n *= 4

    test(1000)
    bench(7)

"""
Very roughly: if n is increased by a factor of 4
then the elapsed time increases by a factor of 10.

math_factorial:

Test n =    1000, elapsed=   0.000s, quot= 0.0
Test n =    4000, elapsed=   0.001s, quot= 0.0
Test n =   16000, elapsed=   0.016s, quot=15.5
Test n =   64000, elapsed=   0.175s, quot=11.0
Test n =  256000, elapsed=   1.682s, quot= 9.6
Test n = 1024000, elapsed=  17.233s, quot=10.2
Test n = 4096000, elapsed= 179.829s, quot=10.4

primeswing_factorial:

Test n =    1000, elapsed=   0.000s, quot= 0.0
Test n =    4000, elapsed=   0.003s, quot= 0.0
Test n =   16000, elapsed=   0.014s, quot= 1.8
Test n =   64000, elapsed=   0.105s, quot= 7.5
Test n =  256000, elapsed=   1.144s, quot=10.9
Test n = 1024000, elapsed=  11.881s, quot=10.4
Test n = 4096000, elapsed= 117.866s, quot= 9.9

primeswing_factorial with gmpy2:

Test n =    1000 elapsed: 0.000s, quot:0.0
Test n =    4000 elapsed: 0.003s, quot:0.0
Test n =   16000 elapsed: 0.004s, quot:1.2
Test n =   64000 elapsed: 0.026s, quot:6.5
Test n =  256000 elapsed: 0.113s, quot:4.3
Test n = 1024000 elapsed: 0.585s, quot:5.2
Test n = 4096000 elapsed: 2.925s, quot:5.0

Test n =  16384000 elapsed:14.314s, quot:5.0
Test n =  65536000 elapsed:70.567s, quot:4.9
Test n = 262144000 ...
GNU MP: Cannot allocate memory (size=4294704160)

Conclusion: if n < 10000 go with math_factorial,
otherwise choose primeswing_factorial.

For n = 4096000 we are down from 3 minutes to 3 seconds.
"""

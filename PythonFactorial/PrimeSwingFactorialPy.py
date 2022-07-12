# psw_factorial, an implementation of the prime swing factorial.
# The claim is that this is the fastest known algorithm for
# computing the factorial. It is based on the prime factorization of n!.
# See Peter Luschny, https://oeis.org/A000142/a000142.pdf
# Also http://www.luschny.de/math/factorial/SwingFactorialPy.html
# http://luschny.de/math/factorial/FastFactorialFunctions.htm

from bisect import bisect_left
from math import log, isqrt


def prime_pi_upper_bound(n) -> int:
    """
    Return an upper bound of the number of primes below n.
    """
    ln: float = log(n)
    return int(n / (ln - 1.0 - (154 / 125) / ln))


def Primes(n: int) -> list[int]:
    """
    Return the primes in the interval 1..n (n inclusive).
    """
    # --- Sieve the primes à la Eratosthenes.
    lim: int = n // 3
    tog: bool = False
    composite: list[bool] = [False] * lim

    d1: int = 8
    d2: int = 8
    p1: int = 3
    p2: int = 7
    s1: int = 7
    s2: int = 3
    m: int = -1

    while s1 < lim:  # --  scan the sieve
        m += 1  # --  if a prime is found
        if not composite[m]:  # --  cancel its multiples
            inc: int = p1 + p2
            for k in range(s1, lim, inc):
                composite[k] = True
            for k in range(s1 + s2, lim, inc):
                composite[k] = True

            tog = not tog
            if tog:
                s1 += d2
                d1 += 16
                p1 += 2
                p2 += 2
                s2 = p2
            else:
                s1 += d1
                d2 += 8
                p1 += 2
                p2 += 6
                s2 = p1

    # --- Collect the primes.
    primes: list[int] = [0] * prime_pi_upper_bound(n)
    primes[0] = 2
    primes[1] = 3
    p: int = 5

    m = 1
    k = 0
    tog = False

    while p <= n:
        if not composite[k]:
            m += 1
            primes[m] = p

        k += 1
        tog = not tog
        p += 2 if tog else 4

    return primes[0:m + 1]


def product(A: list[int]) -> int:
    """
    Return the accumulated product of an array.
    """

    def prod(a, b) -> int:
        n: int = b - a
        if n < 24:
            p: int = 1
            for k in range(a, b + 1):
                p *= A[k]
            return p
        m: int = (a + b) // 2
        return prod(a, m) * prod(m + 1, b)

    return prod(0, len(A) - 1)


def primeswing_factorial(n: int) -> int:
    """
    Return the factorial of n (using the prime-swing algorithm).
    """
    small_swing: list[int] = [1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231,
        3003, 429, 6435, 6435, 109395, 12155, 230945, 46189, 969969, 88179,
        2028117, 676039, 16900975, 1300075, 35102025, 5014575, 145422675,
        9694845, 300540195, 300540195]

    def swing(m: int, primes: list[int]) -> int:
        if m < 33:
            return small_swing[m]

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

        return product(factors)

    def odd_factorial(n: int, primes: list[int]) -> int:
        if n < 2:
            return 1
        tmp: int = odd_factorial(n // 2, primes)
        return (tmp * tmp) * swing(n, primes)

    def eval(n: int) -> int:
        if n < 0:
            raise ValueError("factorial not defined for negative numbers")

        if n == 0:
            return 1
        if n < 20:
            return product(list(range(2, n + 1)))

        bits: int = n - n.bit_count()
        # if you have no bit_count:
        # N: int = n
        # bits: int = n
        # while N != 0:
        #     bits -= N & 1
        #     N >>= 1

        primes: list[int] = Primes(n)
        return odd_factorial(n, primes) << bits

    return eval(n)


if __name__ == "__main__":

    from math import factorial as math_factorial
    import time


    def test(lg: int) -> None:
        for n in range(lg):
            mf: int = math_factorial(n)
            psw: int = primeswing_factorial(n)
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
    bench(6)

"""
Very roughly: if n is increased by a factor of 4
then the elapsed time increases by a factor of 10.

math_factorial:

Test n =    1000, elapsed=  0.000s, quot= 0.0
Test n =    4000, elapsed=  0.001s, quot= 0.0
Test n =   16000, elapsed=  0.016s, quot=15.5
Test n =   64000, elapsed=  0.175s, quot=11.0
Test n =  256000, elapsed=  1.682s, quot= 9.6
Test n = 1024000, elapsed= 17.233s, quot=10.2
Test n = 4096000, elapsed=179.829s, quot=10.4

primeswing_factorial:

Test n =    1000, elapsed=  0.000s, quot=  0.0
Test n =    4000, elapsed=  0.003s, quot=  0.0
Test n =   16000, elapsed=  0.014s, quot=  1.8
Test n =   64000, elapsed=  0.105s, quot=  7.5
Test n =  256000, elapsed=  1.144s, quot= 10.9
Test n = 1024000, elapsed= 11.881s, quot= 10.4
Test n = 4096000, elapsed=117.866s, quot=  9.9

Conclusion: if n <= 10000 go with math_factorial,
otherwise choose primeswing_factorial.
"""

# There are at least five methods for 'prod' (including ours
# and the one from the 'math' module).
# TODO: benchmark which is fastest
# https://stackoverflow.com/questions/2104782/returning-the-product-of-a-list

# Also the use of an alternative prime sive should be considered.
# https://stackoverflow.com/questions/2068372/fastest-way-to-list-all-primes-below-n/70804184#70804184
# https://stackoverflow.com/questions/2211990/how-to-implement-an-efficient-infinite-generator-of-prime-numbers-in-python

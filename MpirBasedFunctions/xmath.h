// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

#ifndef XMATH_H_
#define XMATH_H_

#include <assert.h>
#include <thread>
#include "lmp.h"
#include "primeswing.h"

class Xmath {
public:

    static void Product(Xint result, ulong* a, slong start, slong len)
    {
        if (len < 4)
        {
            lmp::SetUi(result, a[start]);
            if (len == 1) return;
            lmp::MulUi(result, result, a[start + 1]);
            if (len == 2) return;
            lmp::MulUi(result, result, a[start + 2]);
            return;
        }

        Xint temp1; lmp::Init(temp1);
        slong halfLen = len / 2;

        Product(temp1, a, start, halfLen);
        Product(result, a, start + halfLen, len - halfLen);
        lmp::Mul(result, result, temp1);

        lmp::Clear(temp1);
    }

    static const slong PITHRESHOLD = 10000;

    static void ParallelProduct(Xint result, ulong* a, slong start, slong len)
    {
        if(len < PITHRESHOLD)
        {
            Xmath::Product(result, a, start, len);
            return;
        }

        slong threadCnt = (slong) std::thread::hardware_concurrency();

        if(threadCnt < 2)
        {
            Xmath::Product(result, a, start, len);
            return;
        }

        slong halfLen = len / 2;
        Xint temp1; lmp::Init(temp1);

        std::thread prod(Xmath::Product, temp1, a, start, halfLen);
        Xmath::Product(result, a, start + halfLen, len - halfLen);
        prod.join();
        lmp::Mul(result, result, temp1);

        lmp::Clear(temp1);
    }

/* Prime number sieve, Eratosthenes (276-194 b. t.)
*  This implementation considers only multiples of primes
*  greater than 3, so the smallest value has to be mapped to 5.
*  Note: There is no multiplication operation in this function
*  and no call to a sqrt function.
*  Returns the prime numbers not exceeding n, i.e. p_1,p_2,...,p_s where
*  p_1 = 2 and p_s &leq; n. Additionally the number of primes &leq; n,
*  is returned. The third argument, plist, is boolean. plist = 1 returns
*  a list of the primes whereas plist = 0 returns only the number of primes
*  found without allocating a prime list.
*/

#define IS_PRIME(P) ((isComposite[(P) >> 5] & pow2[(P) & (bitsPerInt - 1)]) == 0)
#define SET_COMPOSITE(C) (isComposite[(C) >> 5] |= pow2[(C) & (bitsPerInt - 1)])

static slong xPrimeSieve(ulong** Primes, ulong n, int plist)
{
    const int bitsPerInt = 32;
    int toggle = 0, i, t, bitFieldLength;
    ulong *isComposite; ulong *primes;
    ulong d1 = 8, d2 = 8, p1 = 3, p2 = 7, s = 7, s2 = 3;
    ulong p = 0, k = 1, max = n / 3, piN, inc;
    ulong pow2[bitsPerInt];

    /* precondition n > 2 */
    assert(n > 2);

    bitFieldLength = (n / (3 * bitsPerInt)) + 1;
    isComposite = lmp::MallocUi(bitFieldLength);
    for (i = 0; i < bitFieldLength; i++) isComposite[i] = 0;

    /* bitfield powers of 2 */
    pow2[0] = 1; for (i = 1; i < bitsPerInt; i++) pow2[i] = k <<= 1;

    while (s < max)  /* --  scan the sieve */
    {
        if (IS_PRIME(p))   /* --  if a prime is found */
        {
            inc = p1 + p2;   /* --  then cancel its multiples */
            for (k = s; k < max; k += inc) { SET_COMPOSITE(k); }
            for (k = (s + s2); k < max; k += inc) { SET_COMPOSITE(k); }
        }

        if (toggle = ~toggle) { s += d2; d1 += 16; p1 += 2; p2 += 2; s2 = p2; }
        else { s += d1; d2 += 8; p1 += 2; p2 += 6; s2 = p1; }
        p++;
    }

    toggle = 0; p = 5; s = 0; piN = 2;

    if(plist == 1)  /* Return list of primes. */
    {
        /* Create the array of prime numbers. */
        k = 2;
        for (t = 0; t < bitFieldLength; t++)
            k += BitCount(~isComposite[t]);

        primes = lmp::MallocUi(k);
        primes[0] = 2; primes[1] = 3;

        /* Transform the sieve into the sequence of prime numbers. */
        while (p <= n)
        {
            ulong isc = isComposite[s++];
            for (t = 0; t < bitsPerInt; t++)
            {
                if ((isc & 1) == 0) { primes[piN++] = p; }
                p += (toggle = ~toggle) ? 2u : 4u;
                if (p > n) break;
                isc >>= 1;
            }
        }
        *Primes = primes;
    }
    else /* Return only the number of primes. */
    {
        while (p <= n)
        {
            ulong isc = isComposite[s++];
            for (t = 0; t < bitsPerInt; t++)
            {
                if ((isc & 1) == 0) { piN++; }
                p += (toggle = ~toggle) ? 2u : 4u;
                if (p > n) break;
                isc >>= 1;
            }
        }
        *Primes = NULL;
    }

    free(isComposite);
    return piN;
}

static slong PrimeSieve(ulong** Primes, slong n)
{
    if (n <= 2)
    {
        ulong* primes = (ulong*) malloc(sizeof(ulong));
        if (n == 2) { primes[0] = 2; *Primes = primes; return 1; }
        else { primes[0] = 0; *Primes = primes; return 0; }
    }

    /* precondition of xPrimeSieve is n > 2 */
    return xPrimeSieve(Primes, n, 1);
}

static slong NumberOfPrimes(ulong n)
{
    ulong* Primes = NULL;
    if (n  <= 2) {
        if (n == 2) { return 1; } else {return 0; }
    }

    /* precondition of xPrimeSieve is n > 2 */
    return xPrimeSieve(&Primes, n, 0);
}

    static void NaiveFactorial(Xint result, slong n)
    {
        assert(n <= 20);  // TODO: Error handling
        lmp::SetUi(result, 1);
        if (n < 2) { return; }
        ulong nat[] = {
            1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
        Product(result, nat, 0, n);
    }

    static void NaiveDoubleFactorial(Xint result, ulong n)
    {
        lmp::SetUi(result, 1);

        while (n > 1)
        {
            lmp::MulUi (result, result, n);
            n -= 2;
        }
    }

    static void NaiveBinomial(Xint result, ulong n, ulong k)
    {
        Xint temp1; lmp::Init(temp1);
        Xint temp2; lmp::Init(temp2);

        PrimeSwing::Factorial(result, n);
        PrimeSwing::Factorial(temp1, k);
        PrimeSwing::Factorial(temp2, n - k);
        lmp::Mul (temp1, temp1, temp2);
        lmp::Div (result, result, temp1);

        lmp::Clear(temp1);
        lmp::Clear(temp2);
    }

    static slong Sqrt(slong n)
    {
        slong a, b;
        a = b = n;
        do {
            a = b;
            b = (n / a + a) / 2;
        } while (b < a);
        return a;
    }

    static slong BitCount(slong n)
    {
        slong bc = n - ((n >> 1) & 0x55555555);
        bc = (bc & 0x33333333) + ((bc >> 2) & 0x33333333);
        bc = (bc + (bc >> 4)) & 0x0f0f0f0f;
        bc += bc >> 8;
        bc += bc >> 16;
        bc = bc & 0x3f;
        return bc;
    }
};

#endif // XMATH_H_

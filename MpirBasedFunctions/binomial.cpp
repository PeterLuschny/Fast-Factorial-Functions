// Author & Copyright : Peter Luschny
// Created: 2010-02-01
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

// Based on:
// P. Goetgheluck, Computing Binomial Coefficients,
// American Math. Monthly 94 (1987), 360-365.
// with some additional features.

#include <assert.h>
#include "binomial.h"
#include "xmath.h"

void Binomial::PrimeBinomial(Xint binom, ulong n, ulong k)
{
    assert(k <= n);  // TODO: Error handling

    lmp::SetUi(binom, 1);
    if ((k == 0) || (k == n)) return;

    if (k > n / 2) { k = n - k; }
    ulong fi = 0, nk = n - k;

    ulong rootN = Xmath::Sqrt(n);
    ulong* primes;
    ulong piN = Xmath::PrimeSieve(&primes, n);

    for(ulong i = 0; i < piN; i++)
    {
        ulong prime = primes[i];

        if (prime > nk)
        {
            primes[fi++] = prime;
            continue;
        }

        if (prime > n / 2)
        {
            continue;
        }

        if (prime > rootN)
        {
            if (n % prime < k % prime)
            {
                primes[fi++] = prime;
            }
            continue;
        }

        ulong N = n, K = k, p = 1, r = 0;

        while (N > 0)
        {
            r = (N % prime) < (K % prime + r) ? 1 : 0;
            if (r == 1)
            {
                p *= prime;
            }
            N /= prime;
            K /= prime;
        }
        primes[fi++] = p;
    }

    Xmath::Product(binom, primes, 0, fi);
    lmp::FreeUi(primes, piN);
}


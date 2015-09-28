// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

#include <string.h>
#include "primeswing.h"
#include "xmath.h"

void PrimeSwing::Factorial(Xint fact, ulong n)
{
    if (n < THRESHOLD) { Xmath::NaiveFactorial(fact, n); return; }
    lmp::SetUi(fact, 1);

    ulong* primes;
    ulong piN = Xmath::PrimeSieve(&primes, n);
    ulong* factors = lmp::MallocUi(piN);
    ulong iterLen = 0; ulong i = n;
    slong m = n; while (m > 0) { m >>= 1; iterLen++; }
    ulong* iter = lmp::MallocUi(iterLen);
    ulong* lim  = lmp::MallocUi(iterLen);
    m = n; i = iterLen;
    while (m > 0) { iter[--i] = m; m >>= 1; }

    Xint primorial; lmp::Init(primorial);
    Xint swing; lmp::Init(swing);

    lim[0] = 0;
    for (i = 1; i < iterLen; i++)
        lim[i] = iter[i] < SOSLEN / 2 ?  0 :
        GetIndexOf(primes, iter[i], lim[i-1], piN);

    for (i = 1; i < iterLen; i++)
    {
        ulong N = iter[i];

        if (N < SOSLEN)
        {
            lmp::SetUi(swing, smallOddSwing[N]);
        }
        else
        {
            ulong prime = 3;
            slong pi = 2, fi = 0;
            ulong max = Xmath::Sqrt(N);

            while (prime <= max)
            {
                ulong q = N, p = 1;
                while ((q /= prime) > 0)
                {
                    if ((q & 1) == 1) { p *= prime; }
                }

                if (p > 1)
            {
               factors[fi++] = p;
            }
                prime = primes[pi++];
            }

            max = N / 3;
            while (prime <= max)
            {
                if (((N / prime) & 1) == 1)
                {
                    factors[fi++] = prime;
                }
                prime = primes[pi++];
            }

            // for(int k=lim[i-1]; k<lim[i]; k++) {factors[fi++]=primes[k];}

            pi = lim[i] - lim[i-1];
            memcpy(factors + fi, primes + lim[i-1], pi * sizeof(ulong));

            Xmath::Product(swing, factors, 0, fi + pi);
        }

        lmp::Pow2(fact, fact);
        lmp::Mul(fact, fact, swing);
    }

    lmp::Mul2Exp(fact, fact, n - Xmath::BitCount(n));

    lmp::Clear(swing);
    lmp::Clear(primorial);
    lmp::FreeUi(primes, piN);
    lmp::FreeUi(factors, piN);
    lmp::FreeUi(iter, iterLen);
    lmp::FreeUi(lim, iterLen);
}

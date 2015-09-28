// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

#include "schoenhage.h"
#include "xmath.h"

void Schoenhage::Factorial(Xint result, ulong n)
{
    lmp::SetUi(result, 1);

    if (n < 2) { return; }

    slong iterLen = 0, m = n;
    ulong lim = n / 2, i;
    while (m > 0) { m >>= 1; iterLen++; }

    ulong* primes = 0; 
    ulong piN = Xmath::PrimeSieve(&primes, n);
    ulong* exponents = lmp::MallocUi(piN);
    ulong* factors = lmp::MallocUi(piN);

    exponents[0] = n - Xmath::BitCount(n);

    for(i = 1; i < piN; i++)
    {
        ulong p = primes[i];
        ulong exp = 1;
        if (p <= lim)
        {
            slong N = n / p;
            exp = N;
            while (N > 0)
            {
                N /= p;
                exp += N;
            }
        }
        exponents[i] = exp;
    }

    Xint prod; lmp::Init(prod);

    for (; iterLen >= 0; iterLen--)
    {
        ulong count = 0;
        for (ulong k = 1; k < piN ; k++)
        {
            if (((exponents[k] >> iterLen) & 1) == 1)
            {
                factors[count++] = primes[k];
            }
        }

        lmp::Pow2(result, result); 
        if (count > 0)
        {
            Xmath::Product(prod, factors, 0, count);
            lmp::Mul(result, result, prod);
        }
    }

    lmp::Mul2Exp(result, result, exponents[0]);

    lmp::FreeUi(factors, piN);
    lmp::FreeUi(primes, piN);
    lmp::FreeUi(exponents, piN);
    lmp::Clear(prod);
}



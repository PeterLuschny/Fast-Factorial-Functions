// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

#ifndef PRIMESWING_H_
#define PRIMESWING_H_

#include "lmp.h"

static const ulong smallOddSwing[] =  {
    1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231, 3003, 429,
    6435, 6435, 109395, 12155, 230945, 46189, 969969, 88179,
    2028117, 676039, 16900975, 1300075, 35102025, 5014575,
    145422675, 9694845, 300540195, 300540195 };

class PrimeSwing {
public:

    // Computes the factorial of an integer.
    static void Factorial(Xint fact, ulong n);

    // Computes the factorial of an integer using threads.
    static void ParallelFactorial(Xint fact, ulong n);

    // Computes the double factorial (Swing algo).
    static void DoubleFactorial(Xint fact, ulong n);

    // Computes the double factorial (Swing algo).
    static void ParallelDoubleFactorial(Xint fact, ulong n);

private:

    static const unsigned int THRESHOLD = 20;
    static const ulong SOSLEN = 33;

    static long GetIndexOf(ulong* p, ulong val, slong low, slong high)
    {
        slong lim = high;
        while (low < high)
        {
            slong mid = low + (high - low) / 2;
            if (p[mid] < val) { low = mid + 1; }
            else { high = mid; }
        }
        if (low >= lim) { return low; }
        if (p[low] == val) { low++; }

        return low;
    }
};

#endif // PRIMESWING_H_


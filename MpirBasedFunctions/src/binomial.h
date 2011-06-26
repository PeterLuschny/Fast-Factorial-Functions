// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

#ifndef BINOMIAL_H_
#define BINOMIAL_H_

#include "lmp.h"

class Binomial {
public:
    // Computes the factorial of an integer.
    static void PrimeBinomial(Xint fact, ulong n, ulong k);

    // Computes the double factorial (Swing algo).
    static void ParallelBinomial(Xint fact, ulong n, ulong k);
};

#endif // BINOMIAL_H_

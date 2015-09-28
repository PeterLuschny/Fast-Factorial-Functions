// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

#ifndef SCHOENHAGE_H_
#define SCHOENHAGE_H_

#include "lmp.h"

class Schoenhage {
public:

    // Computes the factorial of an integer.
    static void Factorial(Xint fact, ulong n);

    // Computes the factorial of an integer using threads.
    static void ParallelFactorial(Xint fact, ulong n);
};

#endif // SCHOENHAGE_H_


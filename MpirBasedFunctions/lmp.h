// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

// Adapter for library functions.

#ifndef LMP_H_
#define LMP_H_

#include <stdlib.h>

#ifndef __GMP_H__
#include "mpir.h"
#endif

typedef mpz_t Xint;
typedef long int slong;
typedef unsigned long int ulong;

// Paul Zimmermann's implementation of the double factorial
void mpz_fac_ui2 (mpz_ptr result, ulong n);

class lmp {
public:

    static void Init(Xint res) {
        mpz_init(res);
    }
    static void SetUi(Xint res, ulong n) {
        mpz_set_ui(res, n);
    }
    static void InitSetUi(Xint res, ulong n) {
        mpz_init_set_ui(res, n);
    }
    static void MulUi(Xint res, Xint op1, ulong op2) {
        mpz_mul_ui(res, op1, op2);
    }
    static void Mul(Xint res, Xint op1, Xint op2) {
        mpz_mul(res, op1, op2);
    }
    static void Div(Xint res, Xint op1, Xint op2) {
        mpz_cdiv_q(res, op1, op2);
    }
    // Assuming that mul detects the special case of squaring.
    // Otherwise the use of pow(bas, 2) might be better.
    static void Pow2(Xint res, Xint bas) {
        mpz_mul(res, bas, bas);
    }
    static void Mul2Exp(Xint res, Xint op1, slong op2) {
        mpz_mul_2exp(res, op1, op2);
    }
    static void FacUi(Xint res, ulong n) {
        mpz_fac_ui(res, n);
    }
    static void BinomialUiUi(Xint res, ulong n, ulong k) {
        mpz_bin_uiui(res, n, k);
    }
    static void ZimmermannFacUi2(mpz_ptr res, ulong n) {
        mpz_fac_ui2 (res, n);
    }
    static slong Cmp(Xint op1, Xint op2) {
        return mpz_cmp(op1, op2);
    }
    static void Clear(Xint b) {
        mpz_clear(b);
    }
    static slong* Malloc(ulong card) {
        return (slong*) (malloc)(card * sizeof(slong));
    }
    static ulong* MallocUi(ulong card) {
        return (ulong*) (malloc)(card * sizeof(ulong));
    }
    static void Free(slong* list, ulong card) {
        free(list);
    }
    static void FreeUi(ulong* list, ulong card) {
        free(list);
    }
};

#endif // LMP_H_

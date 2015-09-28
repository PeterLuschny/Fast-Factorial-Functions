/* Computes double factorial:
   http://en.wikipedia.org/wiki/Factorial#Double_factorial

   Copyright 2009, 2010, Paul Zimmermann

   Based on algorithm page 226 of the book
   "Fast Algorithms, A Multitape Turing Machine Implementation",
   by A. Scho"nhage, A. F. W. Grotefeld and E. Vetter,
   BI-Wissenschaftsverlag, 1994.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation; either version 2.1 of the License, or (at your
option) any later version.

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public
License for more details.

You should have received a copy of the GNU Lesser General Public License
along with the GNU MPFR Library; see the file COPYING.LIB.  If not, write to
the Free Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston,
MA 02110-1301, USA. */

/* <P.L.> 2010.01.26 recieved as double_fac_ui.c
from: http://www.loria.fr/~zimmerma/software/
All modifications by Peter Luschny are marked.
They do not affect the double factorial function.
<P.L.> */

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
/* <P.L.>
#include <sys/resource.h>
#include "gmp.h"
<P.L.> */
#include "lmp.h"

/* <P.L.>
int cputime (void);
<P.L.> */
void mpz_fac_ui4 (mpz_ptr, unsigned long int *, unsigned long int);
void mpz_fac_ui3 (mpz_ptr, unsigned long int *, unsigned long int *,
     unsigned long int *, unsigned long int);
void mpz_fac_ui2 (mpz_ptr, unsigned long int);

/* <P.L.>
int
cputime ()
{
    struct rusage rus;

    getrusage (0, &rus);
    return rus.ru_utime.tv_sec * 1000 + rus.ru_utime.tv_usec / 1000;
}
<P.L.> */

/* result <- q[0]*q[1]*...*q[n-1] by binary splitting */
void
    mpz_fac_ui4 (mpz_ptr result, unsigned long int *q, unsigned long int n)
{
    unsigned long int i, n0, n1;
    mpz_t tmp;

    if (n == 0)
    {
        mpz_set_ui (result, 1);
        return;
    }

    if (n <= 3)
    {
        mpz_set_ui (result, q[0]);
        for (i = 1; i < n; i++)
            mpz_mul_ui (result, result, q[i]);
        return;
    }

    n0 = n / 2;
    n1 = n - n0;
    mpz_fac_ui4 (result, q, n1);
    mpz_init (tmp);
    mpz_fac_ui4 (tmp, q + n1, n0);
    mpz_mul (result, result, tmp);
    mpz_clear (tmp);
}

/* put p[0]^e[0]*...*p[n-1]^e[n-1] in result,
assuming e[0] >= e[1] >= ... >= e[n-1] > 0.

Uses auxiliary table q[0...n-1].
*/
void
    mpz_fac_ui3 (mpz_ptr result, unsigned long int *p, unsigned long int *e,
    unsigned long int *q, unsigned long int n)
{
    unsigned long int i, j, mask;
    mpz_t tmp;

    /* find largest power of two <= the exponent of two */
    for (mask = 1; e[0] >= (mask << 1); mask <<= 1);
    mpz_init (tmp);
    mpz_set_ui (result, 1);
    while (mask)
    {
        /* put part corresponding to bit mask set in q[] */
        for (i = 0, j = 0; i < n; i++)
            if (e[i] & mask)
                q[j++] = p[i];
        mpz_fac_ui4 (tmp, q, j);
        mpz_mul (result, result, result);
        mpz_mul (result, result, tmp);
        mask >>= 1;
    }
    mpz_clear (tmp);
}

void
    mpz_fac_ui2 (mpz_ptr result, unsigned long int n)
{
    unsigned long int *p;
    unsigned long int *e;
    unsigned long int *q;
    unsigned long int i, j, k;

    if (n <= 1)
    {
        mpz_set_ui (result, 1);
        return;
    }

    if (n == 2)
    {
        mpz_set_ui (result, 2);
        return;
    }

    p = (unsigned long int*) malloc ((n + 1) * sizeof(unsigned long int));
    e = (unsigned long int*) malloc ((n + 1) * sizeof(unsigned long int));
    for (i = 2; i <= n; i++)
    {
        p[i] = i;
        e[i] = (n + 1 - i) & 1;
    }
    for (i = 2; i < n / 2; i++)
    {
        if (p[i] != 1) /* i is prime */
        {
            /* remove factor i in 2i, 3i, ... */
            for (j = 2 * i; j <= n; j += i)
            {
                p[j] /= i;
                e[i] += (n + 1 - j) & 1;
            }
            /* remove factor i in i^2, 2i^2, ..., i^3, 2i^3, ... */
            for (k = i; k <= n;)
            {
                /* check that k * i does not overflow */
                if (k > (~(0UL)) / i)
                    break;
                k *= i;
                for (j = k; j <= n; j += k)
                {
                    p[j] /= i;
                    e[i] += (n + 1 - j) & 1;
                }
            }
        }
    }
    /* shrink table */
    for (i = 2, j = 0; i <= n; i++)
    {
        if (p[i] != 1)
        {
            p[j] = p[i];
            e[j] = e[i];
            j++;
        }
    }
    q = (unsigned long int*) malloc (j * sizeof(unsigned long int));
    mpz_fac_ui3 (result, p + 1, e + 1, q, j - 1);
    /* treat apart exponent of two */
    mpz_mul_2exp (result, result, e[0]);
    free (q);
    free (p);
    free (e);
}

/* <P.L.>
void
double_fac_ui_ref (mpz_t f, unsigned long n)
{
   mpz_set_ui (f, 1);
   while (n > 1)
   {
      mpz_mul_ui (f, f, n);
      n -= 2;
   }
}

int
main (int argc, char *argv[])
{
   int n = atoi (argv[1]);
   mpz_t f, f2;
   int st;

   mpz_init (f);
   mpz_init (f2);

   st = cputime ();
   double_fac_ui_ref (f, n);
   printf ("double_fac_ui took %dms\n", cputime () - st);

   st = cputime ();
   mpz_fac_ui2 (f2, n);
   printf ("mpz_fac_ui2 took %dms\n", cputime () - st);

   if (mpz_cmp (f, f2))
   {
      fprintf (stderr, "f and f2 differ\n");
      exit (1);
   }

   mpz_clear (f);
   mpz_clear (f2);

   return 0;
}
<P.L.> */


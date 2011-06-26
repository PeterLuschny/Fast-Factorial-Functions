// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

#if(MPIR)
namespace SharithMP.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;
#else
    namespace Sharith.Math.Factorial {
    using XInt = System.Numerics.BigInteger;
#endif
    using Sharith.Math.Primes;
    using XMath = Sharith.Math.MathUtils.XMath;

    public class PrimeSchoenhage : IFactorialFunction 
    {
        public PrimeSchoenhage() { }

        public string Name
        {
            get { return "PrimeSchoenhage     "; }
        }

        private int[] primeList;
        private int[] multiList;

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            int lgN = XMath.FloorLog2(n);
            int piN = 2 + (15 * n) / (8 * (lgN - 1));

            primeList = new int[piN];
            multiList = new int[piN];

            int len = PrimeFactors(n);
            int exp2 = n - XMath.BitCount(n);

            return NestedSquare(len) << exp2;
        }

        private XInt NestedSquare(int len)
        {
            if (len == 0) return XInt.One;

            int i = 0, mult = multiList[0];

            while (mult > 1)
            {
                if ((mult & 1) == 1)  // is mult odd ?
                {
                    primeList[len++] = primeList[i];
                }

                multiList[i++] = mult >> 1;
                mult = multiList[i];
            }

            return XMath.Product(primeList, i, len - i)  
                  * XInt.Pow(NestedSquare(i), 2);
        }

        private int PrimeFactors(int n)
        {
            var sieve = new PrimeSieve(n);
            var primes = sieve.GetPrimeCollection(3, n);

            int maxBound = n / 2, count = 0;

            foreach (int prime in primes)
            {
                int m = prime > maxBound ? 1 : 0;

                if (prime <= maxBound)
                {
                    int q = n;
                    while (q >= prime)
                    {
                        m += q /= prime;
                    }
                }

                primeList[count] = prime;
                multiList[count++] = m;
            }
            return count;
        }
    }
}   // endOfFactorialPrimeSchoenhage

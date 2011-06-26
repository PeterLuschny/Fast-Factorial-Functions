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

    public class PrimeSwingList : IFactorialFunction 
    {
        public PrimeSwingList() { }

        private int[][] primeList;
        private int[] listLength;
        private int[] tower;
        private int[] bound;

        public string Name
        {
            get { return "PrimeSwingList      "; }
        }

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            int log2n = XMath.FloorLog2(n);
            int j = log2n;
            int hN = n;

            primeList = new int[log2n][];
            listLength = new int[log2n];
            bound = new int[log2n];
            tower = new int[log2n + 1];

            while (true)
            {
                tower[j] = hN;
                if (hN == 1) break;
                bound[--j] = hN / 3;
                int pLen = hN < 4 ? 6 : (int)(2.0 * (XMath.FloorSqrt(hN)
                         + (double) hN / (XMath.Log2(hN) - 1)));
                primeList[j] = new int[pLen];
                hN >>= 1;
            }

            tower[0] = 2;
            PrimeFactors(n);

            int init = listLength[0] == 0 ? 1 : 3;
            var oddFactorial = new XInt(init);
            
            for (int i = 1; i < log2n; i++)
            {
                oddFactorial = XInt.Pow(oddFactorial, 2)
                    * XMath.Product(primeList[i], 0, listLength[i]);
            }
            return oddFactorial << (n - XMath.BitCount(n));
        }

        private void PrimeFactors(int n)
        {
            int maxBound = n / 3;
            int lastList = primeList.Length - 1;
            int start = tower[1] == 2 ? 1 : 0;
            var sieve = new PrimeSieve(n);

            for (int section = start; section < primeList.Length; section++)
            {
                var primes = sieve.GetPrimeCollection(tower[section] + 1, tower[section + 1]);

                foreach (int prime in primes)
                {
                    primeList[section][listLength[section]++] = prime;
                    if (prime > maxBound) continue;

                    int np = n;
                    do
                    {
                        int k = lastList;
                        int q = np /= prime;

                        do if ((q & 1) == 1) //if q is odd
                            {
                                primeList[k][listLength[k]++] = prime;
                            }
                        while (((q >>= 1) > 0) && (prime <= bound[--k]));

                    } while (prime <= np);
                }
            }
        }
    }
}  // endOfFactorialPrimeSwingList

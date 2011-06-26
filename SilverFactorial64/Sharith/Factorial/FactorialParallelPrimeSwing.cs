// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

// Same algorithm as PrimeSwing
// but computes swing(n) concurrently

#if(MPIR)
namespace SharithMP.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;
#else
    namespace Sharith.Math.Factorial {
    using XInt = System.Numerics.BigInteger;
#endif
    using System;
    using System.Threading.Tasks;
    using Sharith.Math.Primes;
    using XMath = Sharith.Math.MathUtils.XMath;

    public class ParallelPrimeSwing : IFactorialFunction 
    {
        public ParallelPrimeSwing() { }

        public string Name
        {
            get { return "ParallelPrimeSwing       "; }
        }

        private const int SMALLSWING = 32;
        private IAsyncResult[] results;
        private delegate XInt SwingDelegate(PrimeSieve sieve, int n);
        private SwingDelegate swingDelegate;
        private int taskCounter;

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            var sieve = new PrimeSieve(n);
            results = new IAsyncResult[XMath.FloorLog2(n)];
            swingDelegate = Swing; taskCounter = 0;
            int N = n;

            // -- It is more efficient to add the big swings
            // -- first and the small ones later!

            while (N > SMALLSWING)
            {
                results[taskCounter++] = swingDelegate.BeginInvoke(sieve, N, null, null);
                N >>= 1;
            }

            return RecFactorial(n) << (n - XMath.BitCount(n));
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            XInt recFact = RecFactorial(n / 2);
            XInt sqrFact = XInt.Pow(recFact, 2);
            XInt swing;

            if (n <= SMALLSWING)
            {
                swing = smallOddSwing[n];
            }
            else
            {
                swing = swingDelegate.EndInvoke(results[--taskCounter]);
            }
            return sqrFact * swing;
        }

        private static XInt Swing(PrimeSieve sieve, int n)
        {
            var primorial = Task.Factory.StartNew<XInt>(() =>
            { 
                return sieve.GetPrimorial(n / 2 + 1, n); 
            });

            int count = 0, rootN = XMath.FloorSqrt(n);

            var aPrimes = sieve.GetPrimeCollection(3, rootN);
            var bPrimes = sieve.GetPrimeCollection(rootN + 1, n / 3);

            int[] primeList = new int[aPrimes.NumberOfPrimes + bPrimes.NumberOfPrimes];

            foreach (int prime in aPrimes)
            {
                int q = n, p = 1;

                while ((q /= prime) > 0)
                {
                    if ((q & 1) == 1)
                    {
                        p *= prime;
                    }
                }

                if (p > 1)
                {
                    primeList[count++] = p;
                }
            }

            foreach (int prime in bPrimes)
            {
                if (((n / prime) & 1) == 1)
                {
                    primeList[count++] = prime;
                }
            }

            XInt primeProduct = XMath.Product(primeList, 0, count);
            return primeProduct * primorial.Result;
        }

        private static XInt[] smallOddSwing = {
            1,1,1,3,3,15,5,35,35,315,63,693,231,3003,429,6435,6435,109395,
            12155,230945,46189,969969,88179,2028117,676039,16900975,1300075,
            35102025,5014575,145422675,9694845,300540195,300540195 };
    }
}

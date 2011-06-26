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
    using System;
    using System.Threading.Tasks;
    using Sharith.Math.Primes;
    using XMath = Sharith.Math.MathUtils.XMath;
    
    public class ParallelPrimeSplit : IFactorialFunction 
    {
        public ParallelPrimeSplit() { }
        
        private PrimeSieve sieve;
 
        public string Name
        {
            get { return "ParallelPrimeSplit  "; }
        }    

        private delegate XInt SwingDelegate(int n);
 
        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }
 
            sieve = new PrimeSieve(n);
            int log2n = XMath.FloorLog2(n);

            SwingDelegate swingDelegate = Swing;
            var results = new IAsyncResult[log2n];
 
            int h = 0, shift = 0, taskCounter = 0;
 
            // -- It is more efficient to add the big intervals
            // -- first and the small ones later!
            while (h != n)
            {
                shift += h;
                h = n >> log2n--;
                if (h > 2)
                {
                    results[taskCounter++] = swingDelegate.BeginInvoke(h, null, null);
                }
            }
 
            XInt p = XInt.One, r = XInt.One, rl = XInt.One;
 
            for (int i = 0; i < taskCounter; i++)
            {
                var t = rl * swingDelegate.EndInvoke(results[i]);
                p = p * t;
                rl = r;
                r = r * p;
            }
 
            return r << shift;
        }
 
        private XInt Swing(int n)
        {
            if (n < 33) return smallOddSwing[n];

            var primorial = Task.Factory.StartNew<XInt>(() =>
            { 
                return sieve.GetPrimorial(n / 2 + 1, n); 
            });
 
            int count = 0, rootN = XMath.FloorSqrt(n);
 
            var aPrimes = sieve.GetPrimeCollection(3, rootN);
            var bPrimes = sieve.GetPrimeCollection(rootN + 1, n / 3);
            int piN = aPrimes.NumberOfPrimes + bPrimes.NumberOfPrimes;
            var primeList = new int[piN];
 
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

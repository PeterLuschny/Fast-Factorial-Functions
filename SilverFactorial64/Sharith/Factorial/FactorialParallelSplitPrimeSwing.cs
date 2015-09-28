// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

// Same algorithm as PrimeSwing
// but computes swing(n) asynchronous.

namespace Sharith.Factorial
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Sharith.Primes;

    using XInt = Sharith.Arithmetic.XInt;
    using XMath = Sharith.MathUtils.XMath;

    public class ParallelSplitPrimeSwing : IFactorialFunction
    {
        public string Name => "ParallelSplitPrimeSwing  ";

        private const int Smallswing = 65;
        private IAsyncResult[] results2, results3;
        private delegate XInt SwingDelegate(PrimeSieve sieve, int n);
        private SwingDelegate swingDelegate2, swingDelegate3;
        private int taskCounter2, taskCounter3;

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            var sieve = new PrimeSieve(n);

            var task2 = Task.Factory.StartNew<XInt>(() =>
            {
                this.results2 = new IAsyncResult[XMath.FloorLog2(n)];
                this.swingDelegate2 = Swing2;
                this.taskCounter2 = 0;

                var N = n;

                // -- It is more efficient to add the big swings
                // -- first and the small ones later!
                while (N >= Smallswing)
                {
                    this.results2[this.taskCounter2++] = this.swingDelegate2.BeginInvoke(sieve, N, null, null);
                    N >>= 1;
                }
                return this.RecFactorial2(n);
            });

            this.results3 = new IAsyncResult[XMath.FloorLog2(n)];
            this.swingDelegate3 = Swing3;
            this.taskCounter3 = 0;

            var m = n;

            // -- It is more efficient to add the big swings
            // -- first and the small ones later!
            while (m >= Smallswing)
            {
                this.results3[this.taskCounter3++] = this.swingDelegate3.BeginInvoke(sieve, m, null, null);
                m >>= 1;
            }

            var task3Result = this.RecFactorial3(n);

            return (task2.Result * task3Result) << (n - XMath.BitCount(n));
        }

        private XInt RecFactorial2(int n)
        {
            if (n < 2) return XInt.One;

            var recFact = this.RecFactorial2(n / 2);
            var sqrFact = XInt.Pow(recFact, 2);
            var swing = n < Smallswing 
                      ? SmallOddSwing2[n] 
                      : this.swingDelegate2.EndInvoke(this.results2[--this.taskCounter2]);

            return sqrFact * swing;
        }

        private XInt RecFactorial3(int n)
        {
            if (n < 2) return XInt.One;

            var recFact = this.RecFactorial3(n / 2);
            var sqrFact = XInt.Pow(recFact, 2);
            var swing = n < Smallswing 
                      ? SmallOddSwing3[n] 
                      : this.swingDelegate3.EndInvoke(this.results3[--this.taskCounter3]);

            return sqrFact * swing;
        }

        private static XInt Swing2(PrimeSieve sieve, int n)
        {
            var primorial = Task.Factory.StartNew<XInt>(() =>
            {
                var start = sieve.NextPrime(n / 2);
                return sieve.GetPrimorial(start, n, 2);
            });

            var count = 0;
            var rootN = XMath.FloorSqrt(n);
            var startPrime = sieve.NextPrime(rootN);
            var aPrimes = sieve.GetPrimeCollectionEveryOther(3, rootN);
            var bPrimes = sieve.GetPrimeCollectionEveryOther(startPrime, n / 3);

            var primeList = new int[aPrimes.NumberOfPrimes + bPrimes.NumberOfPrimes];

            foreach (var prime in aPrimes)
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

            foreach (var prime in bPrimes.Where(prime => ((n / prime) & 1) == 1))
            {
                primeList[count++] = prime;
            }

            var primeProduct = XMath.Product(primeList, 0, count);
            return primeProduct * primorial.Result;
        }

        private static XInt Swing3(PrimeSieve sieve, int n)
        {
            var primorial = Task.Factory.StartNew<XInt>(() =>
            {
                var start = sieve.NextPrime(n / 2);
                start = sieve.NextPrime(start);
                return sieve.GetPrimorial(start, n, 2);
            });

            var count = 0;
            var rootN = XMath.FloorSqrt(n);
            var startPrime = sieve.NextPrime(rootN);
            startPrime = sieve.NextPrime(startPrime);
            var aPrimes = sieve.GetPrimeCollectionEveryOther(5, rootN);
            var bPrimes = sieve.GetPrimeCollectionEveryOther(startPrime, n / 3);
            var primeList = new int[aPrimes.NumberOfPrimes + bPrimes.NumberOfPrimes];

            foreach (var prime in aPrimes)
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

            foreach (var prime in bPrimes.Where(prime => ((n / prime) & 1) == 1))
            {
                primeList[count++] = prime;
            }

            var primeProduct = XMath.Product(primeList, 0, count);
            return primeProduct * primorial.Result;
        }

        static readonly XInt[] SmallOddSwing2 = {
        1, 1, 1, 1, 1, 3, 1, 5, 5, 15, 7, 21, 11, 39, 13, 65, 65, 255, 85, 323, 209, 969, 273, 
        1311, 437, 4025, 805, 5865, 2185, 11799, 2907, 17081, 17081, 97185, 24035, 141075, 45885, 
        289275, 65527, 413105, 185185, 1187145, 250305, 1687191, 502541, 3420261, 710489, 4892325, 
        2819295, 19858475, 3947013, 28190929, 7822431, 56904351, 10967873, 81607015, 30886943, 
        232469713, 43320981, 332436405, 85387379, 670707103, 120555703, 956908799, 956908799 };

        static readonly XInt[] SmallOddSwing3 = {
        1, 1, 1, 3, 3, 5, 5, 7, 7, 21, 9, 33, 21, 77, 33, 99, 99, 429, 143, 715, 221, 1001, 323, 
        1547, 1547, 4199, 1615, 5985, 2295, 12325, 3335, 17595, 17595, 102051, 24273, 144739, 49445,
        290191, 67425, 417105, 186093, 1190189, 268801, 1714765, 523365, 3460425, 724275, 4943601, 
        2859545, 19892421, 4003363, 28586061, 7924623, 57736539, 11094559, 82010159, 30954385, 
        234425895, 43378615, 333516557, 86564741, 672251293, 120646603, 957575133, 957575133 };
    }
}

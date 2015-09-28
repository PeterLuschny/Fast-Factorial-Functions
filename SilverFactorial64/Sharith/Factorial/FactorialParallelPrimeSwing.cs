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

    using XInt = Arithmetic.XInt;
    using XMath = MathUtils.XMath;

    public class ParallelPrimeSwing : IFactorialFunction 
    {
        public string Name => "ParallelPrimeSwing       ";

        const int Smallswing = 65;
        IAsyncResult[] results;
        delegate XInt SwingDelegate(PrimeSieve sieve, int n);
        SwingDelegate swingDelegate;
        int taskCounter;

        public XInt Factorial(int n)
        {
            if (n < 20) { return XMath.Factorial(n); }

            var sieve = new PrimeSieve(n);
            this.results = new IAsyncResult[XMath.FloorLog2(n)];
            this.swingDelegate = Swing; this.taskCounter = 0;
            var N = n;

            // -- It is more efficient to add the big swings
            // -- first and the small ones later!
            while (N >= Smallswing)
            {
                this.results[this.taskCounter++] = this.swingDelegate.BeginInvoke(sieve, N, null, null);
                N >>= 1;
            }

            return this.RecFactorial(n) << (n - XMath.BitCount(n));
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            var recFact = this.RecFactorial(n / 2);
            var sqrFact = XInt.Pow(recFact, 2);

            var swing = n < Smallswing 
                      ? SmallOddSwing[n] 
                      : this.swingDelegate.EndInvoke(this.results[--this.taskCounter]);

            return sqrFact * swing;
        }

        private static XInt Swing(PrimeSieve sieve, int n)
        {
            var primorial = Task.Factory.StartNew<XInt>(() => sieve.GetPrimorial(n / 2 + 1, n));
            var count = 0;
            var rootN = XMath.FloorSqrt(n);
            var aPrimes = sieve.GetPrimeCollection(3, rootN);
            var bPrimes = sieve.GetPrimeCollection(rootN + 1, n / 3);

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

        static readonly XInt[] SmallOddSwing = {
            1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231, 3003, 429, 6435, 6435,
            109395, 12155, 230945, 46189, 969969, 88179, 2028117, 676039, 16900975,
            1300075, 35102025, 5014575, 145422675, 9694845, 300540195, 300540195,
            9917826435, 583401555, 20419054425, 2268783825, 83945001525, 4418157975,
            172308161025, 34461632205, 1412926920405, 67282234305, 2893136075115,
            263012370465, 11835556670925, 514589420475, 24185702762325, 8061900920775,
            395033145117975, 15801325804719, 805867616040669, 61989816618513,
            3285460280781189, 121683714103007, 6692604275665385, 956086325095055,
            54496920530418135, 1879204156221315, 110873045217057585, 7391536347803839,
            450883717216034179, 14544636039226909, 916312070471295267, 916312070471295267 };
    }
}

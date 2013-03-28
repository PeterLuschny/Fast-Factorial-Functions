/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

/// Same algorithm as Split
/// but computing products using tasks.

namespace Sharith.Math.Factorial 
{
    using System;
    using System.Threading.Tasks;
    using XInt = Sharith.Arithmetic.XInt;
    using XMath = Sharith.Math.MathUtils.XMath;

    public class ParallelSplit : IFactorialFunction 
    {
        public ParallelSplit() { }

        public string Name
        {
            get { return "ParallelSplit       "; }
        }                
 
        private delegate XInt ProductDelegate(int from, int to);
 
        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    "n", Name + ": n >= 0 required, but was " + n);
            }
 
            if (n < 2) return XInt.One;
 
            int log2n = XMath.FloorLog2(n);
            ProductDelegate prodDelegate = Product;
            var results = new IAsyncResult[log2n];
 
            int high = n, low = n >> 1, shift = low, taskCounter = 0;
 
            // -- It is more efficient to add the big intervals
            // -- first and the small ones later!
            while ((low + 1) < high)
            {
                results[taskCounter++] = prodDelegate.BeginInvoke(low + 1, high, null, null);
                high = low;
                low >>= 1;
                shift += low;
            }
 
            XInt p = XInt.One, r = XInt.One;
            while (--taskCounter >= 0)
            {
                var R = Task.Factory.StartNew<XInt>(() => { return r * p; });
                var t = p * prodDelegate.EndInvoke(results[taskCounter]);
                r = R.Result;
                p = t;
            }

            return (r * p) << shift;
        }
 
        private static XInt Product(int n, int m)
        {
            n = n | 1;       // Round n up to the next odd number
            m = (m - 1) | 1; // Round m down to the next odd number
 
            if (m == n)
            {
                return new XInt(m);
            }

            if (m == (n + 2))
            {
                return new XInt((long)n * m);
            }
 
            int k = (n + m) >> 1;
            return Product(n, k) * Product(k + 1, m);
        }
    }
}

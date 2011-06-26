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
    using XMath = Sharith.Math.MathUtils.XMath;

    public class Split : IFactorialFunction 
    {
        public Split() { }

        public string Name
        {
            get { return "Split               "; }
        }

        private XInt currentN;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            if (n < 2) return XInt.One;

            XInt p = XInt.One;
            XInt r = XInt.One;
            currentN = XInt.One;

            int h = 0, shift = 0, high = 1;
            int log2n = XMath.FloorLog2(n);

            while (h != n)
            {
                shift += h;
                h = n >> log2n--;
                int len = high;
                high = (h - 1) | 1;
                len = (high - len) / 2;

                if (len > 0)
                {
                    p *= Product(len);
                    r *= p;
                }
            }

            return r << shift;
        }

        private XInt Product(int n)
        {
            int m = n / 2;
            if (m == 0) return currentN += 2;
            if (n == 2) return (currentN += 2) * (currentN += 2);
            return Product(n - m) * Product(m);
        }
    }

} // endOfFactorialBinSplit

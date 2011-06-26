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

    public class SwingDouble : IFactorialFunction  
    {
        public SwingDouble() { }

        public string Name
        {
            get { return "SwingDouble         "; }
        }

        private XInt f;
        private long gN;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            gN = 1;
            f = XInt.One;
            return RecFactorial(n);
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            return XInt.Pow(RecFactorial(n / 2),2) * Swing(n);
        }

        private XInt Swing(long n)
        {
            long s = gN - 1 + ((n - gN + 1) % 4);
            bool oddN = (gN & 1) != 1;

            for (; gN <= s; gN++)
            {
                if (oddN = !oddN) f *= gN;
                else f = (f * 4) / gN;
            }

            if (oddN) for (; gN <= n; gN += 4)
            {
                    long m = ((gN + 1) * (gN + 3)) << 1;
                    long d = (gN * (gN + 2)) >> 3;

                    f = (f * m) / d;
            }
            else for (; gN <= n; gN += 4)
            {
                    long m = (gN * (gN + 2)) << 1;
                    long d = ((gN + 1) * (gN + 3)) >> 3;

                    f = (f * m) / d;
             }

            return f;
        }
    }
} // endOfSwingDouble

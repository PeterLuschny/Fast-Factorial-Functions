// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2004-03-01

/////////////////////////////////
/// buggy for large values of n
/////////////////////////////////

#if(MPIR)
    namespace SharithMP.Math.Factorial {
    using XInt = Sharith.Arithmetic.XInt;
#else
    namespace Sharith.Math.Factorial {
    using XInt = System.Numerics.BigInteger;
#endif
    
    public class Hyper : IFactorialFunction 
    {
        public Hyper() { }

        public string Name
        {
            get { return "Hyper               "; }
        }

        private bool nostart;
        private long s, k, a;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            nostart = false;
            int h = n / 2;
            s = h + 1;
            k = s + h;
            a = (n & 1) == 1 ? k : 1;
            if ((h & 1) == 1) a = -a;
            k += 4;

            return HyperFact(h + 1) << h;
        }

        private XInt HyperFact(int l)
        {
            if (l > 1)
            {
                int m = l >> 1;
                return HyperFact(m) * HyperFact(l - m);
            }

            if (nostart)
            {
                s -= k -= 4;
                return (XInt) s;
            }
            else
            {
                nostart = true;
                return (XInt) a;
            }
        }
    }
} // endOfFactorialHyper

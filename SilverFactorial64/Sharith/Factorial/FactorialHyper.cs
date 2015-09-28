// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2004-03-01

/////////////////////////////////
//// buggy for large values of n
/////////////////////////////////
namespace Sharith.Factorial
{
    using XInt = Arithmetic.XInt;

    public class Hyper : IFactorialFunction
    {
        public string Name => "Hyper               ";

        bool nostart;
        long s, k, a;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            this.nostart = false;
            var h = n / 2;
            this.s = h + 1;
            this.k = this.s + h;
            this.a = (n & 1) == 1 ? this.k : 1;
            if ((h & 1) == 1) this.a = -this.a;
            this.k += 4;

            return this.HyperFact(h + 1) << h;
        }

        private XInt HyperFact(int l)
        {
            if (l > 1)
            {
                var m = l >> 1;
                return this.HyperFact(m) * this.HyperFact(l - m);
            }

            if (this.nostart)
            {
                this.s -= this.k -= 4;
                return (XInt)this.s;
            }
            this.nostart = true;
            return (XInt)this.a;
        }
    }
} // endOfFactorialHyper

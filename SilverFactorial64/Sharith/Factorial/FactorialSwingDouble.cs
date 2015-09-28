// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial 
{
    using XInt = Arithmetic.XInt;

    public class SwingDouble : IFactorialFunction  
    {
        public string Name => "SwingDouble         ";

        XInt f;
        long gN;

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    this.Name + ": " + nameof(n) + " >= 0 required, but was " + n);
            }

            this.gN = 1;
            this.f = XInt.One;
            return this.RecFactorial(n);
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            return XInt.Pow(this.RecFactorial(n / 2),2) * this.Swing(n);
        }

        private XInt Swing(long n)
        {
            var s = this.gN - 1 + ((n - this.gN + 1) % 4);
            bool oddN = (this.gN & 1) != 1;

            for (; this.gN <= s; this.gN++)
            {
                if (oddN = !oddN) this.f *= this.gN;
                else this.f = (this.f * 4) / this.gN;
            }

            if (oddN) for (; this.gN <= n; this.gN += 4)
            {
                    var m = ((this.gN + 1) * (this.gN + 3)) << 1;
                    var d = (this.gN * (this.gN + 2)) >> 3;

                    this.f = (this.f * m) / d;
            }
            else for (; this.gN <= n; this.gN += 4)
            {
                    var m = (this.gN * (this.gN + 2)) << 1;
                    var d = ((this.gN + 1) * (this.gN + 3)) >> 3;

                    this.f = (this.f * m) / d;
             }

            return this.f;
        }
    }
} // endOfSwingDouble

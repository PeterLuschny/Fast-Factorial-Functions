/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

#if(MPIR)
namespace SharithMP.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;
#else
    namespace Sharith.Math.Factorial {
    using XInt = System.Numerics.BigInteger;
#endif

    public class SwingSimple : IFactorialFunction  
    {
        public SwingSimple() { }
        
        public string Name
        {
            get { return "SwingSimple         "; }
        }

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                Name + ": n >= 0 required, but was " + n);
            }

            return RecFactorial(n);
        }

        private XInt RecFactorial(int n)
        {
            if (n < 2) return XInt.One;

            return XInt.Pow(RecFactorial(n / 2), 2) * Swing(n);
        }

        private static XInt Swing(int n)
        {
            int z;

            switch (n % 4)
            {
                case 1: z = n / 2 + 1; break;
                case 2: z = 2; break;
                case 3: z = 2 * (n / 2 + 2); break;
                default: z = 1; break;
            }

            var b = new XInt(z);
            z = 2 * (n - ((n + 1) & 1));

            for (int i = 1; i <= n / 4; i++, z -= 4)
            {
                b = (b * z) / i;
            }

            return b;
        }
    }
} // endOfFactorialSwingSimple

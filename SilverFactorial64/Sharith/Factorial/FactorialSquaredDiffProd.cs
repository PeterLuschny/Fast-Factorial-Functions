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
    using XMath = Sharith.Math.MathUtils.XMath;

    public class SquaredDiffProd : IFactorialFunction 
    {
        public SquaredDiffProd() { }

        public string Name
        {
            get { return "SquaredDiffProduct  "; }
        }                

        public XInt Factorial(int n)
        {
            if (n < 0)
            {
                throw new System.ArgumentOutOfRangeException("n",
                    Name + ": n >= 0 required, but was " + n);
            }

            if (n < 4)
            {
                return new XInt(n < 2 ? 1 : n == 2 ? 2 : 6);
            }

            long h = n / 2;
            long q = h * h;
            var f = new long[(int)h];
            f[0] = (n & 1) == 1 ? 2 * q * n : 2 * q;
            int i = 1;

            for (int d = 1; d < n - 2; d += 2)
            {
                f[i++] = q -= d;
            }

            return XMath.Product(f, f.Length);
        }
    }
} //endOfFactorialSquaredDiffProd

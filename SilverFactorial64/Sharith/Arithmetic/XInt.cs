// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

#if(MPIR)

namespace Sharith.Arithmetic
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Wrapper for MPIR's mpz_t type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct mpz_t
    {
        internal Int32 _mp_alloc;
        internal Int32 _mp_size;
        internal IntPtr ptr;
    }

    /// <summary>
    /// Mini-Wrapper for some of MPIR's BigInt functions.
    /// </summary>
    public class XInt
    {
        private mpz_t impl;

        public static readonly XInt One = new XInt(1);
        public static readonly XInt Zero = new XInt(0);
        public static readonly XInt MinusOne = new XInt(-1);

        ~XInt()
        {
            mpz_clear(ref impl);
        }

        public object Clone()
        {
            return new XInt(this);
        }

        public XInt()
        {
            mpz_init(ref impl);
        }

        public XInt(Int32 value)
        {
            mpz_init_set_si(ref impl, value);
        }

        public XInt(UInt32 value)
        {
            mpz_init_set_ui(ref impl, value);
        }

        public XInt(Int64 value)
        {
            if (value > 0)
            {
                if (value <= UInt32.MaxValue)
                {
                    mpz_init_set_ui(ref this.impl, (UInt32)value);
                }
                else
                {
                    mpz_init_set_ui(ref this.impl, (UInt32)(value >> 32));
                    mpz_mul_2exp(ref this.impl, ref this.impl, 32);
                    mpz_add_ui(ref this.impl, ref this.impl, (UInt32)value);
                }
            }
            else if (value == 0)
            {
                mpz_init(ref this.impl);
            }
            else // value < 0
            {
                Int64 absValue = (Int64)(-value);

                if (absValue <= UInt32.MaxValue)
                {
                    mpz_init_set_si(ref this.impl, (Int32)absValue);
                }
                else
                {
                    mpz_init_set_si(ref this.impl, (Int32)(absValue >> 32));
                    mpz_mul_2exp(ref this.impl, ref this.impl, 32);
                    mpz_add_ui(ref this.impl, ref this.impl, (UInt32)absValue);
                }
            }
        }

        public XInt(UInt64 value)
        {
            if (value == 0)
            {
                mpz_init(ref this.impl);
            }
            else
            {
                if (value <= UInt32.MaxValue)
                {
                    mpz_init_set_ui(ref this.impl, (UInt32)value);
                }
                else
                {
                    mpz_init_set_ui(ref this.impl, (UInt32)(value >> 32));
                    mpz_mul_2exp(ref this.impl, ref this.impl, 32);
                    mpz_add_ui(ref this.impl, ref this.impl, (UInt32)value);
                }
            }
        }

        public XInt(XInt value)
        {
            if (ReferenceEquals(value, null))
                throw new NullReferenceException();

            mpz_init(ref impl);
            mpz_set(ref impl, ref value.impl);
        }

        public XInt(String value)
        {
            // marshal string
            IntPtr unmanagedString = Marshal.StringToHGlobalAnsi(value);
            mpz_init_set_str(ref impl, unmanagedString, 10);
            // free unmanaged space
            Marshal.FreeHGlobal(unmanagedString);
        }

        public Int32 IntValue
        {
            get { return mpz_get_si(ref impl); }
            set { mpz_set_si(ref impl, value); }
        }

        public Int32 CompareTo(Int32 other)
        {
            return mpz_cmp_si(ref impl, other);
        }

        public Int32 CompareTo(XInt other)
        {
            return mpz_cmp(ref impl, ref other.impl);
        }

        public bool Equals(XInt other)
        {
            if (object.ReferenceEquals(other, null))
                return false;

            return mpz_cmp(ref other.impl, ref this.impl) == 0;
        }

        public static XInt operator %(XInt x, XInt mod)
        {
            var result = new XInt();
            mpz_mod(ref result.impl, ref x.impl, ref mod.impl);
            return result;
        }

        public static XInt operator *(XInt x, XInt y)
        {
            var result = new XInt();
            mpz_mul(ref result.impl, ref x.impl, ref y.impl);
            return result;
        }

        public static XInt operator *(Int32 x, XInt y)
        {
            var z = new XInt();
            mpz_mul_si(ref z.impl, ref y.impl, x);
            return z;
        }

        public static XInt operator *(XInt x, Int32 y)
        {
            var z = new XInt();
            mpz_mul_si(ref z.impl, ref x.impl, y);
            return z;
        }

        public static XInt operator *(UInt32 x, XInt y)
        {
            var z = new XInt();
            mpz_mul_ui(ref z.impl, ref y.impl, x);
            return z;
        }

        public static XInt operator *(XInt x, UInt32 y)
        {
            var z = new XInt();
            mpz_mul_ui(ref z.impl, ref x.impl, y);
            return z;
        }

        public static XInt operator +(XInt x, XInt y)
        {
            var result = new XInt();
            mpz_add(ref result.impl, ref x.impl, ref y.impl);
            return result;
        }

        public static XInt operator +(XInt x, UInt32 y)
        {
            var result = new XInt();
            mpz_add_ui(ref result.impl, ref x.impl,  y);
            return result;
        }

        public static XInt operator -(XInt x, XInt y)
        {
            var result = new XInt();
            mpz_sub(ref result.impl, ref x.impl, ref y.impl);
            return result;
        }

        public static XInt operator /(XInt x, XInt y)
        {
            var result = new XInt();
            mpz_tdiv_q(ref result.impl, ref x.impl, ref y.impl);
            return result;
        }

        public static XInt operator <<(XInt x, Int32 shiftAmount)
        {
            var z = new XInt();
            mpz_mul_2exp(ref z.impl, ref x.impl, (UInt32)shiftAmount);
            return z;
        }

        public static bool operator <(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) < 0;
        }

        public static bool operator >(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) > 0;
        }

        public static bool operator >=(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) >= 0;
        }

        public static bool operator <=(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) <= 0;
        }

        public static bool operator ==(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) == 0;
        }

        public static bool operator !=(XInt x, XInt y)
        {
            return mpz_cmp(ref x.impl, ref y.impl) != 0;
        }

        public static XInt operator ++(XInt op)
        {
            return op + One;
        }

        public static XInt operator --(XInt op)
        {
            return op + MinusOne;
        }

        public override Int32 GetHashCode()
        {
            Int32 hash = 0;
            Int32 count = this.impl._mp_size;
            count = count >= 0 ? count : -count;

            unsafe
            {
                Int32* p = (Int32*)this.impl.ptr;

                while (count-- > 0)
                {
                    hash ^= *p;
                    ++p;
                }
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null)) return false;
            if (this.GetType() != obj.GetType()) return false;
            XInt IntObj = (XInt)obj; 
            return mpz_cmp(ref IntObj.impl, ref this.impl) == 0;
        }

        public static XInt Sqrt(XInt i)
        {
            var result = new XInt();
            mpz_sqrt(ref result.impl, ref i.impl);
            return result;
        }

        public XInt Sqrt()
        {
            return Sqrt(this);
        }

        public static XInt Pow(XInt bas, UInt32 exp)
        {
            var result = new XInt();
            mpz_pow_ui(ref result.impl, ref bas.impl, exp);
            return result;
        }

        public static XInt Pow(XInt bas, Int32 exp)
        {
            if (exp == 2) return bas * bas;
            if (exp >= 0) return Pow(bas, (UInt32)exp);
            throw new ArgumentOutOfRangeException("exp");
        }

        public static XInt Pow(UInt32 bas, UInt32 exp)
        {
            var result = new XInt();
            mpz_ui_pow_ui(ref result.impl, bas, exp);
            return result;
        }

        public XInt PowMod(XInt exp, XInt mod)
        {
            var result = new XInt();
            mpz_powm(ref result.impl, ref this.impl, ref exp.impl, ref mod.impl);
            return result;
        }

        public XInt Pow(Int32 n)
        {
            return Pow(this, n);
        }

        public static XInt GreatestCommonDivisor(XInt x, XInt y)
        {
            var z = new XInt();
            mpz_gcd(ref z.impl, ref x.impl, ref y.impl);
            return z;
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString();
        }

        public Int32 SizeInBase(Int32 basis)
        {
            return mpz_sizeinbase(ref impl, basis);
        }

        public static implicit operator XInt(Int64 value)
        {
            return new XInt(value);
        }

        public static implicit operator XInt(UInt64 value)
        {
            return new XInt(value);
        }

        public static implicit operator XInt(Int32 value)
        {
            return new XInt(value);
        }

        public static implicit operator XInt(UInt32 value)
        {
            return new XInt(value);
        }

        public static XInt Factorial(Int32 x)
        {
            XInt z = new XInt();
            mpz_fac_ui(ref z.impl, (UInt32)x);
            return z;
        }
        
        #region DLL imports

        private const string mpir = "mpir.dll";

        [DllImport(mpir, EntryPoint = "__gmpz_init")]
        private static extern void mpz_init(ref mpz_t value);

        [DllImport(mpir, EntryPoint = "__gmpz_init_set_si")]
        private static extern void mpz_init_set_si(ref mpz_t value, Int32 v);

        [DllImport(mpir, EntryPoint = "__gmpz_init_set_ui")]
        private static extern void mpz_init_set_ui(ref mpz_t rop, UInt32 v);

        [DllImport(mpir, EntryPoint = "__gmpz_init_set_str")]
        private static extern Int32 mpz_init_set_str(ref mpz_t rop, IntPtr s, Int32 basis);

        [DllImport(mpir, EntryPoint = "__gmpz_clear")]
        private static extern void mpz_clear(ref mpz_t src);

        [DllImport(mpir, EntryPoint = "__gmpz_mul_si")]
        private static extern void mpz_mul_si(ref mpz_t dest, ref mpz_t src, Int32 val);

        [DllImport(mpir, EntryPoint = "__gmpz_mul_ui")]
        private static extern void mpz_mul_ui(ref mpz_t dest, ref mpz_t src, UInt32 val);

        [DllImport(mpir, EntryPoint = "__gmpz_mul")]
        private static extern void mpz_mul(ref mpz_t dest, ref mpz_t x, ref mpz_t y);

        [DllImport(mpir, EntryPoint = "__gmpz_add")]
        private static extern void mpz_add(ref mpz_t dest, ref mpz_t src, ref mpz_t src2);

        [DllImport(mpir, EntryPoint = "__gmpz_add_ui")]
        private static extern void mpz_add_ui(ref mpz_t dest, ref mpz_t src, UInt32 val);

        [DllImport(mpir, EntryPoint = "__gmpz_tdiv_q")]
        private static extern void mpz_tdiv_q(ref mpz_t dest, ref mpz_t src, ref mpz_t src2);

        [DllImport(mpir, EntryPoint = "__gmpz_set")]
        private static extern void mpz_set(ref mpz_t dest, ref mpz_t src);

        [DllImport(mpir, EntryPoint = "__gmpz_set_si")]
        private static extern void mpz_set_si(ref mpz_t src, Int32 value);

        [DllImport(mpir, EntryPoint = "__gmpz_set_str")]
        private static extern Int32 mpz_set_str(ref mpz_t rop, IntPtr s, Int32 sbase);

        [DllImport(mpir, EntryPoint = "__gmpz_get_si")]
        private static extern Int32 mpz_get_si(ref mpz_t src);

        [DllImport(mpir, EntryPoint = "__gmpz_get_d")]
        private static extern double mpz_get_d(ref mpz_t src);

        [DllImport(mpir, EntryPoint = "__gmpz_get_str", CharSet = CharSet.Ansi)]
        private static extern IntPtr mpz_get_str(IntPtr out_string, Int32 _base, ref mpz_t src);

        [DllImport(mpir, EntryPoint = "__gmpz_sizeinbase")]
        internal static extern Int32 mpz_sizeinbase(ref mpz_t src, Int32 _base);

        [DllImport(mpir, EntryPoint = "__gmpz_cmp")]
        private static extern Int32 mpz_cmp(ref mpz_t x, ref mpz_t y);

        [DllImport(mpir, EntryPoint = "__gmpz_cmp_d")]
        private static extern Int32 mpz_cmp_d(ref mpz_t x, double y);

        [DllImport(mpir, EntryPoint = "__gmpz_cmp_si")]
        private static extern Int32 mpz_cmp_si(ref mpz_t x, Int32 y);

        [DllImport(mpir, EntryPoint = "__gmpz_sub")]
        private static extern void mpz_sub(ref mpz_t rop, ref mpz_t x, ref mpz_t y);

        [DllImport(mpir, EntryPoint = "__gmpz_sqrt")]
        private static extern void mpz_sqrt(ref mpz_t rop, ref mpz_t op);

        [DllImport(mpir, EntryPoint = "__gmpz_pow_ui")]
        private static extern void mpz_pow_ui(ref mpz_t rop, ref mpz_t op, UInt32 exp);

        [DllImport(mpir, EntryPoint = "__gmpz_powm")]
        private static extern void mpz_powm(ref mpz_t rop, ref mpz_t bas, ref mpz_t exp, ref mpz_t mod);

        [DllImport(mpir, EntryPoint = "__gmpz_mod")]
        private static extern void mpz_mod(ref mpz_t rop, ref mpz_t x, ref mpz_t mod);

        [DllImport(mpir, EntryPoint = "__gmpz_gcd")]
        private static extern void mpz_gcd(ref mpz_t rop, ref mpz_t op1, ref mpz_t op2);

        [DllImport(mpir, EntryPoint = "__gmpz_ui_pow_ui")]
        private static extern void mpz_ui_pow_ui(ref mpz_t rop, UInt32 bas, UInt32 exp);

        [DllImport(mpir, EntryPoint = "__gmpz_invert")]
        private static extern Int32 mpz_invert(ref mpz_t rop, ref mpz_t x, ref mpz_t y);

        [DllImport(mpir, EntryPoint = "__gmpz_mul_2exp")]
        private static extern Int32 mpz_mul_2exp(ref mpz_t rop, ref mpz_t x, UInt32 shift);

        [DllImport(mpir, EntryPoint = "__gmpz_fac_ui")]
        private static extern void mpz_fac_ui(ref mpz_t rop, UInt32 op);

        #endregion

        private const string ffflib = "FastFactorialLibrary.dll";

        [DllImport(ffflib, EntryPoint = "PrimeSwing::ParallelFactorial")]
        private static extern void _PrimeSwingParallelFactorial(ref mpz_t fact, ulong n);

        [DllImport(ffflib, EntryPoint = "Schoenhage::ParallelFactorial")]
        private static extern void _SchoenhageParallelFactorial(ref mpz_t fact, ulong n);

        public static XInt PrimeSwingParallelFactorial(Int32 x)
        {
            XInt z = new XInt();
            _PrimeSwingParallelFactorial(ref z.impl, (UInt32)x);
            return z;
        }

        public static XInt SchoenhageParallelFactorial(Int32 x)
        {
            XInt z = new XInt();
            _SchoenhageParallelFactorial(ref z.impl, (UInt32)x);
            return z;
        }
    }
}

#endif

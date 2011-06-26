// This is a template.
// Just replace the body of "Factorial(int n)"  with your function, 
// compile and start benchmarking your solution. If you found a nice
// solution please contribute your solution.

#if(MPIR)
namespace SharithMP.Math.Factorial
{
    using XInt = Sharith.Arithmetic.XInt;
#else
    namespace Sharith.Math.Factorial {
    using XInt = System.Numerics.BigInteger;
#endif
    
    public class MyFactorial : IFactorialFunction
    {
        public MyFactorial() { }

        public string Name
        {
            get { return "MyFactorial        "; }
        }

        // --- Implement this function! ---
        public XInt Factorial(int n)
        {
            return new ParallelPrimeSwing().Factorial(n);
        }
    }
}   // endOfMyFactorial

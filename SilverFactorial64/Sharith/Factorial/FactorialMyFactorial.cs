// This is a template.
// Just replace the body of "public XInt Factorial(int n)" with your function,  
// compile and start benchmarking your solution. If you found a nice solution
// please share it and send it to me for inclusion.

namespace Sharith.Factorial
{
    using XInt = Sharith.Arithmetic.XInt;

    public class MyFactorial : IFactorialFunction
    {
        public string Name => "MyFactorial        ";

        // --- Implement this function! ---
        public XInt Factorial(int n)
        {
            return new ParallelPrimeSwing().Factorial(n);
        }
    }
} // endOfMyFactorial

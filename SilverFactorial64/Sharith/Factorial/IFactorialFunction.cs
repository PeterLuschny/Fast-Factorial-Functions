/// -------- ToujoursEnBeta
/// Author & Copyright : Peter Luschny
/// License: LGPL version 3.0 or (at your option)
/// Creative Commons Attribution-ShareAlike 3.0
/// Comments mail to: peter(at)luschny.de
/// Created: 2010-03-01

namespace Sharith.Math.Factorial 
{
    using XInt = Sharith.Arithmetic.XInt;

    /// <summary> An interface for the factorial function
    /// n! = 1*2*3*...*n
    /// for nonnegative integer values n.
    /// </summary>
    public interface IFactorialFunction
    {
        string Name { get; }
        XInt Factorial(int n);
    }

} // endOfIFactorialFunction

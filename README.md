Fast Factorial Functions
========================

Project status: maintained and up-to-date.

Here you can find implementations of the most efficient algorithms to compute

* the [factorial](http://en.wikipedia.org/wiki/Factorial) function,
* the [double factorial](http://en.wikipedia.org/wiki/Double_factorial#Double_factorial) function,
* the [binomial](http://en.wikipedia.org/wiki/Binomial) function.

See also this nice [overview](http://functions.wolfram.com/GammaBetaErf/Factorial2/introductions/FactorialBinomials/ShowAll.html).

Additionally you can find in this repository

* a small database of factorial algorithms which let you
  quickly find the most appropriate version for your needs.

* An efficient sieve of Eratosthenes implemented in an object-oriented way.


Implementation languages
------------------------

* Project MpirBasedFunctions : C++
* Project SilverFactorial64 : C#
* Project JavaFactorial : Java
* Project GoFactorial : Go
* Project PyFactorial : Python
* Project JFactorial : Julia

The C# and the Java version come with a small benchmark program.
Here a screenshot of the [Java](http://www.luschny.de/math/factorial/JavaFactorialBench.png) version
and a screenshot of the [C#](http://www.luschny.de/math/factorial/FastFactorial64.JPG) version.


Browsing the code
-----------------

To browse the code the following two pages might be more convenient: [primes](http://www.luschny.de/math/primes/PrimeSieveForJavaAndCsharp.html) and [factorials](http://www.luschny.de/math/factorial/index.html).


Porting
-------

If you want to port the algorithms to other languages the C# version is recommended as the point of departure.
The [benchmarks](http://www.luschny.de/math/factorial/Benchmark.html) indicate that it is a good idea to start with the [swing algorithm](http://www.luschny.de/math/factorial/csharp/FactorialSwing.cs.html)
or (more demanding to implement, but at least twice as fast) the [prime swing algorithm](http://www.luschny.de/math/factorial/csharp/FactorialPrimeSwing.cs.html).

A good starting point for the binomial function is [here](http://www.luschny.de/math/factorial/FastBinomialFunction.html).


Dependencies
------------

To build the sources you need for

* the C++ version the [MPIR](http://www.mpir.org) library.

* The C# version uses the [MPIR](http://www.mpir.org) library (an interop is provided).

* The Java version needs Mikko Tommila's [Apfloat](http://www.apfloat.org/apfloat_java)
library. If you want to compile the benchmark program additionally Karsten
Lentzsch's [JGoodies](http://www.jgoodies.com/downloads/libraries.html) is needed.


Contributing
------------

Please notify me of any bugs. Want to contribute new algorithms? Great, please contact me.
If you already ported to some other language (Scala, F#, Rust, Ruby, Lisp) then please send 
me your code so I can incorporate it into this repository.


Contributors
---------------

[Sonia Keys](http://soniacodes.wordpress.com) ported the algorithms to [Go](http://golang.org/).

msaupe contributed a performance optimized python3 version with gmpy2.

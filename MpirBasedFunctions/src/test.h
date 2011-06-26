// Author & Copyright : Peter Luschny
// Created: 2010-01-15
// License: LGPL version 2.1 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0

#ifndef TEST_H_
#define TEST_H_

#include <iostream>
#include "lmp.h"
#include "stopwatch.h"
#include "xmath.h"
#include "primeswing.h"
#include "schoenhage.h"
#include "binomial.h"

static ulong MemoryConsumption(ulong n, int type)
{
    ulong mem;

    if(type == 0)  // -- case swing
    {
        // -- case even swing
        if((n & 1) == 0) mem = 2*Xmath::NumberOfPrimes(n/2);
        // -- case odd swing
        else mem = 2*Xmath::NumberOfPrimes(n);
    }

    if(type == 1) // -- case Schoenhage/Zimmermann
    {
        mem = 2*n + Xmath::NumberOfPrimes(n);
    }

    std::cout << "memory: " << mem << " long " << std::endl;
    return mem;
}

class Test {
public:

    static void Test::FactorialSanityCheck(ulong limit)
    {
        Xint swing, paraswing, schoen, paraschoen, fact;
        lmp::InitSetUi(swing, 1);
        lmp::InitSetUi(paraswing, 1);
        lmp::InitSetUi(schoen, 1);
        lmp::InitSetUi(paraschoen, 1);
        lmp::InitSetUi(fact, 1);

        std::cout << std::endl << "SanityCheck" << std::endl;

        for (ulong n = 0; n < limit; n++)
        {
            std::cout << n << "," ;

            PrimeSwing::Factorial(swing, n);
            if (lmp::Cmp(fact, swing) != 0) {
                std::cout << "PrimeSwingFactorial failed: "
                    << n << std::endl; std::cin.get(); }

            PrimeSwing::ParallelFactorial(paraswing, n);
            if (lmp::Cmp(fact, paraswing) != 0) {
                std::cout << "ParallelPrimeSwing failed: "
                    << n << std::endl; std::cin.get(); }

            Schoenhage::Factorial(schoen, n);
            if (lmp::Cmp(fact, schoen) != 0) {
                std::cout << "SchoenhageFactorial failed: "
                    << n << std::endl; std::cin.get(); }

            Schoenhage::ParallelFactorial(paraschoen, n);
            if (lmp::Cmp(fact, paraschoen) != 0) {
                std::cout << "ParallelSchoenhage failed:  "
                    << n << std::endl; std::cin.get(); }

            lmp::MulUi(fact, fact, n + 1);  // (n+1)! = n! * (n+1)
        }

        std::cout << std::endl << "Done " << std::endl;

        lmp::Clear(swing);
        lmp::Clear(paraswing);
        lmp::Clear(schoen);
        lmp::Clear(paraschoen);
        lmp::Clear(fact);
    }

    static void Test::FactorialBenchmark(ulong n)
    {
        Xint swing, paraswing, schoen, paraschoen, fact;

        std::cout << std::endl << "Test   n =  "
            << n << std::endl;

        std::cout << "ParalSwing: ";
        {
            lmp::Init(paraswing); StopWatch::Start();
            PrimeSwing::ParallelFactorial(paraswing, n);
            StopWatch::ElapsedTime(); std::cout << std::endl;
            lmp::Clear(paraswing);
        }
        std::cout << "ParaSchoen: ";
        {
            lmp::Init(paraschoen); StopWatch::Start();
            Schoenhage::ParallelFactorial(paraschoen, n);
            StopWatch::ElapsedTime(); std::cout << std::endl;
            lmp::Clear(paraschoen);
        }
        std::cout << "PrimeSwing: ";
        {
            lmp::Init(swing); StopWatch::Start();
            PrimeSwing::Factorial(swing, n);
            StopWatch::ElapsedTime(); std::cout << std::endl;
            lmp::Clear(swing);
        }
        std::cout << "Schoenhage: ";
        {
            lmp::Init(schoen); StopWatch::Start();
            Schoenhage::Factorial(schoen, n);
            StopWatch::ElapsedTime(); std::cout << std::endl;
            lmp::Clear(schoen);
        }
        // Calculate factorial using the mp-library
        std::cout << "MP-library: ";
        {
            lmp::Init(fact); StopWatch::Start();
            lmp::FacUi(fact, n);
            StopWatch::ElapsedTime(); std::cout << std::endl;
            lmp::Clear(fact);
        }
    }

    static void DoubleFactorialSanityCheck(ulong limit)
    {
        Xint swing, paraswing, schoen, dblfact;
        lmp::InitSetUi(swing, 1);
        lmp::InitSetUi(paraswing, 1);
        lmp::InitSetUi(schoen, 1);
        lmp::InitSetUi(dblfact, 1);

        std::cout << std::endl << "SanityCheck" << std::endl;

        for (ulong n = 0; n < limit; n++)
        {
            std::cout << n << "," ;
			Xmath::NaiveDoubleFactorial(dblfact, n);

            PrimeSwing::ParallelDoubleFactorial(paraswing, n);
            if (lmp::Cmp(dblfact, paraswing) != 0) {
                std::cout << "ParallelDouble failed:   "
                    << n << std::endl; std::cin.get(); }

            PrimeSwing::DoubleFactorial(swing, n);
            if (lmp::Cmp(dblfact, swing) != 0) {
                std::cout << "SwingDouble failed:      "

                    << n << std::endl; std::cin.get(); }

            lmp::ZimmermannFacUi2(schoen, n);
            if (lmp::Cmp(dblfact, schoen) != 0) {
                std::cout << "ZimmermannDouble failed: "
                    << n << std::endl; std::cin.get(); }
        }

        std::cout << std::endl << "Done " << std::endl;

        lmp::Clear(swing);
        lmp::Clear(paraswing);
        lmp::Clear(schoen);
        lmp::Clear(dblfact);
    }

    static void DoubleFactorialBenchmark(ulong n)
    {
        Xint swing, paraswing, schoen;

        std::cout << std::endl << "Testing number:   "
            << n << std::endl;

        std::cout << "ParallelDouble:   ";
        {
            lmp::Init(paraswing); StopWatch::Start();
            PrimeSwing::ParallelDoubleFactorial(paraswing, n);
            StopWatch::ElapsedTime(); lmp::Clear(paraswing);
            MemoryConsumption(n, 0);
        }
        std::cout << "SwingDouble:      ";
        {
            lmp::Init(swing); StopWatch::Start();
            PrimeSwing::DoubleFactorial(swing, n);
            StopWatch::ElapsedTime(); lmp::Clear(swing);
            MemoryConsumption(n, 0);
        }
        std::cout << "ZimmermannDouble: ";
        {
            lmp::Init(schoen); StopWatch::Start();
            lmp::ZimmermannFacUi2(schoen, n);
            StopWatch::ElapsedTime(); lmp::Clear(schoen);
            MemoryConsumption(n, 1);
        }
    }

    static void BinomialSanityCheck(ulong limit)
    {
        Xint binom, parabinom, libbinom, naivebinom;
        lmp::InitSetUi(binom, 1);
        lmp::InitSetUi(parabinom, 1);
        lmp::InitSetUi(libbinom, 1);
        lmp::InitSetUi(naivebinom, 1);

        std::cout << std::endl << "SanityCheck" << std::endl;

        for (ulong n = 0; n < limit; n++)
        for (ulong k = 0; k <= n; k++)
        {
			Xmath::NaiveBinomial(naivebinom, n, k);
            std::cout << n << "," << k  << " " ;
            mpz_out_str(stdout, 10, naivebinom); 
            std::cout << std::endl;

            Binomial::ParallelBinomial(parabinom, n, k);
            if (lmp::Cmp(naivebinom, parabinom) != 0) {
                std::cout << "ParallelBinomial failed:   "
                    << n << std::endl; std::cin.get(); }

            Binomial::PrimeBinomial(binom, n, k);
            if (lmp::Cmp(naivebinom, binom) != 0) {
                std::cout << "PrimeBinomial failed:      "
                   << n << std::endl; std::cin.get(); }

            lmp::BinomialUiUi(libbinom, n, k);
            if (lmp::Cmp(naivebinom, libbinom) != 0) {
                std::cout << "LibraryBinom failed: "
                   << n << std::endl; std::cin.get(); }
        }

        std::cout << std::endl << "Done " << std::endl;

        lmp::Clear(binom);
        lmp::Clear(parabinom);
        lmp::Clear(libbinom);
        lmp::Clear(naivebinom);
    }

    static void BinomialBenchmark(ulong n, ulong k)
    {
        Xint binom, parabinom, libbinom, naivebinom;
        
        std::cout << std::endl << "Testing binomial: "
            << n << "," << k  << std::endl;

        lmp::Init(naivebinom);
		Xmath::NaiveBinomial(naivebinom, n, k);

        std::cout << "ParallelBinomial: ";
        {              
            lmp::Init(parabinom); StopWatch::Start();
            Binomial::ParallelBinomial(parabinom, n, k);
            StopWatch::ElapsedTime(); std::cout << std::endl;
            if (lmp::Cmp(naivebinom, parabinom) != 0) {
                std::cout << "ParallelBinomial failed:   "
                    << n << std::endl; std::cin.get(); }
            lmp::Clear(parabinom);
        }
        std::cout << "PrimeBinomial:    ";
        {            
            lmp::Init(binom); StopWatch::Start();
            Binomial::PrimeBinomial(binom, n, k);
            StopWatch::ElapsedTime(); std::cout << std::endl;
            if (lmp::Cmp(naivebinom, binom) != 0) {
                std::cout << "PrimeBinomial failed:      "
                   << n << std::endl; std::cin.get(); }
            lmp::Clear(binom);
        }
        std::cout << "LibraryBinomial:  ";
        {            
            lmp::Init(libbinom); StopWatch::Start();
            lmp::BinomialUiUi(libbinom, n, k);
            StopWatch::ElapsedTime(); std::cout << std::endl;
            if (lmp::Cmp(naivebinom, libbinom) != 0) {
                std::cout << "LibraryBinom failed: "
                   << n << std::endl; std::cin.get(); }
            lmp::Clear(libbinom);
        }

        lmp::Clear(naivebinom);
    }
};

#endif // TEST_H_

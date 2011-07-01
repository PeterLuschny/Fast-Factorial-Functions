// primes_test project primes_test.go
// 2010/6/29 Sonia Keys 
// 2011/6/29 edited Peter Luschny 
// MIT license 

package main

import (
	"fmt"
	"time"
	"../../obj/primes"
)

type sieveTest struct {
	in uint64
	out int
}

var sieveTests = []sieveTest{
	sieveTest{0, 0},
	sieveTest{1, 0},
	sieveTest{2, 2},
	sieveTest{3, 2},
	sieveTest{4, 2},
	sieveTest{5, 3},
	sieveTest{6, 3},
	sieveTest{7, 4},
	sieveTest{8, 4},
	sieveTest{100000000, 5761455},
//	sieveTest{4294967295, 203280221},
}

func TestSieve(test []sieveTest) {
	fmt.Println("Test running!")
	for _, t := range test {
	
		v := primes.NumberOfPrimesNotExceeding(t.in)
		
		if v == t.out {
			fmt.Printf("%d passed.\n", t.in)
		} else {
			fmt.Printf("in %d, out %d, want %d.", t.in, v, t.out)
			fmt.Println()
		}
	}
}

var sieveBenchmarks = []uint64{
	10000,
	100000,
	1000000,
	10000000,
	100000000,
	200000000,
//  4294967295,
}

func BenchmarkSieve(test []uint64) {
	fmt.Println()
	fmt.Println("Benchmark running!")
	for _, t := range test {
		start := time.Nanoseconds()
		
		primes.NumberOfPrimesNotExceeding(t)
		
		stop := time.Nanoseconds()
		fmt.Printf("Time for %d was %.2f sec\n", t, float64(stop-start)/1e9)
	}
}

func IteratorTest() {
	fmt.Println()
	fmt.Println("Iterator test running!")
	
	p := primes.Primes(100000000)
	
	//  -----------------
	
	count := p.NumberOfPrimes(1, 1000)
	fmt.Printf("[1,1000] includes %d primes.\n", count)
	
	count = p.NumberOfPrimes(99999000,100000000)
	fmt.Printf("[99999000,100000000] includes %d primes.\n", count)

	//  -----------------
	
	var sum uint64
	primesum := func(prime uint64) {
		sum += prime
	}
	
	p.IteratePrimes(1, 10, primesum)
	fmt.Printf("The sum of primes <= %d is %d.\n", 10, sum)
	
	//  -----------------
	
	bp := p.Primorial(1, 10)
	fmt.Printf("The promorial(%d) = %d.\n", 10, bp)
	
	bp = p.Primorial(100, 110)
	fmt.Printf("The product of primes p, %d <= p <= %d is %d.\n", 100, 110, bp)
	
	//  -----------------
	
	if p.IsPrime(4567) {
		fmt.Printf("4567 is prime.\n"); 
	}
		
	if ! p.IsPrime(1234567) {
		fmt.Printf("1234567 is not prime.\n");		
	}
}

func Tests() {
	TestSieve(sieveTests)
	BenchmarkSieve(sieveBenchmarks)	
	IteratorTest()
}

func main() {
	Tests()
}

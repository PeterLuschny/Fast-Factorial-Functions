// primes_test project primes_test.go
// 2010/6/29 Sonia Keys
// 2011/6/29 edited Peter Luschny
// MIT license

package primes

import (
	"fmt"
	"strconv"
	"testing"
)

var benchmarks = []uint64{
	10000,
	100000,
	1000000,
	10000000,
	100000000,
	200000000,
	//  4294967295,
}

func TestNumberOfPrimesNotExceeding(t *testing.T) {
	tests := []struct {
		input    uint64
		expected int
	}{
		{0, 0},
		{1, 0},
		{2, 2},
		{3, 2},
		{4, 2},
		{5, 3},
		{6, 3},
		{7, 4},
		{8, 4},
		{100000000, 5761455},
		//{4294967295, 203280221},
	}
	for ii, tt := range tests {
		t.Run(strconv.Itoa(ii), func(t *testing.T) {
			if gotR := NumberOfPrimesNotExceeding(tt.input); gotR != tt.expected {
				t.Errorf("NumberOfPrimesNotExceeding() = %v, want %v", gotR, tt.expected)
			}
		})
	}
}

func BenchmarkNumberOfPrimesNotExceeding(b *testing.B) {
	for _, benchmark := range benchmarks {
		b.Run(fmt.Sprintf("%d", benchmark), func(b *testing.B) {
			for j := 0; j < b.N; j++ {
				NumberOfPrimesNotExceeding(benchmark)
			}
		})
	}
}

func TestIterator(t *testing.T) {

	t.Log("Iterator test running!")

	p := Primes(100000000)

	//  -----------------

	count := p.NumberOfPrimes(1, 1000)
	t.Logf("[1,1000] includes %d primes.\n", count)

	count = p.NumberOfPrimes(99999000, 100000000)
	t.Logf("[99999000,100000000] includes %d primes.\n", count)

	//  -----------------

	var sum uint64
	primesum := func(prime uint64) {
		sum += prime
	}

	p.IteratePrimes(1, 10, primesum)
	t.Logf("The sum of primes <= %d is %d.\n", 10, sum)
	if sum != 2+3+5+7 {
		t.Errorf("expected %v but found %v", 2+3+5+7, sum)
	}

	//  -----------------

	bp := p.Primorial(1, 10)
	t.Logf("The promorial(%d) = %d.\n", 10, bp)

	bp = p.Primorial(100, 110)
	t.Logf("The product of primes p, %d <= p <= %d is %d.\n", 100, 110, bp)

	//  -----------------

	if p.IsPrime(4567) {
		t.Logf("4567 is prime.\n")
	} else {
		t.Errorf("expected true but found false")
	}

	if !p.IsPrime(1234567) {
		t.Logf("1234567 is not prime.\n")
	} else {
		t.Errorf("expected false but found true")
	}
}

// primes package primes.go
// 2010/6/29 Sonia Keys
// 2011/6/29 edited and extended by Peter Luschny
// MIT license

package primes

import (
	"math/big"
)

// word size dependent constants
const (
	bitsPerInt = 64
	mask       = bitsPerInt - 1
	log2Int    = 6
)

// holds completed sieve
// isComposite[0] ... isComposite[n] includes
// 5 <= sieve numbers <= ??*(n+1) + 1

type Sieve struct {
	SieveLen    uint64
	isComposite []uint64
}

// Prime number sieve, Eratosthenes (276-194 b.t. )
// This implementation considers only multiples of sieve
// greater than 3, so the smallest value has to be mapped to 5.
// Note: There is no multiplication operation in this function
// and no call to a sqrt function.

// constructor, completes sieve.
func Primes(n uint64) (s *Sieve) {

	s = new(Sieve)
	s.SieveLen = n

	if n < 965 {
		s.isComposite = []uint64{3644759964122252416,
			10782565419096678876, 5393006238418678630,
			7319957818701628715, 16892333181782511326}
		return
	}

	s.isComposite = make([]uint64, (n/(3*bitsPerInt))+1)

	var (
		d1, d2, p1, p2, s1, s2 uint64 = 8, 8, 3, 7, 7, 3
		l, c, max, inc         uint64 = 0, 1, n / 3, 0
		toggle                 bool
	)

	for s1 < max { // --  scan the sieve
		// --  if a prime is found ...
		if (s.isComposite[l>>log2Int] & (1 << (l & mask))) == 0 {
			inc = p1 + p2 // --  ... cancel its multiples

			// --  ... set c as composite
			for c = s1; c < max; c += inc {
				s.isComposite[c>>log2Int] |= 1 << (c & mask)
			}
			for c = s1 + s2; c < max; c += inc {
				s.isComposite[c>>log2Int] |= 1 << (c & mask)
			}
		}

		l++
		toggle = !toggle
		if toggle {
			s1 += d2
			d1 += 16
			p1 += 2
			p2 += 2
			s2 = p2
		} else {
			s1 += d1
			d2 += 8
			p1 += 2
			p2 += 6
			s2 = p1
		}
	}
	return
}

func (s *Sieve) IteratePrimes(min, max uint64, visitor func(prime uint64)) {

	if max > s.SieveLen {
		return // Max larger than sieve
	}
	if max < 2 {
		return
	}
	if min <= 2 {
		min = 2
		visitor(2)
	}
	if min <= 3 {
		visitor(3)
	}

	absPos := (min+(min+1)%2)/3 - 1
	index := absPos / bitsPerInt
	bitPos := absPos % bitsPerInt
	prime := 5 + 3*(bitsPerInt*index+bitPos) - (bitPos & 1)
	inc := bitPos&1*2 + 2

	for prime <= max {
		bitField := s.isComposite[index] >> bitPos
		index++
		for ; bitPos < bitsPerInt; bitPos++ {
			if (bitField & 1) == 0 {
				visitor(prime)
			}
			prime += inc
			if prime > max {
				return
			}
			inc = 6 - inc
			bitField >>= 1
		}
		bitPos = 0
	}
}

func NumberOfPrimesNotExceeding(n uint64) int {

	var count int
	visitor := func(prime uint64) {
		count = count + 1
	}

	p := Primes(n)
	p.IteratePrimes(1, n, visitor)

	return count
}

func (s *Sieve) NumberOfPrimes(low, high uint64) int {

	if high > s.SieveLen {
		panic("high bound not in the range of the sieve.")
	}

	var count int
	visitor := func(prime uint64) {
		count = count + 1
	}

	s.IteratePrimes(low, high, visitor)

	return count
}

func (s *Sieve) IsPrime(n uint64) bool {

	if n > s.SieveLen {
		panic("n not in the range of the sieve.")
	}

	var count int
	visitor := func(prime uint64) {
		count = count + 1
	}

	s.IteratePrimes(n, n, visitor)

	return count == 1
}

func (s *Sieve) Primorial(lo, hi uint64) *big.Int {

	if lo > hi {
		return big.NewInt(1)
	}

	if hi-lo < 200 {
		var r, t big.Int
		r.SetInt64(1)
		s.IteratePrimes(lo, hi, func(prime uint64) {
			r.Mul(&r, t.SetInt64(int64(prime)))
			return
		})
		return &r
	}

	h := (lo + hi) / 2
	r := s.Primorial(lo, h)

	return r.Mul(r, s.Primorial(h+1, hi))
}

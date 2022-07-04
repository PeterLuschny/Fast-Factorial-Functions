// swingfactorial package swingfactorial.go
// 2010/6/29 Sonia Keys
// 2011/6/29 edited by Peter Luschny
// MIT license

package swingfactorial

import (
	"math/big"

	"github.com/PeterLuschny/Fast-Factorial-Functions/GoFactorial/primes"
	"github.com/PeterLuschny/Fast-Factorial-Functions/GoFactorial/xmath"
)

type Swing struct {
	primes  *primes.Sieve
	factors []uint64
}

// constructor, completes Swing
func NewSwing(n uint64) (s *Swing) {
	s = new(Swing)
	s.primes = primes.Primes(n)

	if n >= uint64(len(smallOddSwing)) {
		s.factors = make([]uint64, n)
	}

	return s
}

// Computes A056040 on OEIS
func SwingingFactorial(n uint64) *big.Int {
	s := new(Swing)
	s.primes = primes.Primes(n)

	if n >= uint64(len(smallOddSwing)) {
		s.factors = make([]uint64, n)
	}

	r := s.OddSwing(n)
	return r.Lsh(r, xmath.BitCount64(n>>1))
}

func (s *Swing) OddSwing(k uint64) *big.Int {

	if k < uint64(len(smallOddSwing)) {
		return big.NewInt(smallOddSwing[k])
	}

	rootK := xmath.FloorSqrt(k)
	var i int

	s.primes.IteratePrimes(3, rootK, func(p uint64) {
		q := k / p
		for q > 0 {
			if q&1 == 1 {
				s.factors[i] = p
				i++
			}
			q /= p
		}
	})

	s.primes.IteratePrimes(rootK+1, k/3, func(p uint64) {
		if (k / p & 1) == 1 {
			s.factors[i] = p
			i++
		}
	})

	s.primes.IteratePrimes(k/2+1, k, func(p uint64) {
		s.factors[i] = p
		i++
	})

	return xmath.Product(s.factors[0:i])
}

var smallOddSwing []int64 = []int64{1, 1, 1, 3, 3, 15, 5,
	35, 35, 315, 63, 693, 231, 3003, 429, 6435, 6435,
	109395, 12155, 230945, 46189, 969969, 88179, 2028117, 676039,
	16900975, 1300075, 35102025, 5014575, 145422675, 9694845,
	300540195, 300540195, 9917826435, 583401555, 20419054425,
	2268783825, 83945001525, 4418157975, 172308161025,
	34461632205, 1412926920405, 67282234305, 2893136075115,
	263012370465, 11835556670925, 514589420475, 24185702762325,
	8061900920775, 395033145117975, 15801325804719,
	805867616040669, 61989816618513, 3285460280781189,
	121683714103007, 6692604275665385, 956086325095055,
	54496920530418135, 1879204156221315, 110873045217057585,
	7391536347803839, 450883717216034179, 14544636039226909,
	916312070471295267, 916312070471295267}

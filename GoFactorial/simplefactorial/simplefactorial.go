// simplefactorial package simplefactorial.go
// 2010/6/29 Sonia Keys
// 2011/6/29 edited Peter Luschny
// MIT license

package simplefactorial

import (
	"big"
	"../obj/xmath"
)

func Factorial(n uint64) (r *big.Int) {
	var oddFactNDiv2, oddFactNDiv4 big.Int

	// closes on oddFactNDiv2, oddFactNDiv4
	oddSwing := func(n uint64) (r *big.Int) {
		
		if n < uint64(len(smallOddSwing)) {
			return big.NewInt(smallOddSwing[n])
		}

		length := (n - 1) / 4
		if n % 4 != 2 {
			length++
		}
		high := n - (n + 1) & 1
		ndiv4 := n / 4

		var oddFact big.Int
		if ndiv4 < uint64(len(smallOddFactorial)) {
			oddFact.SetInt64(smallOddFactorial[ndiv4])
			r = &oddFact
		} else {
			r = &oddFactNDiv4
		}

		return oddFact.Quo(oddProduct(high, length), r)
	}

	// closes on oddFactNDiv2, oddFactNDiv4, oddSwing, and itself
	var oddFactorial func(uint64) *big.Int
	oddFactorial = func (n uint64) (oddFact *big.Int) {
		if n < uint64(len(smallOddFactorial)) {
			oddFact = big.NewInt(smallOddFactorial[n])
		} else {
			oddFact = oddFactorial(n / 2)
			oddFact.Mul(oddFact.Mul(oddFact, oddFact), oddSwing(n))
		}

		oddFactNDiv4.Set(&oddFactNDiv2)
		oddFactNDiv2.Set(oddFact)
		return oddFact
	}

	oddFactNDiv2.SetInt64(1)
	oddFactNDiv4.SetInt64(1)
	r = oddFactorial(n)
	exp := uint(n - uint64(xmath.BitCount64(n)))
	return r.Lsh(r, exp)
}

func oddProduct(m, length uint64) *big.Int {
	switch length {
		case 1:
			return big.NewInt(int64(m))
		case 2:
			var mb big.Int
			mb.SetInt64(int64(m))
			mb2 := big.NewInt(int64(m - 2))
			return mb2.Mul(&mb, mb2)
	}
	hlen := length / 2
	h := oddProduct(m - hlen*2, length - hlen)
	return h.Mul(h, oddProduct(m, hlen))
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

var smallOddFactorial []int64 = []int64{1, 1, 1, 3, 3,
	15, 45, 315, 315, 2835, 14175, 155925, 467775,
	6081075, 42567525, 638512875, 638512875, 10854718875,
	97692469875, 1856156927625, 9280784638125, 194896477400625,
	2143861251406875, 49308808782358125, 147926426347074375,
	3698160658676859375}

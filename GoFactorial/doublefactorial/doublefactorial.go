// doublefactorial package doublefactorial.go
// 2010/6/29 Sonia Keys
// 2011/6/29 edited Peter Luschny
// MIT license

package doublefactorial

import (
	"big"
	"../obj/xmath"
	"../obj/swingfactorial"
)

// returns nil if sieve not big enough
func doubleFactorial(s *swingfactorial.Swing, n uint64) (r *big.Int) {

	nEven := n & 1 == 0

	if n < uint64(len(smallOddDoubleFactorial)) {
		r = big.NewInt(smallOddDoubleFactorial[n])
	} else {
		var nn uint64
		if nEven {
			nn = n / 2
		} else {
			nn = n + 1
		}
		r = oddDoubleFactorial(s, nn, n)
	}

	if nEven {
		exp := uint(n - uint64(xmath.BitCount64(n)))
		r.Lsh(r, exp)
	}

	return
}

func oddDoubleFactorial(s *swingfactorial.Swing, n, m uint64) *big.Int {

	if n < uint64(len(smallOddFactorial)) {
		return big.NewInt(smallOddFactorial[n])
	}

	of := oddDoubleFactorial(s, n / 2, m)
	if n < m {
		of.Mul(of, of)
	}

	return of.Mul(of, s.OddSwing(n))
}

func DoubleFactorial(n uint64) *big.Int {
	s := swingfactorial.NewSwing(n+1)
	return doubleFactorial(s, n)
}

var smallOddFactorial []int64 = []int64{1, 1, 1, 3, 3,
	15, 45, 315, 315, 2835, 14175, 155925, 467775,
	6081075, 42567525, 638512875, 638512875, 10854718875,
	97692469875, 1856156927625, 9280784638125, 194896477400625,
	2143861251406875, 49308808782358125, 147926426347074375,
	3698160658676859375}

var smallOddDoubleFactorial []int64 = []int64 {1, 1, 1, 3, 1,
	15, 3, 105, 3, 945, 15, 10395, 45, 135135, 315, 2027025, 315,
	34459425, 2835, 654729075, 14175, 13749310575, 155925,
	316234143225, 467775, 7905853580625, 6081075, 213458046676875,
	42567525, 6190283353629375, 638512875, 191898783962510625,
	638512875, 6332659870762850625, 10854718875}

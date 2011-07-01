// primefactorial package primefactorial.go
// 2010/6/29 Sonia Keys
// 2011/6/29 edited Peter Luschny
// MIT license

package primefactorial

import (
	"big"
	"../obj/xmath"
	"../obj/swingfactorial"
)

func Factorial(n uint64) *big.Int {

	if n < 20 {
		var r big.Int
		return r.MulRange(1, int64(n))
	}

	s := swingfactorial.NewSwing(n)
	r := OddFactorial(s, n)
	exp := uint(n - uint64(xmath.BitCount64(n)))
	return r.Lsh(r, exp)
}

func OddFactorial(s *swingfactorial.Swing, n uint64) *big.Int {

	if n < uint64(len(smallOddFactorial)) {
		return big.NewInt(smallOddFactorial[n])
	}

	of := OddFactorial(s, n / 2) // recurse
	of.Mul(of, of)               // square

	return of.Mul(of, s.OddSwing(n))
}

var smallOddFactorial []int64 = []int64{1, 1, 1, 3, 3,
	15, 45, 315, 315, 2835, 14175, 155925, 467775,
	6081075, 42567525, 638512875, 638512875, 10854718875,
	97692469875, 1856156927625, 9280784638125, 194896477400625,
	2143861251406875, 49308808782358125, 147926426347074375,
	3698160658676859375}

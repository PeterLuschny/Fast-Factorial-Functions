// binomial package binomial.go
// 2010/6/29 Sonia Keys
// 2011/6/29 edited Peter Luschny
// MIT license

package binomial

import (
	"math/big"

	"github.com/PeterLuschny/Fast-Factorial-Functions/GoFactorial/primes"
	"github.com/PeterLuschny/Fast-Factorial-Functions/GoFactorial/xmath"
)

func binomial(p *primes.Sieve, n, k uint64) *big.Int {
	var r big.Int
	if k > n {
		return &r
	}

	if k > n/2 {
		k = n - k
	}

	if k < 3 {
		switch k {
		case 0:
			return r.SetInt64(1)
		case 1:
			return r.SetInt64(int64(n))
		case 2:
			var n1 big.Int
			return r.Rsh(r.Mul(r.SetInt64(int64(n)), n1.SetInt64(int64(n-1))), 1)
		}
	}

	var i int
	rootN := xmath.FloorSqrt(n)
	factors := make([]uint64, n)

	p.IteratePrimes(2, rootN, func(p uint64) {
		var r, nn, kk uint64 = 0, n, k
		for nn > 0 {
			if nn%p < kk%p+r {
				r = 1
				factors[i] = p
				i++
			} else {
				r = 0
			}
			nn /= p
			kk /= p
		}
	})

	p.IteratePrimes(rootN+1, n/2, func(p uint64) {
		if n%p < k%p {
			factors[i] = p
			i++
		}
	})

	p.IteratePrimes(n-k+1, n, func(p uint64) {
		factors[i] = p
		i++
	})

	return xmath.Product(factors[0:i])
}

func Binomial(n, k uint64) *big.Int {
	p := primes.Primes(n)
	return binomial(p, n, k)
}

// xmath package xmath.go
// 2010/6/29 Sonia Keys
// 2011/6/29 Peter Luschny edited
// MIT license

package xmath

import (
	"math/big"
)

const productSerialThreshold = 20

func Product(seq []uint64) *big.Int {

	if len(seq) <= productSerialThreshold {
		var b big.Int
		sprod := big.NewInt(int64(seq[0]))
		for _, s := range seq[1:] {
			b.SetInt64(int64(s))
			sprod.Mul(sprod, &b)
		}
		return sprod
	}

	halfLen := len(seq) / 2
	lprod := Product(seq[0:halfLen])
	return lprod.Mul(lprod, Product(seq[halfLen:]))
}

func FloorSqrt(n uint64) uint64 {
	for b := n; ; {
		a := b
		b = (n/a + a) / 2
		if b >= a {
			return a
		}
	}
	return 0 // unreachable.  required by current compiler.
}

func BitCount32(w uint32) uint {
	const (
		ff    = 1<<32 - 1
		mask1 = ff / 3
		mask3 = ff / 5
		maskf = ff / 17
		maskp = ff / 255
	)
	w -= w >> 1 & mask1
	w = w&mask3 + w>>2&mask3
	w = (w + w>>4) & maskf
	return uint(w * maskp >> 24)
}

func BitCount64(w uint64) uint { // loopfree!
	const (
		ff    = 1<<64 - 1
		mask1 = ff / 3
		mask3 = ff / 5
		maskf = ff / 17
		maskp = maskf >> 3 & maskf
	)
	w -= w >> 1 & mask1
	w = w&mask3 + w>>2&mask3
	w = (w + w>>4) & maskf
	return uint(w * maskp >> 56)
}

func BitCount(x uint) uint {
	x = x - ((x >> 1) & 0x55555555)
	x = (x & 0x33333333) + ((x >> 2) & 0x33333333)
	x = (x + (x >> 4)) & 0x0F0F0F0F
	x = x + (x >> 8)
	x = x + (x >> 16)
	return x & 0x0000003F
}

package simplefactorial

import (
	"fmt"
	"math/big"
	"strconv"
	"testing"
)

var one00, _ = new(big.Int).SetString("93326215443944152681699238856266700490715968264381621468592963895217599993229915608941463976156518286253697920827223758251185210916864000000000000000000000000", 10)
var benchmarks = []uint64{
	100,
	1000,
	10000,
	100000,
}

func BenchmarkFactorial(b *testing.B) {
	for _, benchmark := range benchmarks {
		b.Run(fmt.Sprintf("%d", benchmark), func(b *testing.B) {
			for j := 0; j < b.N; j++ {
				Factorial(benchmark)
			}
		})
	}
}

// func info(a *big.Int, n uint) {
// 	dtrunc := int64(float64(a.BitLen())*.30103) - 10
// 	var first, rest big.Int
// 	rest.Exp(first.SetInt64(10), rest.SetInt64(dtrunc), nil)
// 	first.Quo(a, &rest)
// 	fstr := first.String()
// 	fmt.Printf("%d! begins %s... and has %d digits.\n",
// 		n, fstr, int64(len(fstr))+dtrunc)
// }

func TestFactorial(t *testing.T) {
	tests := []struct {
		input    uint64
		expected *big.Int
	}{
		{0, big.NewInt(1)},
		{1, big.NewInt(1)},
		{2, big.NewInt(2)},
		{3, big.NewInt(6)},
		{4, big.NewInt(24)},
		{5, big.NewInt(120)},
		{100, one00},
	}
	for ii, tt := range tests {
		t.Run(strconv.Itoa(ii), func(t *testing.T) {
			if gotR := Factorial(tt.input); gotR.Cmp(tt.expected) != 0 {
				t.Errorf("Factorial() = %v, want %v", gotR, tt.expected)
			}
		})
	}
}

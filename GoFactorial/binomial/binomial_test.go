// binomial_test project binomial_test.go

package binomial

import (
	"fmt"
	"math/big"
	"strconv"
	"testing"
)

var four00, _ = new(big.Int).SetString("102440298642203415893508338627265658464886492916495172372984138881515753604628814525708055242762679553795073231350024000", 10)
var five00, _ = new(big.Int).SetString("2314422827984300469017756871661048812545657819062792522329327913362690", 10)
var binomialBenchmarks = []uint64{
	100,
	1000,
	10000,
	100000,
	1000000,
	2000000,
}

func BenchmarkBinomial(b *testing.B) {
	for _, benchmark := range binomialBenchmarks {
		b.Run(fmt.Sprintf("%d", benchmark), func(b *testing.B) {
			for j := 0; j < b.N; j++ {
				Binomial(benchmark, benchmark/3)
			}
		})
	}
}

func BenchmarkGoBinomial(b *testing.B) {
	for _, benchmark := range binomialBenchmarks {
		var bi big.Int
		b.Run(fmt.Sprintf("%d", benchmark), func(b *testing.B) {
			for j := 0; j < b.N; j++ {
				bi.Binomial(int64(benchmark), int64(benchmark/3))

			}
		})
	}
}

func TestBinomial(t *testing.T) {
	tests := []struct {
		inputN, inputK uint64
		expected       *big.Int
	}{
		{0, 0, big.NewInt(1)},
		{1, 0, big.NewInt(1)},
		{1, 1, big.NewInt(1)},
		{2, 0, big.NewInt(1)},
		{2, 1, big.NewInt(2)},
		{2, 2, big.NewInt(1)},
		{3, 0, big.NewInt(1)},
		{3, 1, big.NewInt(3)},
		{3, 2, big.NewInt(3)},
		{3, 3, big.NewInt(1)},
		{123, 108, big.NewInt(int64(7012067478708989884))},
		{400, 199, four00},
		{500, 50, five00},
		{1000, 998, big.NewInt(499500)},
	}
	for ii, tt := range tests {
		t.Run(strconv.Itoa(ii), func(t *testing.T) {
			if gotR := Binomial(tt.inputN, tt.inputK); gotR.Cmp(tt.expected) != 0 {
				t.Errorf("Binomial() = %v, want %v", gotR, tt.expected)
			}
		})
	}
}

func TestBinomial2(t *testing.T) {
	for n := 10; n < 1<<10; n++ {
		for k := 0; k < n; k++ {
			expected := new(big.Int).Binomial(int64(n), int64(k))
			actual := Binomial(uint64(n), uint64(k))
			if expected.Cmp(actual) != 0 {
				t.Fatalf("expected (%v,%v) to match", n, k)
			}
		}
	}
}

// swingfactorial_test project swingfactorial_test.go

package swingfactorial

import (
	"fmt"
	"math/big"
	"strconv"
	"testing"
)

var one03, _ = new(big.Int).SetString("41159712051274678559296251331536", 10)
var five00, _ = new(big.Int).SetString("116744315788277682920934734762176619659230081180311446124100284957811112673608473715666417775521605376810865902709989580160037468226393900042796872256", 10)
var one000, _ = new(big.Int).SetString("270288240945436569515614693625975275496152008446548287007392875106625428705522193898612483924502370165362606085021546104802209750050679917549894219699518475423665484263751733356162464079737887344364574161119497604571044985756287880514600994219426752366915856603136862602484428109296905863799821216320", 10)
var benchmarks = []uint64{
	100,
	1000,
	10000,
	100000,
	1000000,
}

func TestSwingingFactorial(t *testing.T) {
	tests := []struct {
		input    uint64
		expected *big.Int
	}{
		{0, big.NewInt(1)},
		{1, big.NewInt(1)},
		{2, big.NewInt(2)},
		{3, big.NewInt(6)},
		{4, big.NewInt(6)},
		{5, big.NewInt(30)},
		{6, big.NewInt(20)},
		{7, big.NewInt(140)},
		{8, big.NewInt(70)},
		{103, one03},
		{500, five00},
		{1000, one000},
	}
	for ii, tt := range tests {
		t.Run(strconv.Itoa(ii), func(t *testing.T) {
			if gotR := SwingingFactorial(tt.input); gotR.Cmp(tt.expected) != 0 {
				t.Errorf("SwingingFactorial() = %v, want %v", gotR, tt.expected)
			}
		})
	}
}

func BenchmarkSwingingFactorial(b *testing.B) {
	for _, benchmark := range benchmarks {
		b.Run(fmt.Sprintf("%d", benchmark), func(b *testing.B) {
			for j := 0; j < b.N; j++ {
				SwingingFactorial(benchmark)
			}
		})
	}
}

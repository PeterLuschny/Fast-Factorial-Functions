// primeswing_test project

package primefactorial

import (
	"fmt"
	"math/big"
	"strconv"
	"testing"
)

var six5, _ = new(big.Int).SetString("8247650592082470666723170306785496252186258551345437492922123134388955774976000000000000000", 10)
var one00, _ = new(big.Int).SetString("93326215443944152681699238856266700490715968264381621468592963895217599993229915608941463976156518286253697920827223758251185210916864000000000000000000000000", 10)
var four02, _ = new(big.Int).SetString("10322493151921465164081017511444523549144957788957729070658850054871632028467255601190963314928373192348001901396930189622367360453148777593779130493841936873495349332423413459470518031076600468677681086479354644916620480632630350145970538235260826120203515476630017152557002993632050731959317164706296917171625287200618560036028326143938282329483693985566225033103398611546364400484246579470387915281737632989645795534475998050620039413447425490893877731061666015468384131920640823824733578473025588407103553854530737735183050931478983505845362197959913863770041359352031682005647007823330600995250982455385703739491695583970372977196372367980241040180516191489137558020294105537577853569647066137370488100581103217089054291400441697731894590238418118698720784367447615471616000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", 10)
var benchmarks = []uint64{
	100,
	1000,
	10000,
	100000,
	//	1000000,
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

func BenchmarkGoFactorial(b *testing.B) {
	for _, benchmark := range benchmarks {
		b.Run(fmt.Sprintf("%d", benchmark), func(b *testing.B) {
			for j := 0; j < b.N; j++ {
				Factorial(benchmark)
			}
		})
	}
}

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
		{20, big.NewInt(int64(2432902008176640000))},
		{65, six5},
		{100, one00},
		{402, four02},
	}
	for ii, tt := range tests {
		t.Run(strconv.Itoa(ii), func(t *testing.T) {
			if got := Factorial(tt.input); got.Cmp(tt.expected) != 0 {
				t.Errorf("Factorial() = %v, want %v", got, tt.expected)
			}
		})
	}
}
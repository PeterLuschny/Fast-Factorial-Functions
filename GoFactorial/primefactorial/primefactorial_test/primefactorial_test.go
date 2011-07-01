// primeswing_test project

package main

import (
	"big"
	"fmt"
	"time"
	"../../obj/primefactorial"
)

var one00, six5, four02 big.Int

type primefactorialTest struct {
	in uint64
	out *big.Int
}

var primefactorialTests = []primefactorialTest{
	primefactorialTest{0, big.NewInt(1)},
	primefactorialTest{1, big.NewInt(1)},
	primefactorialTest{2, big.NewInt(2)},
	primefactorialTest{3, big.NewInt(6)},
	primefactorialTest{4, big.NewInt(24)},
	primefactorialTest{20, big.NewInt(int64(2432902008176640000))},
	primefactorialTest{65,  &six5},
	primefactorialTest{100, &one00},
	primefactorialTest{402, &four02},
}

func TestPrimefactorial(test []primefactorialTest) {
	fmt.Println("\nTest running!")
	for _, t := range test {

		v := primefactorial.Factorial(t.in)

		if v.Cmp(t.out) == 0 {
			fmt.Printf("%d! pass.\n", t.in)
		} else {
			fmt.Printf("%d! fail.\nout   %d \nwant %d.", t.in, v, t.out)
			fmt.Println()
		}
	}
}

var primefactorialBenchmarks = []uint64{
	100,
	1000,
	10000,
	100000,
//	1000000,
}

func BenchmarkPrimefactorial(test []uint64) {
	fmt.Println("\nBenchmark running!\n")
	for _, t := range test {
		start := time.Nanoseconds()

		primefactorial.Factorial(t)

		stop := time.Nanoseconds()
		fmt.Printf("Time for %d was %.2f sec.\n", t, float64(stop-start)/1e9)
	}
}

/*
func BenchmarkGoFactorial(test []uint64) {
	fmt.Println("\nBenchmark running!\n")
	var b big.Int
	for _, t := range test {
		start := time.Nanoseconds()

		// there is no
		b.Factorial(t)

		stop := time.Nanoseconds()
		fmt.Printf("Time for %d was %.2f sec.\n", t, float64(stop-start)/1e9)
	}
}
*/

func Tests() {
	six5.SetString(six5Fact, 10)
	one00.SetString(one00Fact, 10)
	four02.SetString(four02Fact, 10)

	TestPrimefactorial(primefactorialTests)
	BenchmarkPrimefactorial(primefactorialBenchmarks)
//	BenchmarkGoFactorial(primefactorialBenchmarks)
}

func main() {
	Tests()
}

const six5Fact   = "8247650592082470666723170306785496252186258551345437492922123134388955774976000000000000000"
const one00Fact  = "93326215443944152681699238856266700490715968264381621468592963895217599993229915608941463976156518286253697920827223758251185210916864000000000000000000000000"
const four02Fact = "10322493151921465164081017511444523549144957788957729070658850054871632028467255601190963314928373192348001901396930189622367360453148777593779130493841936873495349332423413459470518031076600468677681086479354644916620480632630350145970538235260826120203515476630017152557002993632050731959317164706296917171625287200618560036028326143938282329483693985566225033103398611546364400484246579470387915281737632989645795534475998050620039413447425490893877731061666015468384131920640823824733578473025588407103553854530737735183050931478983505845362197959913863770041359352031682005647007823330600995250982455385703739491695583970372977196372367980241040180516191489137558020294105537577853569647066137370488100581103217089054291400441697731894590238418118698720784367447615471616000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"


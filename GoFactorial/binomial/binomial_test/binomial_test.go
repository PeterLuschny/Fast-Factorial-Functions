// binomial_test project binomial_test.go

package main 

import (
	"big"
	"fmt"
	"time"
	"../../obj/binomial"
)

var four00, five00 big.Int  

type binomialTest struct {
	nin uint64
	kin uint64
	out *big.Int
}

var binomialTests = []binomialTest{
	binomialTest{0, 0, big.NewInt(1)},
	binomialTest{1, 0, big.NewInt(1)},
	binomialTest{1, 1, big.NewInt(1)},
	binomialTest{2, 0, big.NewInt(1)},
	binomialTest{2, 1, big.NewInt(2)},
	binomialTest{2, 2, big.NewInt(1)},
	binomialTest{3, 0, big.NewInt(1)},
	binomialTest{3, 1, big.NewInt(3)},
	binomialTest{3, 2, big.NewInt(3)},
	binomialTest{3, 3, big.NewInt(1)},
	binomialTest{123, 108, big.NewInt(int64(7012067478708989884))},
	binomialTest{400, 199, &four00},
	binomialTest{500, 50,  &five00},
	binomialTest{1000, 998, big.NewInt(499500)},
}

func TestBinomial(test []binomialTest) {
	fmt.Println("\nTest running!")
	for _, t := range test {
		
		v := binomial.Binomial(t.nin, t.kin)
		
		if v.Cmp(t.out) == 0 {
			fmt.Printf("%d %d passed.\n", t.nin, t.kin)
		} else {
			fmt.Printf("%d %d failed.\nout   %d \nwant %d.", t.nin, t.kin, v, t.out)
			fmt.Println()			
		}
	}
}

var binomialBenchmarks = []uint64{
	100,
	1000,
	10000,
	100000,
	1000000,
	2000000,
}

func BenchmarkBinomial(test []uint64) {
	fmt.Println("\nBenchmark running!\n")
	for _, t := range test {
		start := time.Nanoseconds()
		
		binomial.Binomial(t, t / 3)
		
		stop := time.Nanoseconds()
		fmt.Printf("Time for %d %d was %.2f sec.\n", t, t/3, float64(stop-start)/1e9)
	}
}

func BenchmarkGoBinomial(test []uint64) {
	fmt.Println("\nBenchmark running!\n")
	var b big.Int
	for _, t := range test {
		start := time.Nanoseconds()
		
		b.Binomial(int64(t), int64(t / 3))
		
		stop := time.Nanoseconds()
		fmt.Printf("Time for %d %d was %.2f sec.\n", t, t/3, float64(stop-start)/1e9)
	}
}

func Tests() {
	four00.SetString(four00Fact, 10)  
	five00.SetString(five00Fact, 10)
	
	TestBinomial(binomialTests)
	fmt.Println()
	fmt.Println("Our implementation:")
	BenchmarkBinomial(binomialBenchmarks)
	fmt.Println()
	fmt.Println("Golang's implementation:")
	fmt.Println("Good night!")	
	BenchmarkGoBinomial(binomialBenchmarks)	
}

func main() {
	Tests()
}

const four00Fact = "102440298642203415893508338627265658464886492916495172372984138881515753604628814525708055242762679553795073231350024000"
const five00Fact = "2314422827984300469017756871661048812545657819062792522329327913362690"

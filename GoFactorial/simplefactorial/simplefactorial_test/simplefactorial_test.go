package main 

import (
	"big"
	"fmt"
	"time"
	"../../obj/simplefactorial"
)

var one00 big.Int  

type simplefacts struct {
	in uint64
	out *big.Int
}

var simplefactorials = []simplefacts {
	simplefacts{0, big.NewInt(1)},
	simplefacts{1, big.NewInt(1)},
	simplefacts{2, big.NewInt(2)},
	simplefacts{3, big.NewInt(6)},
	simplefacts{4, big.NewInt(24)},
	simplefacts{5, big.NewInt(120)},
	simplefacts{100, &one00},
}

func TestSimplefactorial(test []simplefacts) {
	fmt.Println("\nTest running!")
	for _, t := range test {
	
		v := simplefactorial.Factorial(t.in)
		
		if v.Cmp(t.out) == 0 {
			fmt.Printf("%d! pass.\n", t.in)
		} else {
			fmt.Printf("%d! fail.\nout   %d \nwant %d.", t.in, v, t.out)
			fmt.Println()			
		}
	}
}

var simplefactorialBenchmarks = []uint64{
	100,
	1000,
	10000,
	100000,
}

func BenchmarkSimplefactorial(test []uint64) {
	fmt.Println("\nBenchmark running!\n")
	for _, t := range test {
		start := time.Nanoseconds()
		
		simplefactorial.Factorial(t)
		
		stop := time.Nanoseconds()
		fmt.Printf("Time for %d was %.2f sec.\n", t, float64(stop-start)/1e9)
	}
}

func Tests() {
	one00.SetString(one00Fact, 10)
	TestSimplefactorial(simplefactorials)
	BenchmarkSimplefactorial(simplefactorialBenchmarks)	
}

func info(a *big.Int, n uint) {
	dtrunc := int64(float64(a.BitLen())*.30103) - 10
	var first, rest big.Int
	rest.Exp(first.SetInt64(10), rest.SetInt64(dtrunc), nil)
	first.Quo(a, &rest)
	fstr := first.String()
	fmt.Printf("%d! begins %s... and has %d digits.\n",
		n, fstr, int64(len(fstr))+dtrunc)
}

func main() {
	Tests()
}

const one00Fact = "93326215443944152681699238856266700490715968264381621468592963895217599993229915608941463976156518286253697920827223758251185210916864000000000000000000000000"

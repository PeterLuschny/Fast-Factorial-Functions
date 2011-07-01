// swingfactorial_test project swingfactorial_test.go

package main 

import (
	"big"
	"fmt"
	"time"
	"../../obj/swingfactorial"
)

var one03, five00, one000 big.Int  

type swingfacts struct {
	in uint64
	out *big.Int
}

var swingfactorials = []swingfacts {
	swingfacts{0, big.NewInt(1)},
	swingfacts{1, big.NewInt(1)},
	swingfacts{2, big.NewInt(2)},
	swingfacts{3, big.NewInt(6)},
	swingfacts{4, big.NewInt(6)},
	swingfacts{5, big.NewInt(30)},
	swingfacts{6, big.NewInt(20)},
	swingfacts{7, big.NewInt(140)},
	swingfacts{8, big.NewInt(70)},
	swingfacts{103, &one03},
	swingfacts{500, &five00},
	swingfacts{1000, &one000},
}

func TestSwingfactorial(test []swingfacts) {
	fmt.Println("\nTest running!")
	for _, t := range test {
	
		v := swingfactorial.SwingingFactorial(t.in)
		
		if v.Cmp(t.out) == 0 {
			fmt.Printf("%d$ passed.\n", t.in)
		} else {
			fmt.Printf("%d$ failed. out %d want %d.", t.in, v, t.out)
			fmt.Println()			
		}
	}
}

var swingfactorialBenchmarks = []uint64{
	100,
	1000,
	10000,
	100000,
	1000000,
}

func BenchmarkSwingfactorial(test []uint64) {
	fmt.Println("\nBenchmark running!\n")
	for _, t := range test {
		start := time.Nanoseconds()
		
		swingfactorial.SwingingFactorial(t)
		
		stop := time.Nanoseconds()
		fmt.Printf("Time for %d was %.2f sec.\n", t, float64(stop-start)/1e9)
	}
}

func Tests() {
	one03.SetString(one03SWFact, 10)
	five00.SetString(five00SWFact, 10)
	one000.SetString(one000SWFact, 10)
	
	TestSwingfactorial(swingfactorials)
	BenchmarkSwingfactorial(swingfactorialBenchmarks)	
}

func main() {
	Tests()
}

const one03SWFact  = "41159712051274678559296251331536"
const five00SWFact = "116744315788277682920934734762176619659230081180311446124100284957811112673608473715666417775521605376810865902709989580160037468226393900042796872256"
const one000SWFact = "270288240945436569515614693625975275496152008446548287007392875106625428705522193898612483924502370165362606085021546104802209750050679917549894219699518475423665484263751733356162464079737887344364574161119497604571044985756287880514600994219426752366915856603136862602484428109296905863799821216320"

// xmath_test project xmath_test.go
package main

import (
	"../../obj/xmath"
)

// TODO: Calibrate the serial threshold for product.

func TestFloorSqrt() bool {

	return xmath.FloorSqrt(4) == 2 
}

func Tests() {
	TestFloorSqrt()
}

func main() {
	Tests()
}


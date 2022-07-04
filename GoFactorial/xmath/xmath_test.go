// xmath_test project xmath_test.go
package xmath

import (
	"strconv"
	"testing"
)

// TODO: Calibrate the serial threshold for product.

func TestBinomial(t *testing.T) {
	tests := []struct {
		input    uint64
		expected uint64
	}{
		{4, 2},
	}
	for ii, tt := range tests {
		t.Run(strconv.Itoa(ii), func(t *testing.T) {
			if gotR := FloorSqrt(tt.input); gotR != tt.expected {
				t.Errorf("FloorSqrt() = %v, want %v", gotR, tt.expected)
			}
		})
	}
}

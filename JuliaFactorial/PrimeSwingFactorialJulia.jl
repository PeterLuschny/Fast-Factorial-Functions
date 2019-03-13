# Copyright Peter Luschny. License is MIT.

module PrimeSwingFactorial
using PrimesIterator
export PSfactorial, Swing

"""
Return the accumulated product of an array.
"""
function ∏(A)
    function prod(a, b)
        n = b - a
        if n < 24
            p = BigInt(1)
            for k in a:b
                p *= A[k]
            end
            return BigInt(p)
        end
        m = div(a + b, 2)
        prod(a, m) * prod(m + 1, b)
    end
    A == [] && return 1
    prod(1, length(A))
end

const SwingOddpart = [1,1,1,3,3,15,5,35,35, 315, 63, 693, 231, 3003, 429, 6435,
   6435,109395,12155,230945,46189,969969,88179,2028117,676039,16900975,1300075,
   35102025,5014575,145422675,9694845,300540195,300540195]

"""
Computes the odd part of the swinging factorial ``n≀``. Cf. A163590.
"""
function swing_oddpart(n::Int)
    n < 33 && return ZZ(SwingOddpart[n+1])

    sqrtn = isqrt(n)
    factors = Primes(div(n,2) + 1, n)
    r = Primes(sqrtn + 1, div(n, 3))
    s = filter(x -> isodd(div(n, x)), r)

    for prime in Primes(3, sqrtn)
        p, q = 1, n
        while true
            q = div(q, prime)
            q == 0 && break
            isodd(q) && (p *= prime)
        end
        p > 1 && push!(s, p)
    end

    return ∏(factors)*∏(s)
end

"""
Computes the swinging factorial (a.k.a. Swing numbers n≀). Cf. A056040.
"""
function Swing(n::Int)
    sh = count_ones(div(n, 2))
    swing_oddpart(n) << sh
end

const FactorialOddPart = [1, 1, 1, 3, 3, 15, 45, 315, 315, 2835, 14175, 155925,
    467775, 6081075, 42567525, 638512875, 638512875, 10854718875, 97692469875,
    1856156927625, 9280784638125, 194896477400625, 2143861251406875,
    49308808782358125, 147926426347074375, 3698160658676859375]

"""
Return the largest odd divisor of ``n!``. Cf. A049606.
"""
function factorial_oddpart(n::Int)
    n < length(FactorialOddPart) && return BigInt(FactorialOddPart[n+1])
    swing_oddpart(n)*(factorial_oddpart(div(n,2))^2)
end

"""
Return the factorial ``n! = 1*2*...*n``, which is the order of the
symmetric group S_n or the number of permutations of n letters. Cf. A000142.
"""
function PSfactorial(n::Int)
    n < 0 && ArgumentError("Argument must be >= 0")
    sh = n - count_ones(n)
    factorial_oddpart(n) << sh
end

#START-TEST-########################################################
using Test

function main()
    @testset "PrimeSwing" begin
        for n in 0:999
            S = PSfactorial(n)
            B = Base.factorial(BigInt(n))
            @test S == B
        end
    end
    @time PSfactorial(1000000)
end

main()

end # module

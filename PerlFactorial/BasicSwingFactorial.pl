#!perl
#
# swingFactorial
# An efficient way to compute the factorial numbers.
#
# 2019-06-28, Georg Fischer: rewritten from Julia module of Peter Luschny 
# 2019-06-29, Peter Luschny: some tweaks
#---------------------------------------------------------------------

use strict;
use integer;
use Math::BigInt;
use Math::BigInt':constant'; 
sub bigint { Math::BigInt->new(shift); }
sub bighex { Math::BigInt->from_hex(shift); }

my @smallOddFactorial =                  ( "0x0000000000000000000000000000001"
    , "0x0000000000000000000000000000001", "0x0000000000000000000000000000001"
    , "0x0000000000000000000000000000003", "0x0000000000000000000000000000003"
    , "0x000000000000000000000000000000f", "0x000000000000000000000000000002d"
    , "0x000000000000000000000000000013b", "0x000000000000000000000000000013b"
    , "0x0000000000000000000000000000b13", "0x000000000000000000000000000375f"
    , "0x0000000000000000000000000026115", "0x000000000000000000000000007233f"
    , "0x00000000000000000000000005cca33", "0x0000000000000000000000002898765"
    , "0x00000000000000000000000260eeeeb", "0x00000000000000000000000260eeeeb"
    , "0x0000000000000000000000286fddd9b", "0x00000000000000000000016beecca73"
    , "0x000000000000000000001b02b930689", "0x00000000000000000000870d9df20ad"
    , "0x0000000000000000000b141df4dae31", "0x00000000000000000079dd498567c1b"
    , "0x00000000000000000af2e19afc5266d", "0x000000000000000020d8a4d0f4f7347"
    , "0x000000000000000335281867ec241ef", "0x0000000000000029b3093d46fdd5923"
    , "0x0000000000000465e1f9767cc5866b1", "0x0000000000001ec92dd23d6966aced7"
    , "0x0000000000037cca30d0f4f0a196e5b", "0x0000000000344fd8dc3e5a1977d7755"
    , "0x000000000655ab42ab8ce915831734b", "0x000000000655ab42ab8ce915831734b"
    , "0x00000000d10b13981d2a0bc5e5fdcab", "0x0000000de1bc4d19efcac82445da75b"
    , "0x000001e5dcbe8a8bc8b95cf58cde171", "0x00001114c2b2deea0e8444a1f3cecf9"
    , "0x0002780023da37d4191deb683ce3ffd", "0x002ee802a93224bddd3878bc84ebfc7"
    , "0x07255867c6a398ecb39a64b83ff3751", "0x23baba06e131fc9f8203f7993fc1495"
    );

    sub oddProduct { my ($m, $len) = @_;
        if ($len < 24) {
            my $p = bigint($m);
            my $k = 2;

            while ($k <= 2 * ($len - 1)) {
                $p *= ($m - $k);
                $k += 2;
            } # while

            return $p;
        } # if

        my $hlen = $len >> 1;
        return &oddProduct($m - 2 * $hlen, $len - $hlen)->bmul(&oddProduct($m, $hlen));
    } # oddProduct

    sub oddFactorial { my ($n) = @_;
        my ($sqrOddFact, $oddFact, $oldOddFact);

        if ($n < scalar(@smallOddFactorial)) { 
            $oddFact    = bighex($smallOddFactorial[$n]);
            $sqrOddFact = bighex($smallOddFactorial[$n / 2]);
        } else {
            ($sqrOddFact, $oldOddFact) = &oddFactorial($n / 2);
            my $len = ($n - 1) / 4;
            if ($n % 4 != 2) {
                $len ++;
            }
            my $high = $n - (($n + 1) & 1);
            my $oddSwing = &oddProduct($high, $len)->bdiv($oldOddFact);
            $oddFact = $sqrOddFact->copy()->bpow(2)->bmul($oddSwing);
        } # if/else

        return ($oddFact, $sqrOddFact);
    } # oddFactorial
            
    sub swingFactorial { my ($n) = @_;
        if ($n < 0) {
            print STDERR "n must be ≥ 0\n";
            return 0/0;
        } # if

        return (&oddFactorial($n))[0]->blsft($n - &popcount($n));
    } # swingFactorial
    
    # from <https://www.perlmonks.org/?node_id=1199987>
    sub popcount {
        my ($parm) = @_;
        my $result = (bigint($parm)->as_bin()) =~ tr/1//;
        return $result;
    } # popcount

# START-TEST

my $n;
for $n (0..999) {
    my $s = &swingFactorial($n);
    my $f = bigint($n)->bfac(); # factorial from library

    print "$n -> $s \n";
    print "Error at n = $n" if ($s != $f);
} 
exit();

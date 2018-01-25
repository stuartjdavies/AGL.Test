using System.Linq;
using System;
using FsCheck;
using Fp.Common;

namespace AGL.Test.Solution.XUnit.Tests
{
    public class IntBetween1and100
    {
        public static Gen<int> Generate()
        => Gen.Elements(Enumerable.Range(1, 100));            

        public static Arbitrary<Int32> Int()
        => Gen.Elements(Enumerable.Range(1, 100))
                .Pipe(es => Arb.From(es));
    }
}

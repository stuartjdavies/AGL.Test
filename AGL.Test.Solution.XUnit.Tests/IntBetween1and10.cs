using System.Linq;
using System;
using FsCheck;
using Fp.Common;

namespace AGL.Test.Solution.XUnit.Tests
{
    public partial class PetRepositoryTests
    {
        public class IntBetween1and10
        {
            public static Gen<int> Generate()
            => Gen.Elements(Enumerable.Range(1, 10));

            public static Arbitrary<Int32> Int()
            => Gen.Elements(Enumerable.Range(1, 10))
                  .Pipe(es => Arb.From(es));            
        }
    }
}

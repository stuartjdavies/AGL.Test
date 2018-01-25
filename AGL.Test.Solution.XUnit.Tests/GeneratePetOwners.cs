using System.Linq;
using FsCheck;
using Fp.Common;
using System.Collections.Generic;

namespace AGL.Test.Solution.XUnit.Tests
{
    public class GeneratePetOwners
    {
        public static Arbitrary<List<Domain.Person>> GetInput()
        => Gen.zip(IntBetween1and10.Generate(),
                   IntBetween1and100.Generate())                  
              .Select(x => PetOwnerGenerator.Generate(0, x.Item1)
                                            .Generate(x.Item2))
              .Pipe(Arb.From);
    }
}

using System.Linq;
using FsCheck;
using Fp.Common;

namespace AGL.Test.Solution.XUnit.Tests
{
    public partial class PetRepositoryTests
    {
        public class PetOwnerGeneratorInputData
        {
            public static Arbitrary<(int MaxNumberOfPets, int NumberOfPeople)> GetInput()
            => Gen.zip(IntBetween1and10.Generate(),
                       IntBetween1and100.Generate())
                  .Select(x => (x.Item1, x.Item2))
                  .Pipe(Arb.From);                        
        }
    }
}

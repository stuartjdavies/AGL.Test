using Fp.Common.Monads.EitherMonad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AGL.Test.Solution.Domain
{
    // 
    // Based on Either Monad
    // See https://www.schoolofhaskell.com/school/starting-with-haskell/basics-of-haskell/10_Error_Handling
    //
    public enum Gender { Male, Female }
    public interface IPetRepository
    {
        Task<Either<string, List<(string Gender, List<string> PetNames)>>> GetPetNamesInAlphabeticalOrderGroupedByGenderAsync();
    }
}

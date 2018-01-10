using Fp.Common.Monads.RopResultMonad;
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
        Task<RopResult<List<(string Gender, List<string> PetNames)>, DomainEvent[]>> GetPetNamesInAlphabeticalOrderGroupedByGenderAsync();
    }
}

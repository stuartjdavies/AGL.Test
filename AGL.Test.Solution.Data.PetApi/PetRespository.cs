using AGL.Test.Solution.Domain;
using Fp.Common;
using Fp.Common.Monads.EitherMonad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AGL.Test.Solution.Data.PetApi
{
    public class PetRespository : IPetRepository
    {
        private Func<Task<IEnumerable<Person>>> GetPetOwners { get; set; }
        public PetRespository(Func<Task<IEnumerable<Person>>> getPetOwners)
        => this.GetPetOwners = getPetOwners;

        public async Task<Either<string, List<(string Gender, List<string> PetNames)>>> GetPetNamesInAlphabeticalOrderGroupedByGenderAsync()
        {
            try
            {
                return (await GetPetOwners())
                       .GroupBy(p => p.Gender)                                  
                       .Select(g => (g.Key, g.Where(x => x.Pets != null)
                                             .SelectMany(x => x.Pets)                                                        
                                             .Select(x => x.Name)
                                             .OrderBy(x => x)
                                             .ToList()))
                       .ToList()
                       .Pipe(ps => Either<string, List<(string Gender, List<string> PetNames)>>.ReturnRight(ps));
            }
            catch(Exception ex)
            {
                return Either<string, List<(string Gender, List<string> PetNames)>>.ReturnLeft(ex.ToString());
            }
        }
    }
}

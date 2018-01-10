using AGL.Test.Solution.Domain;
using Fp.Common;
using Fp.Common.Monads.RopResultMonad;
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

        public async Task<RopResult<List<(string Gender, List<string> PetNames)>, DomainEvent[]>> GetPetNamesInAlphabeticalOrderGroupedByGenderAsync()
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
                       .Pipe(ps => {
                            var es = new [] { new DomainEvent("GetPetNamesInAlphabeticalOrderGroupedByGenderAsync",
                                                              "PetRespository",
                                                              EventLevel.Info, 
                                                              null) };
                            return RopResult<List<(string Gender, List<string> PetNames)>, DomainEvent[]>.ReturnSuccess((ps, es));
                        });
            }
            catch(Exception ex)
            {
                var es = new DomainEvent[] { new DomainEvent("GetPetNamesInAlphabeticalOrderGroupedByGenderFailure",
                                                             "PetRespository",
                                                             EventLevel.Error,
                                                             ex.Message) };

                return RopResult<List<(string Gender, List<string> PetNames)>, DomainEvent[]>.ReturnFailure(es);
            }
        }
    }
}

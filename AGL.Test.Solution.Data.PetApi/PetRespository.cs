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
        private Func<Task<RopResult<Person[], DomainEvent[]>>> GetPetOwners { get; set; }
        public PetRespository(Func<Task<RopResult<Person[], DomainEvent[]>>> getPetOwners)
        => this.GetPetOwners = getPetOwners;

        public async Task<RopResult<List<(string Gender, List<string> PetNames)>, DomainEvent[]>> GetPetNamesInAlphabeticalOrderGroupedByGenderAsync()
        {            
            RopResult<List<(string Gender, List<string> PetNames)>, DomainEvent[]> OnGetPetOwnersSuccess((IEnumerable<Person> PetOwners, DomainEvent[] DomainEvents) result)
            {
                var xs = result.PetOwners
                                .GroupBy(p => p.Gender)
                                .Select(g => (g.Key, g.Where(x => x.Pets != null)
                                            .SelectMany(x => x.Pets)
                                            .Select(x => x.Name)
                                            .OrderBy(x => x)
                                            .ToList())).ToList();

                var es = new[] { new DomainEvent("GetPetNamesInAlphabeticalOrderGroupedByGenderAsync",
                                                    "PetRespository",
                                                    EventLevel.Info,
                                                    null) }.Concat(result.DomainEvents).ToArray();
                    
                return RopResult<List<(string Gender, List<string> PetNames)>, DomainEvent[]>
                                .ReturnSuccess((xs, es));                    
            }

            RopResult<List<(string Gender, List<string> PetNames)>, DomainEvent[]> OnGetPetOwnersFailure(DomainEvent[] domainEvents)
            {
                var es = domainEvents.Concat(new DomainEvent[] { new DomainEvent("Called GetPetNamesInAlphabeticalOrderGroupedByGenderFailure",
                                                                "PetRespository",
                                                                EventLevel.Error,
                                                                "") }).ToArray();

                return RopResult<List<(string Gender, List<string> PetNames)>, DomainEvent[]>.ReturnFailure(es);
            };


            return (await GetPetOwners())
                    .Match(success => OnGetPetOwnersSuccess(success),
                            failure => OnGetPetOwnersFailure(failure));                                   
        }
    }
}

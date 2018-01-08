using AGL.Test.Solution.Domain;
using Fp.Common;
using Fp.Common.Monads.EitherMonad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AGL.Test.Solution
{
    public class PetRespository : IPetRepository
    {
        private Func<Task<IEnumerable<Person>>> GetPeople { get; set; }
        public PetRespository(Func<Task<IEnumerable<Person>>> getPeople)
        => this.GetPeople = getPeople;

        public async Task<Either<string, IEnumerable<string>>> GetPetNamesByGenderAsync(Gender g)
        {
            try
            {
                return (await GetPeople())
                       .Where(p => p.Pets != null &&
                                   String.Compare(p.Gender, g.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                       .Select(p => p.Pets)
                       .SelectMany(p => p)
                       .Select(p => p.Name)
                       .OrderBy(p => p)
                       .ToArray()
                       .Pipe(ps => Either<string, IEnumerable<string>>.ReturnRight(ps));
            }
            catch(Exception ex)
            {
                return Either<string, IEnumerable<string>>.ReturnLeft(ex.ToString());
            }
        }
    }
}

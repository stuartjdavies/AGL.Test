using System.Collections.Generic;
using System;

namespace AGL.Test.Solution.Tests
{
    public partial class PetRepositoryTests
    {
        public class AlternateTestAlgorithm
        {
            public static string[] GetPetNamesByGender(IEnumerable<Domain.Person> ps, Domain.Gender g)
            {                
                var names = new List<string>();

                foreach (var p in ps)
                {
                    if (String.Compare(p.Gender, g.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        foreach (var pet in p.Pets)
                        {
                            names.Add(pet.Name);
                        }
                    }                        
                }

                names.Sort();
                
                return names.ToArray();
            }
        }
    }
}

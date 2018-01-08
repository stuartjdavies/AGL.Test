﻿using System.Collections.Generic;
using System;
using System.Linq;

namespace AGL.Test.Solution.XUnit.Tests
{
    public partial class PetRepositoryTests
    {
        public class AlternateTestAlgorithm
        {
            public static (string, string)[] GetPetNamesInAlphabeticalOrderGroupedByGender(IEnumerable<Domain.Person> ps)
            {
                var genderPetNames = new Dictionary<string, List<string>>();

                foreach (var p in ps)
                {                    
                    foreach (var pet in p.Pets)
                    {                        
                        if (genderPetNames.ContainsKey(p.Gender))
                        {
                            var l = genderPetNames[p.Gender];
                            l.Add(pet.Name);                            
                        }
                        else
                        {
                            var l = new List<string>();
                            l.Add(pet.Name);
                            genderPetNames.Add(p.Gender, l);
                        }
                    }                        
                }

                genderPetNames["Male"].Sort();
                genderPetNames["Female"].Sort();

                var males = genderPetNames["Male"].ToArray().Select(x => ("Male", x));
                var females = genderPetNames["Female"].ToArray().Select(x => ("Female", x));

                return males.Concat(females).ToArray();
            }
        }
    }
}

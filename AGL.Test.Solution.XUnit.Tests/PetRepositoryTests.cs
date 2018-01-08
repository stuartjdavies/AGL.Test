using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Xunit;
using Fp.Common.Monads.EitherMonad;
using System;
using AGL.Test.Solution.Data.PetApi;

namespace AGL.Test.Solution.XUnit.Tests
{    
    public partial class PetRepositoryTests
    {
        #region SampleTestData
        public string SampleTestJson = @"[
   {
      'name':'Bob',
      'gender':'Male',
      'age':23,
      'pets':[
         {
            'name':'Garfield',
            'type':'Cat'
         },
         {
            'name':'Fido',
            'type':'Dog'
         }
      ]
   },
   {
      'name':'Jennifer',
      'gender':'Female',
      'age':18,
      'pets':[
         {
            'name':'Garfield',
            'type':'Cat'
         }
      ]
   },
   {
      'name':'Steve',
      'gender':'Male',
      'age':45,
      'pets':null
   },
   {
      'name':'Fred',
      'gender':'Male',
      'age':40,
      'pets':[
         {
            'name':'Tom',
            'type':'Cat'
         },
         {
            'name':'Max',
            'type':'Cat'
         },
         {
            'name':'Sam',
            'type':'Dog'
         },
         {
            'name':'Jim',
            'type':'Cat'
         }
      ]
   },
   {
      'name':'Samantha',
      'gender':'Female',
      'age':40,
      'pets':[
         {
            'name':'Tabby',
            'type':'Cat'
         }
      ]
   },
   {
      'name':'Alice',
      'gender':'Female',
      'age':64,
      'pets':[
         {
            'name':'Simba',
            'type':'Cat'
         },
         {
            'name':'Nemo',
            'type':'Fish'
         }
      ]
   }
]";
        #endregion

        [Fact]
        public async void PetRepository_BasedOnProvidedData_Test()
        {
            var verificationSet = new[] { ("Male", "Fido"), ("Male", "Garfield"), ("Male", "Jim"), ("Male", "Max"), ("Male", "Sam"), ("Male", "Tom"),
                                          ("Female", "Garfield"), ("Female", "Nemo"), ("Female", "Simba"), ("Female", "Tabby") };

            var people = JsonConvert.DeserializeObject<Domain.Person[]>(SampleTestJson);

            var r = new PetRespository(() => Task.FromResult(people.Select(x => x)));

            var result = (await r.GetPetNamesInAlphabeticalOrderGroupedByGenderAsync())                                   
                         .Match(left => throw new Exception($"Error: {left}"),
                                        right => right.Select(g => g.PetNames.Select(y => (g.Gender, y)))
                                                  .SelectMany(x => x).ToArray());

            Assert.True(result.SequenceEqual(verificationSet) == true,
                        "Result doesn't match verification set");
        }

        [Fact]
        public async void PetRepository_CheckAgainstAlternateAlgorithm_Test()
        {
            // 
            // Generate 100 random generated people
            //
            var people = PersonGenerator.Generate(3, 10).Generate(100);

            var r = new PetRespository(() => Task.FromResult(people.Select(x => x)));
          
            var verificationSet = AlternateTestAlgorithm.GetPetNamesInAlphabeticalOrderGroupedByGender(people);
            var result = (await r.GetPetNamesInAlphabeticalOrderGroupedByGenderAsync())
                         .Match(left => throw new Exception($"Error: {left}"),
                                       right => right.Select(g => g.PetNames.Select(y => (g.Gender, y)))
                                                     .SelectMany(x => x)
                                                     .OrderByDescending(x => x.Gender)
                                                     .ToArray());

            Assert.True(result.SequenceEqual(verificationSet) == true,
                        "Result doesn't match verification set");
        }

        [Fact]
        public async void PetRepository_CheckIfApiFailureAreHandledCorrectly_Test()
        {
            // 
            // Generate 100 random generated people
            //           
            var r = new PetRespository(() => throw new Exception("Failed in getting data"));

            var males = await r.GetPetNamesInAlphabeticalOrderGroupedByGenderAsync();
                        
            var errorMsg = males.Match(left => left, right => string.Empty);

            Assert.True(errorMsg.Contains("Failed in getting data"), "Expected to receive error message");
        }
    }
}

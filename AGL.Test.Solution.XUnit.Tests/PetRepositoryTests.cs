using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Xunit;
using Fp.Common.Monads.RopResultMonad;
using System;
using AGL.Test.Solution.Data.PetApi;
using FluentAssertions;
using AGL.Test.Solution.Domain;

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

            var r = new PetRespository(() => Task.FromResult(RopResult<Person[], DomainEvent[]>.ReturnSuccess((people.Select(x => x).ToArray(), new DomainEvent[] { }))));            

            (await r.GetPetNamesInAlphabeticalOrderGroupedByGenderAsync())
            .Match(success => success
                              .Result
                              .Select(g => g.PetNames.Select(y => (g.Gender, y)))
                              .SelectMany(y => y)
                              .ToArray(),
                   failure => throw new Exception($"Error: {failure}"))            
            .Should()
            .BeEquivalentTo(verificationSet, "Result doesn't match verification set");            
        }

        [Fact]
        public async void PetRepository_CheckAgainstAlternateAlgorithm_Test()
        {
            // 
            // Generate 100 random generated people
            //
            var petOwners = PetOwnerGenerator.Generate(3, 10).Generate(100);

            var r = new PetRespository(() => Task.FromResult(RopResult<Person[], DomainEvent[]>.ReturnSuccess((petOwners.Select(x => x).ToArray(), new DomainEvent[] { }))));

            var verificationSet = AlternateTestAlgorithm.GetPetNamesInAlphabeticalOrderGroupedByGender(petOwners);

            (await r.GetPetNamesInAlphabeticalOrderGroupedByGenderAsync())
            .Match(success => success
                              .Result
                              .Select(g => g.PetNames.Select(y => (g.Gender, y)))
                              .SelectMany(x => x)
                              .OrderByDescending(x => x.Gender)
                              .ToArray(),
                    failure => throw new Exception($"Error: {failure}"))
            .Should()
            .BeEquivalentTo(verificationSet, "Result doesn't match verification set");
        }

        [Fact]
        public async void PetRepository_CheckIfApiFailureAreHandledCorrectly_Test()
        {
            var es = new DomainEvent[] { new DomainEvent("FAILURE", "TEST", EventLevel.Error, "Failed in getting data") };           
            
            string errorMsg = (await (new PetRespository(() => Task.FromResult((RopResult<Person[], DomainEvent[]>)RopResult<Person[], DomainEvent[]>.ReturnFailure(es))))
                              .GetPetNamesInAlphabeticalOrderGroupedByGenderAsync())
                              .Match(success => throw new Exception($"Should receive success message"),
                                     failure => failure[0].Body.ToString());

            errorMsg.Should().Contain("Failed in getting data");
        }
    }
}

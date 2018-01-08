using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
using Fp.Common.Monads.EitherMonad;
using Newtonsoft.Json;

namespace AGL.Test.Solution.Tests
{
    [TestClass]
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

        [TestMethod]
        public void PetRepository_BasedOnProvidedData_Test()
        {
            var people = JsonConvert.DeserializeObject<Domain.Person[]>(SampleTestJson);

            var r = new PetRespository(() => Task.FromResult(people.Select(x => x)));

            var males = Task.Run(async () => await r.GetPetNamesByGenderAsync(Domain.Gender.Male))
                            .GetAwaiter()
                            .GetResult();

            var females = Task.Run(async () => await r.GetPetNamesByGenderAsync(Domain.Gender.Female))
                              .GetAwaiter()
                              .GetResult();

            Assert.IsTrue(males.Match(left => throw new System.Exception($"Error: {left}"),
                                      right => right.SequenceEqual(new[] { "Fido", "Garfield", "Jim", "Max", "Sam", "Tom" })), 
                                      "Doesn't not match test result given in question");

            Assert.IsTrue(females.Match(left => throw new System.Exception($"Error: {left}"),
                                        right => right.SequenceEqual(new[] { "Garfield", "Nemo", "Simba", "Tabby" })),
                                        "Doesn't not match test result given in question");
        }

        [TestMethod]
        public void PetRepository_CheckAgainstAlternateAlgorithm_Test()
        {
            // 
            // Generate 100 random generated people
            //
            var people = PersonGenerator.Generate(3, 30).Generate(100);           

            var r = new PetRespository(() => Task.FromResult(people.Select(x => x)));

            // 
            // Generate Alternative solution
            //
            var testMales = AlternateTestAlgorithm.GetPetNamesByGender(people, Domain.Gender.Male);
            var testFemales = AlternateTestAlgorithm.GetPetNamesByGender(people, Domain.Gender.Female);

            var males = Task.Run(async () => await r.GetPetNamesByGenderAsync(Domain.Gender.Male))
                            .GetAwaiter()
                            .GetResult();


            var females = Task.Run(async () => await r.GetPetNamesByGenderAsync(Domain.Gender.Female))
                              .GetAwaiter()
                              .GetResult();

            Assert.IsTrue(males.Match(left => throw new System.Exception($"Error: {left}"),
                                      right => right.SequenceEqual(testMales)),
                                      "Alternative solution doesn't match solution for males");

            Assert.IsTrue(females.Match(left => throw new System.Exception($"Error: {left}"),
                                        right => right.SequenceEqual(testFemales)),
                                        "Alternative solution doesn't match solution for females");
        }
    }
}

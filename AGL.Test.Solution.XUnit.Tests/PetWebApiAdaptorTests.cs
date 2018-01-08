using AGL.Test.Solution.Data.PetApi;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace AGL.Test.Solution.XUnit.Tests
{    
    public class PetApiHttpAdaptorTests
    {

        [Fact]
        public async void PetWebApiGetPeople_BasicIntegrationTest()
        {
            var mockServiceUrl = "https://raw.githubusercontent.com/stuartjdavies/AGL.Test/master/people.json";
            ////var mockServiceUrl = "http://agl-developer-test.azurewebsites.net/people.json";

            var petOwners = await PetServiceHttpAdaptor.GetPetOwners(mockServiceUrl);

            petOwners.Should().HaveCountGreaterThan(0, "We should receive at least one person");                        
            Assert.True(petOwners.Any(x => x.Pets.Any()), "We should receive some pets");
        }
    }   
}

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

            (await PetServiceHttpAdaptor.GetPetOwners(mockServiceUrl))
            .Should()
            .HaveCountGreaterThan(0, "We should receive at least one person")
            .And
            .Contain(x => x.Pets.Any(), "We should receive some pets");            
        }
    }   
}

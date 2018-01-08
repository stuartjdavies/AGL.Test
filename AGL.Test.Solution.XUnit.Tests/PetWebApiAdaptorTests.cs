using AGL.Test.Solution.Data.PetApi;
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

            var peoples = await PetServiceHttpAdaptor.GetPetOwners(mockServiceUrl);


            Assert.True(peoples.Any(), "We should receive at least one person");
            Assert.True(peoples.Any(x => x.Pets.Any()), "We should receive some pets");
        }
    }   
}

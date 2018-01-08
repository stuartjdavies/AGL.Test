using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;

namespace AGL.Test.Solution.Tests
{
    [TestClass]
    public class PetWebApiAdaptorTests
    {

        [TestMethod]
        public void PetWebApiGetPeople_BasicIntegrationTest()
        {            
            var mockServiceUrl = "https://raw.githubusercontent.com/stuartjdavies/AGL.Test/master/people.json";
            //var mockServiceUrl = "http://agl-developer-test.azurewebsites.net/people.json";

            var peoples = Task.Run(async () => await PetWebApiAdaptor.GetPeoples(mockServiceUrl))
                              .GetAwaiter()
                              .GetResult()
                              .ToArray();

            Assert.IsTrue(peoples.Any(), "We should receive at least one person");
            Assert.IsTrue(peoples.Any(x => x.Pets.Any()), "We should receive some pets");
        }
    }
}

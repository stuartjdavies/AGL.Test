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
            var peoples = Task.Run(async () => await PetWebApiAdaptor.GetPeoples("http://agl-developer-test.azurewebsites.net/"))
                              .GetAwaiter()
                              .GetResult()
                              .ToArray();

            Assert.IsTrue(peoples.Any(), "We should receive at least one person");
            Assert.IsTrue(peoples.Any(x => x.Pets.Any()), "We should receive some pets");
        }
    }
}

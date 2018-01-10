using AGL.Common;
using AGL.Test.Solution.Domain;
using FluentAssertions;
using Fp.Common.Monads.RopResultMonad;
using System.Linq;
using Xunit;

namespace AGL.Test.Solution.XUnit.Tests
{    
    public class HttpAdaptorTests
    {
        [Fact]
        public async void HttpAdaptor_BasicIntegrationGetTest()
        {
            var mockServiceUrl = "https://raw.githubusercontent.com/stuartjdavies/AGL.Test/master/people.json";
            ////var mockServiceUrl = "http://agl-developer-test.azurewebsites.net/people.json";

            (await HttpAdaptor.GetAsync<Person[]>(mockServiceUrl))
            .Match(success => success.Result,
                   failure => throw new System.Exception("Should not get here"))
            .Should()
            .HaveCountGreaterThan(0, "We should receive at least one person")
            .And
            .Contain(x => x.Pets.Any(), "We should receive some pets");            
        }
    }   
}

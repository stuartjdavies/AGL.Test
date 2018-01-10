using AGL.Common;
using AGL.Test.Solution.Data.PetApi;
using AGL.Test.Solution.Domain;
using Fp.Common.Monads.RopResultMonad;
using SimpleInjector;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AGL.Test.Solution.Console
{
    class Program
    {
        static readonly Container container;

        static Program()
        {
            var serviceUrl = "https://raw.githubusercontent.com/stuartjdavies/AGL.Test/master/people.json";
            // var serviceUrl = "http://agl-developer-test.azurewebsites.net/people.json";

            container = new Container();
            container.Register<IPetRepository>(() => new PetRespository(async () => await HttpAdaptor.GetAsync<Person[]>(serviceUrl)));            
            container.Verify();
        }

        static void Main(string[] args)
        {
            var r = container.GetInstance<IPetRepository>();

            var petNames = Task.Run(async () => await r.GetPetNamesInAlphabeticalOrderGroupedByGenderAsync())
                               .GetAwaiter()
                               .GetResult();   
            
            string BuildDisplayResultString((List<(string Gender, List<string> PetNames)> Result, DomainEvent[] Messages) success)
            {                
                var sb = new StringBuilder();

                foreach (var g in success.Result)
                {
                    sb.AppendLine(g.Gender);

                    foreach (var name in g.PetNames)
                    {
                        sb.AppendLine($"\t{name}");
                    }
                }

                return sb.ToString();
            };

            string BuildDisplayErrorString(DomainEvent[] es)
            {
                var sb = new StringBuilder();

                foreach (var e in es)
                {
                    sb.AppendLine($"EventType: {e.EventType}");
                    sb.AppendLine($"Body: {e.Body}");                    
                }

                return sb.ToString();
            }

            System.Console.WriteLine(petNames.Match(BuildDisplayResultString,
                                                    BuildDisplayErrorString));            
            
            System.Console.ReadKey();
        }
    }
}

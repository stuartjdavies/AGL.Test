using AGL.Test.Solution.Data.PetApi;
using AGL.Test.Solution.Domain;
using Fp.Common.Monads.EitherMonad;
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
            container.Register<IPetRepository>(() => new PetRespository(() => PetServiceHttpAdaptor.GetPetOwners(serviceUrl)));
            container.Verify();
        }

        static void Main(string[] args)
        {
            var r = container.GetInstance<IPetRepository>();

            var petNames = Task.Run(async () => await r.GetPetNamesInAlphabeticalOrderGroupedByGenderAsync())
                               .GetAwaiter()
                               .GetResult();   
            
            string BuildDisplayResultString(List<(string Gender, List<string> PetNames)> right)
            {                
                var sb = new StringBuilder();

                foreach (var g in right)
                {
                    sb.AppendLine(g.Gender);

                    foreach (var name in g.PetNames)
                    {
                        sb.AppendLine($"\t{name}");
                    }
                }

                return sb.ToString();
            };

            System.Console.WriteLine(petNames.Match(left => $"Received error {left}", BuildDisplayResultString));            
            
            System.Console.ReadKey();
        }
    }
}

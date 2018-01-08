using AGL.Test.Solution.Domain;
using Fp.Common.Monads.EitherMonad;
using SimpleInjector;
using System.Text;
using System.Threading.Tasks;

namespace AGL.Test.Solution.Console
{
    class Program
    {
        static readonly Container container;

        static Program()
        {
            container = new Container();
            container.Register<IPetRepository>(() => new PetRespository(() =>
                    PetWebApiAdaptor.GetPeoples("http://agl-developer-test.azurewebsites.net/")));
            container.Verify();
        }

        static void Main(string[] args)
        {
            var r = container.GetInstance<IPetRepository>();

            var males = Task.Run(async () => await r.GetPetNamesByGenderAsync(Gender.Male))
                            .GetAwaiter()
                            .GetResult();

            var females = Task.Run(async () => await r.GetPetNamesByGenderAsync(Gender.Female))
                              .GetAwaiter()
                              .GetResult();

            System.Console.WriteLine("Male");

            System.Console.WriteLine(males.Match(left => $"Received error {left}",
                                                 right => {
                                                    var sb = new StringBuilder();
                                                    foreach (var item in right) sb.AppendLine(item);
                                                    return sb.ToString();
                                                 }));

            System.Console.WriteLine("\rFemale");
            System.Console.WriteLine(females.Match(left => $"Received error {left}",
                                                   right => {
                                                       var sb = new StringBuilder();
                                                       foreach (var item in right) sb.AppendLine(item);
                                                       return sb.ToString();
                                                   }));
            
            System.Console.ReadKey();
        }
    }
}

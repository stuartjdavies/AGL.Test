using AGL.Test.Solution.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AGL.Test.Solution.Data.PetApi
{
    public class PetServiceHttpAdaptor 
    {      
        public static async Task<IEnumerable<Person>> GetPetOwners(string url)
        {
            using (var client = new HttpClient())
            {                
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                var v = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Person[]>(v);
            };
        }

    }
}

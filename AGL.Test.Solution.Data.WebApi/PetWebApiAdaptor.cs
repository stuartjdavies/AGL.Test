using AGL.Test.Solution.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AGL.Test.Solution
{
    public class PetWebApiAdaptor 
    {      
        public static async Task<IEnumerable<Person>> GetPeoples(string baseAddress)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("/people.json");
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                return await response.Content.ReadAsAsync<Person[]>();
            };
        }

    }
}

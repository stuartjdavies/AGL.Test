using AGL.Test.Solution.Domain;
using Fp.Common.Monads.RopResultMonad;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AGL.Common
{
    public class HttpAdaptor
    {
        public static async Task<RopResult<T, DomainEvent[]>> GetAsync<T>(string url)
        {
            try
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

                    return RopResult<T, DomainEvent[]>.ReturnSuccess(
                                (JsonConvert.DeserializeObject<T>(v), 
                                new[] { new DomainEvent($"HTTP_GET_SUCCESS", "HttpAdaptor", EventLevel.Info, $"GET { url }") }));
                };
            }
            catch(Exception ex)
            {
                return RopResult<T, DomainEvent[]>.ReturnFailure(
                                new[] { new DomainEvent($"HTTP_GET_FAILURE", "HttpAdaptor", EventLevel.Info, $"GET FAILURE with message {ex.Message}") });

            }
        }

    }
}

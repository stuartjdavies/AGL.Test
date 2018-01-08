using Newtonsoft.Json;

namespace AGL.Test.Solution.Domain
{
    public partial class Person
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("age")]
        public long Age { get; set; }

        [JsonProperty("pets")]
        public Pet[] Pets { get; set; }
    }

    public partial class Pet
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Person
    {
        public static Person[] FromJson(string json) => JsonConvert.DeserializeObject<Person[]>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Person[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}

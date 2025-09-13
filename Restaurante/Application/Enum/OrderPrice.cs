using System.Text.Json.Serialization;

namespace Application.Enum
{    public enum OrderPrice
    {

        [JsonConverter(typeof(JsonStringEnumConverter))]
        asc,
        desc
    }
}

using System.Text.Json.Serialization;

namespace IASD.Sonoplastia.Data
{
    public class FileInfo
    {
        [JsonPropertyName("titulo")]
        public string Titulo { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("data")]
        public string Data { get; set; }
    }
}

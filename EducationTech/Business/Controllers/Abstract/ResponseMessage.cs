using Newtonsoft.Json;

namespace EducationTech.Business.Controllers.Abstract
{
    public class ResponseMessage
    {
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public int Status { get; set; } = 200;

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public object? Message { get; set; } = null;

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object? Data { get; set; } = null;

        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public object? Errors { get; set; } = null;
    }
}

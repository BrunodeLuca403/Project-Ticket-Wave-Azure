using Newtonsoft.Json;

namespace TicketAzure.Core.Dto.CreateEvent
{
    public class ResponseEfi
    {
        [JsonProperty("txid")]
        public string? TxId { get; set; }
        [JsonProperty("pixCopiaECole")]
        public string? PixCopyAndPast { get; set; }
    }
}

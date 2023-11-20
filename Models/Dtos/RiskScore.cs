using System.Text.Json.Serialization;
namespace FrmApp.Models.Dtos;



public abstract class RiskScoreBase{



        [JsonPropertyName("requestUUID")] 
        public string RequestUuid { get; set; } = null!;

        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }= null!;

        [JsonPropertyName("requestTime")]
        public DateTime RequestTime { get; set; }= DateTime.Now;

        [JsonPropertyName("custID")]
        public string CustId { get; set; }= null!;

        [JsonPropertyName("mobileNumber")]
        public string MobileNumber { get; set; }= null!;

}
public  class RiskScoreRequestDTO: RiskScoreBase
    {    

        [JsonPropertyName("remAccNumber")]
        public string RemAccNumber { get; set; }= null!;

        [JsonPropertyName("benAccNumber")]
        public string BenAccNumber { get; set; }=null!;

        [JsonPropertyName("benIfsc")]
        public string BenIfsc { get; set; } = null!;

        [JsonPropertyName("amount")]
        public int Amount { get; set; } = 0;

        [JsonPropertyName("transferType")]
        public string TransferType { get; set; } = "DUMMY";
    }


    public  class RiskScoreResponseDTO: RiskScoreBase
    {

        [JsonPropertyName("respCategory")]
        public string RespCategory { get; set; }

        [JsonPropertyName("respCode")]
        public long RespCode { get; set; }

        [JsonPropertyName("respDesc")]
        public string RespDesc { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("rrn")]
        public string Rrn { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("failedAt")]
        public object FailedAt { get; set; }

        [JsonPropertyName("riskscore")]
        public long Riskscore { get; set; }
    }
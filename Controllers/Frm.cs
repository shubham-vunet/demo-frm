using FrmApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FrmApp.Controllers;

[Route("frm/{action}")]
public class FrmController : Controller
{
    static Random r = new Random();
    static float ep = 0;
    [HttpPost]
    public ActionResult<RiskScoreResponseDTO> RiskScore([FromBody] RiskScoreRequestDTO req)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var resp = new RiskScoreResponseDTO(){
            CustId = req.CustId,
            ErrorMessage = "",
            FailedAt = "",
            MobileNumber = req.MobileNumber,
            RequestId = req.RequestId,
            RequestTime = req.RequestTime,
            RequestUuid = req.RequestUuid,
            RespCategory = "Approved",
            RespCode = 0,
            RespDesc = "Success",
            Riskscore = GetRiskScore(),
            Rrn = "1234",
            Status = "Success"            
        };
        return resp;
    }
    
    [HttpPost]
    public IActionResult Signal([FromBody] FrmSignalRequestDTO req){
        if(!ModelState.IsValid) return BadRequest(ModelState);
        if(req.Signal == "frm-err"){

        }
        return Ok();
    }
    private static short GetRiskScore() {
        return (short)r.NextInt64(60,100);
    }
}
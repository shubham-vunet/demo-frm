using FrmApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FrmApp.Controllers;

[Route("frm/{action}")]
public class FrmController : Controller
{
    static Random r = new Random();
    static int errorRate = 50;
    static int delayInSec = 1;


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
            if(req.Active){
              errorRate = req.ExpectedFailurePct ?? 50;
            }else{
                errorRate = -100;
            }
        }else if(req.Signal == "frm-slow"){
            if(req.Active){
              delayInSec = req.ExpectedDelayInSec ?? 0;
            }else{
                delayInSec = 0;
            }
        }
        return Ok();
    }
    private static short GetRiskScore() {
        var ere = r.Next(100);
        if(delayInSec > 0){
            var delay = r.Next(delayInSec * 1000);
            Thread.Sleep(delay);
        }
        if(ere + errorRate > 100){
            throw new Exception("Failed");
        }
        return (short)r.NextInt64(65,101);
    }
}
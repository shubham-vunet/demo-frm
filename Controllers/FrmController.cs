using FrmApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FrmApp.Controllers;

[Route("frm/{action}")]
public class FrmController : Controller
{
    private readonly ILogger<FrmController> _logger;
    private static Random _rng = new Random();
    private static int _errorRate = 0;
    private static int _delayInSec = 0;

    public FrmController(ILogger<FrmController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public ActionResult<RiskScoreResponseDTO> RiskScore([FromBody] RiskScoreRequestDTO req)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        var rrn = getRRN();
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
            Rrn = rrn,
            Status = "Success"            
        };
        return resp;
    }
    
    [HttpPost]
    public IActionResult Signal([FromBody] FrmSignalRequestDTO req){
        if(!ModelState.IsValid) return BadRequest(ModelState);
        _logger.LogInformation(@"2023-10-15 07:44:38.723+0000 [http-nio-18080-exec-10] INFO  FrmApp.Controllers.FrmController - msg [Request to FRM for getting risk score] ReqData [requestUUID=VEBA867901, requestId=MUC94129697, requestTime=13:14:38.716389657, custID=BVB44665, mobileNumber=2827660661, remAccNumber=2147483647, benAccNumber=1, benIfsc=DMBL493004, amount=111.0, transferType=IMPS] metaData [URL=http://localhost:18082/frm/riskscore] spanContext [traceId=ab617801dea84dd6ec250b21baa6e441, spanId=4823a33410f7f75b]");
        if(req.Signal == "frm-err"){
            if(req.Active){
              _errorRate = req.ExpectedFailurePct ?? 50;
            }else{
                _errorRate = -100;
            }
        }else if(req.Signal == "frm-slow"){
            if(req.Active){
              _delayInSec = req.ExpectedDelayInSec ?? 0;
            }else{
                _delayInSec = 0;
            }
        }
        return Ok();
    }
    private static short GetRiskScore() {
        var ere = _rng.Next(100);
        if(_delayInSec > 0){
            var delay = _rng.Next(_delayInSec * 1000);
            Thread.Sleep(delay);
        }
        if(ere + _errorRate > 100){
            throw new Exception("Failed");
        }
        return (short)_rng.NextInt64(65,101);
    }
   private static string getRRN(){
        var rrn =  _rng.NextInt64(1000000000,9000000000);
        return rrn.ToString();
    }
}
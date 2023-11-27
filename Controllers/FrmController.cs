using frm.Models;
using FrmApp.Models.Dtos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FrmApp.Controllers;

[Route("frm/{action}")]
public class FrmController : Controller
{
  private readonly ILogger<FrmController> _logger;
  private readonly FrmContext _context;
  private static Random _rng = new Random();
  private static int _errorRate = 0;
  private static int _delayInSec = 0;

  public FrmController(ILogger<FrmController> logger, FrmContext context)
  {
    _logger = logger;
    _context = context;
  }

  [HttpPost]
  public async Task<ActionResult<RiskScoreResponseDTO>> RiskScore([FromBody] RiskScoreRequestDTO req)
  {
    try
    {
      if (!ModelState.IsValid) return BadRequest(ModelState);


      _logger.LogInformation($"[Request to FRM for getting risk score] ReqData [requestUUID={req.RequestUuid}, requestId={req.RequestId}, requestTime={req.RequestTime}, custID=${req.RequestTime}, mobileNumber=${req.MobileNumber}, remAccNumber={req.RemAccNumber}, benAccNumber={req.BenAccNumber}, benIfsc={req.BenIfsc}, amount={req.Amount}, transferType={req.TransferType}] metaData [URL={Request.GetDisplayUrl()}]");

      var rrn = getRRN();
      var resp = new RiskScoreResponseDTO()
      {
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
        RiskScore = GetRiskScore(),
        Rrn = rrn,
        Status = "Success"
      };

      _context.RiskScoreResponses.Add(resp);
      await _context.SaveChangesAsync();

      _logger.LogInformation($"[Response from FRM for risk score] RespData [requestUUID={resp.RequestUuid}, requestId={resp.RequestId}, requestTime={resp.RequestTime}, riskScore=${resp.RiskScore}]  statusData [status={resp.Status}, respCategory={resp.RespCategory}, respDesc={resp.RespDesc}, respCode={resp.RespCode}] metaData [URL={Request.GetDisplayUrl()}]");


      return resp;
    }
    catch (Exception e)
    {
      _logger.LogTrace(new EventId(), e, "Error while getting riskScore.");
      return UnprocessableEntity(ModelState);
    }
  }

  [HttpPost]
  public IActionResult Signal([FromBody] FrmSignalRequestDTO req)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    if (req.Signal == "frm-err")
    {
      if (req.Active)
      {
        _errorRate = req.ExpectedFailurePct ?? 50;
      }
      else
      {
        _errorRate = -100;
      }
    }
    else if (req.Signal == "frm-slow")
    {
      if (req.Active)
      {
        _delayInSec = req.ExpectedDelayInSec ?? 0;
      }
      else
      {
        _delayInSec = 0;
      }
    }
    return Ok();
  }
  private static short GetRiskScore()
  {
    var ere = _rng.Next(100);
    if (_delayInSec > 0)
    {
      var delay = _rng.Next(_delayInSec * 1000);
      Thread.Sleep(delay);
    }
    if (ere + _errorRate > 100)
    {
      throw new Exception("Failed");
    }
    return (short)_rng.NextInt64(65, 101);
  }
  private static string getRRN()
  {
    var rrn = _rng.NextInt64(1000000000, 9000000000);
    return rrn.ToString();
  }
}
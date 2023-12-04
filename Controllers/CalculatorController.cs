using CalculatorAPI.Models;
using CalculatorAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly MathOperationsService _mathOperationsService;

    public CalculatorController(MathOperationsService mathOperationsService)
    {
        _mathOperationsService = mathOperationsService;
    }

    [HttpPost("calculate")]
    public ActionResult<ReturnModel> Calculate([FromBody]RequestModel requestModel)
    {
        if (!_mathOperationsService.ValidateExpression(requestModel.Expression, out var message))
            return BadRequest(message);
        
        return Ok(new ReturnModel
        {
            Result = _mathOperationsService.CalculateExpression(requestModel.Expression),
        });
    }
}
using CalcAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalcAPI.Controllers
{
    [Route("calc")]
    [ApiController]
    public class CalcController : ControllerBase
    {
        [HttpGet("sum")]
        public async Task<double> GetSumResult([FromQuery] double a, [FromQuery] double b)
        {
            return a + b;
        }

        [HttpGet("sub")]
        public async Task<double> GetSubResult([FromQuery] double a, [FromQuery] double b)
        {
            return a - b;
        }

        [HttpGet("multiply")]
        public async Task<double> GetMultiplyResult([FromQuery] double a, [FromQuery] double b)
        {
            return a * b;
        }

        [HttpGet("divide")]
        public async Task<double> GetDivideResult([FromQuery] double a, [FromQuery] double b)
        {
            return a / b;
        }

        [HttpGet("pow")]
        public async Task<double>  GetPowResult([FromQuery] double a, [FromQuery] double b)
        {
            return Math.Pow(a, b);
        }
        [HttpGet("sqrt")]
        public async Task<double> GetSqrtResult([FromQuery] double a, [FromQuery] double b)
        {
            return Math.Pow(a, 1/b);
        }
        [HttpGet("complex/")]
        public async Task<List<string>> GetComplexExpressionResult([FromQuery]string expression)
        {
            ComplexExpression complexExpression = new ComplexExpression(expression);
            return complexExpression.GetParsedExpression();
        }
    }
}

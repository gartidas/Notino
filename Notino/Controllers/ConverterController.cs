using Microsoft.AspNetCore.Mvc;
using Notino.Features.ConvertFile;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Controllers
{
    [Route("api/converter")]
    public class ConverterController : BaseController
    {
        [HttpPost("convert-xml-to-json")]
        public async Task<ActionResult> ConvertXmlToJson(ConvertXmlToJson.Query query, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);

            if (result.Failed)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }
    }
}

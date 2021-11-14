using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notino.Features.DownloadFile;
using Notino.Features.UploadFile;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Controllers
{
    [Route("api/files")]
    public class FilesController : BaseController
    {
        [HttpPost("upload-file-to-disc")]
        public async Task<ActionResult> UploadFileToDisc(IFormFile file, CancellationToken cancellationToken)
        {
            var command = new UploadFileToDisc.Command { File = file };
            var result = await Mediator.Send(command, cancellationToken);

            if (result.Failed)
                return BadRequest(result.Errors);

            return Ok(result.Data);
        }

        [HttpPost("download-file-from-disc")]
        public async Task<ActionResult> DownloadFileFromDisc(DownloadFileFromDisc.Query query, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);

            if (result.Failed)
                return BadRequest(result.Errors);

            return File(result.Data.Bytes, "text/plain", Path.GetFileName(result.Data.Path));
        }

        [HttpPost("download-file-from-url")]
        public async Task<ActionResult> DownloadFileFromUrl(DownloadFileFromUrl.Query query, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);

            if (result.Failed)
                return BadRequest(result.Errors);

            return File(result.Data.Bytes, "text/plain", Path.GetFileName((new Uri(result.Data.Path)).LocalPath));
        }
    }
}

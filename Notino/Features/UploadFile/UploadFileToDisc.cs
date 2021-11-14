using MediatR;
using Microsoft.AspNetCore.Http;
using Notino.Contracts;
using Notino.WriterProviders;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Features.UploadFile
{
    public class UploadFileToDisc
    {
        public class Command : IRequest<Result<Response>>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Response>>
        {
            private readonly WriterProviderFactory _writerProviderFactory;

            public Handler(WriterProviderFactory writerProviderFactory)
            {
                _writerProviderFactory = writerProviderFactory;
            }

            public async Task<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
            {
                var writerProvider = _writerProviderFactory.GetWriterProvider(WriterTargetType.File);
                string content;

                try
                {
                    using var reader = new StreamReader(request.File.OpenReadStream());
                    content = await reader.ReadToEndAsync();
                }
                catch (Exception ex)
                {
                    return Result<Response>.Failure(ex.Message);
                }

                var path = $"StaticFiles/{request.File.FileName}";
                var result = await writerProvider.Write(path, content);

                if (result.Failed)
                    return Result<Response>.Failure(result.Errors);

                return Result<Response>.Success(new Response { Path = path });
            }
        }

        public class Response
        {
            public string Path { get; set; }
        }
    }
}

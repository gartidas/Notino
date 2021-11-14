using MediatR;
using Notino.Contracts;
using Notino.ReaderProviders;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Features.DownloadFile
{
    public class DownloadFileFromDisc
    {
        public class Query : IRequest<Result<Response>>
        {
            public string Path { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Response>>
        {
            private readonly ReaderProviderFactory _readerProviderFactory;

            public Handler(ReaderProviderFactory readerProviderFactory)
            {
                _readerProviderFactory = readerProviderFactory;
            }

            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var readerProvider = _readerProviderFactory.GetReaderProvider(ReaderSourceType.File);
                var result = await readerProvider.Read(request.Path);

                if (result.Failed)
                    return Result<Response>.Failure(result.Errors);

                return Result<Response>.Success(new Response { Path = request.Path, Bytes = result.Data });
            }
        }

        public class Response
        {
            public string Path { get; set; }
            public byte[] Bytes { get; set; }
        }
    }
}

using MediatR;
using Notino.Contracts;
using Notino.ConverterProviders;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Features.ConvertFile
{
    public class ConvertXmlToJson
    {
        public class Query : IRequest<Result<Response>>
        {
            public string Content { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Response>>
        {
            private readonly ConverterProviderFactory _converterProviderFactory;

            public Handler(ConverterProviderFactory converterProviderFactory)
            {

                _converterProviderFactory = converterProviderFactory;
            }

            public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var converterProvider = _converterProviderFactory.GetConverterProvider(ConversionType.XmlToJson);
                var result = converterProvider.Convert(request.Content);

                if (result.Failed)
                    return Result<Response>.Failure(result.Errors);

                return Result<Response>.Success(new Response { ConvertedContent = result.Data });
            }
        }

        public class Response
        {
            public string ConvertedContent { get; set; }
        }
    }
}

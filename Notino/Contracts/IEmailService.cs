using FluentEmail.Core.Models;
using Notino.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Contracts
{
    public interface IEmailService
    {
        public Task<SendResponse> SendConvertedFileEmail(string emailAddress, ConvertedFileEmailTemplateModel model, CancellationToken cancellationToken);
    }
}

using FluentEmail.Core;
using FluentEmail.Core.Models;
using Newtonsoft.Json.Linq;
using Notino.Contracts;
using Notino.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Messaging.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _pathToTemplates = $"Notino.Messaging.Email.Templates.";
        private readonly IFluentEmail _fluentEmail;

        public async Task<SendResponse> SendConvertedFileEmail(string emailAddress, ConvertedFileEmailTemplateModel model, CancellationToken cancellationToken)
        {
            var template = BuildEmailUsingTemplate(
                emailAddress,
                EmailSubjects.ConvertedFile,
                TemplateNames.ConvertedFile,
                new { RedirectUrl = model.Url }
                );

            try
            {
                return await template.SendAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return new SendResponse { ErrorMessages = new List<string>() { ex.Message } };
            }
        }

        private IFluentEmail BuildEmailUsingTemplate(string emailAddress, string subject, string template, object model)
        {
            var email = _fluentEmail
            .To(emailAddress)
            .Subject(subject)
            .UsingTemplateFromEmbedded(_pathToTemplates + template, JObject.FromObject(model), GetType().Assembly);

            return email;
        }
    }
}

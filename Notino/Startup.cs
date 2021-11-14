using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Notino.Contracts;
using Notino.ConverterProviders;
using Notino.Messaging.Email;
using Notino.Options;
using Notino.ReaderProviders;
using Notino.WriterProviders;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace Notino
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Notino.WebApi", Version = "v1" });
            });
            services.ConfigureSwaggerGen(x => x.CustomSchemaIds(xx => xx.FullName));

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddSingleton<WriterProviderFactory>();
            services.AddSingleton<ReaderProviderFactory>();
            services.AddSingleton<ConverterProviderFactory>();

            var emailOptions = new EmailOptions();
            Configuration.GetSection(nameof(EmailOptions)).Bind(emailOptions);

            services
          .AddFluentEmail(emailOptions.Email)
          .AddRazorRenderer()
          .AddSmtpSender(
              new SmtpClient(emailOptions.Host, emailOptions.Port)
              { Credentials = new NetworkCredential("apikey", emailOptions.Password), EnableSsl = true }
              );

            services.AddTransient<IEmailService, EmailService>();

            var webClientOptions = new WebClientOptions();
            Configuration.GetSection(nameof(WebClientOptions)).Bind(webClientOptions);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notino.WebApi v1"));
            }
            app.UseCors(config =>
            {
                config.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "StaticFiles")),
                RequestPath = "/StaticFiles"
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

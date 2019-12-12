using System;
using Certes;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FluffySpoon.AspNet.LetsEncrypt.Tests
{
    public class ResolutionTests
    {
        [Fact]
        public void Go()
        {
            var thing = WebHost.CreateDefaultBuilder()
                .ConfigureLogging(options => options.AddConsole())
                .ConfigureServices(services =>
                {
                    services.AddFluffySpoonLetsEncryptRenewalService(new LetsEncryptOptions()
                    {
                        Email = "some-email@github.com",
                        UseStaging = true,
                        Domains = new[] {"test.com"},
                        TimeUntilExpiryBeforeRenewal = TimeSpan.FromDays(30),
                        CertificateSigningRequest = new CsrInfo()
                        {
                            CountryName = "CountryNameStuff",
                            Locality = "LocalityStuff",
                            Organization = "OrganizationStuff",
                            OrganizationUnit = "OrganizationUnitStuff",
                            State = "StateStuff"
                        }
                    });

                    services.AddFluffySpoonLetsEncryptFileCertificatePersistence();
                    services.AddFluffySpoonLetsEncryptFileChallengePersistence();
                })
                .Configure(appBuilder => { appBuilder.UseFluffySpoonLetsEncryptChallengeApprovalMiddleware(); })
                .Build();

            thing.Services.GetRequiredService<ILetsEncryptRenewalService>();
        }
    }
}
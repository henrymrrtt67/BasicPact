using System.Collections.Generic;
using Consumer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Sdk;

namespace PactTest.ProviderTests
{
    public class BasicApiTests
    {
        //Can't guarantee this test actually works but it doesn't fail
        [Fact]
        public void Given_request_is_recieved_Then_value_is_returned()
        {
            const string ServiceUri = "http://localhost:9222";
            var config = new PactVerifierConfig {
                Outputters = new List<IOutput> { 
                       new XUnitOutput(new TestOutputHelper())
                },
                CustomHeaders = new Dictionary<string, string> { { "Authorization", "Basic VGVzdA==" } },
                Verbose = true
            };

            using (Host.CreateDefaultBuilder().ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).Build())
            {
                IPactVerifier pactVerifier = new PactVerifier(config);

                var pactUriOptions = new PactUriOptions()
                    .SetBasicAuthentication("Someone", "User");

                pactVerifier
                    .ProviderState($"{ServiceUri}/provider-states")
                    .ServiceProvider("Something API", ServiceUri)
                    .HonoursPactWith("Consumer")
                    .PactUri("..\\..\\..\\Consumer.Tests\\pacts\\consumer-something_api.json")
                    .Verify();

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Consumer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Owin.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace PactTest.ProviderTests
{
    public class BasicApiTests : IDisposable
    {
        private readonly ITestOutputHelper _output;

        public BasicApiTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Dispose()
        {
        }


        //Can't guarantee this test actually works but it doesn't fail
        [Fact]
        public void Given_request_is_recieved_Then_value_is_returned()
        {
            const string ServiceUri = "http://localhost:9222";
            var config = new PactVerifierConfig {
                Outputters = new List<IOutput> { 
                       new XUnitOutput(_output)
                },
                CustomHeaders = new Dictionary<string, string> { { "Authorization", "Basic VGVzdA==" } },
                Verbose = true
            };

            using (Host.CreateDefaultBuilder().ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseStartup<TestStartup>();
                }).Build())
            {
                IPactVerifier pactVerifier = new PactVerifier(config);

                var pactUriOptions = new PactUriOptions()
                    .SetBasicAuthentication("Someone", "User");

                pactVerifier
                    .ProviderState($"{ServiceUri}/provider-states")
                    .ServiceProvider("Basic API", ServiceUri)
                    .HonoursPactWith("Consumer")
                    .PactUri($"..{Path.DirectorySeparatorChar}Consumer.Tests{Path.DirectorySeparatorChar}pacts{Path.DirectorySeparatorChar}event_api_consumer-event_api.json")
                    .Verify();

            }
        }
    }
}

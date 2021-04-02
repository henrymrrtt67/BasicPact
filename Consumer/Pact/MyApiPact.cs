using System;
using System.Collections.Generic;
using PactNet.Models;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace Consumer.Pact
{
    public class MyApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        private int MockServerPort { get { return 9222; } } 
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

        public MyApiPact()
        {
            PactBuilder = new PactBuilder();

            PactBuilder
                .ServiceConsumer("Consumer")
                .HasPactWith("Basic API");

            MockProviderService = PactBuilder
                                    .MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactBuilder
                .Build();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consumer.Client;
using Consumer.Pact;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace PactTest.CosumerTests
{
    public class MyApiConsumerTests : IClassFixture<MyApiPact>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public MyApiConsumerTests(MyApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
        }

        [Fact]
        public void Given_something_with_id_of_tester_exists_When_get_request_is_received_Then_it_is_passed()
        {
            _mockProviderService
                  .Given("There is a something with id 'tester'")
                  .UponReceiving("A GET request to retrieve the something")
                  .With(new ProviderServiceRequest
                  {
                      Method = HttpVerb.Get,
                      Path = "/basic/tester",
                      Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                  })
                  .WillRespondWith(new ProviderServiceResponse
                  {
                      Status = 200,
                      Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                      Body = new
                      {
                          Id = "tester",
                          Number = 21
                      }
                  }); 

            var Consumer = new ApiClient(_mockProviderServiceBaseUri);

            //Act
            var Result = Consumer.GetBasicObject("tester");

            //Assert
            Assert.Equal("tester", Result.Result.Id);

            _mockProviderService.VerifyInteractions();
        }
    }
}

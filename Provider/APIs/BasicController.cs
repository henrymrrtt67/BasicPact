using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using PactNet.Mocks.MockHttpService.Models;

namespace Provider.APIs
{
    [Route("basic")]
    public class BasicController : Controller
    {
        [HttpGet]
        [RouteAttribute("{id}")]
        public ProviderServiceResponse GetBasicObject(string id)
        {
            if (id == "tester")
            {
                return new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
                    {
                        Id = "tester",
                        Number = 21
                    }
                };
            }
            return new ProviderServiceResponse();
        }
    }
}

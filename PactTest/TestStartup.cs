using Consumer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Owin;

namespace PactTest
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var apiStartup = new Startup(_configuration);

            app.UseMiddleware<ProviderStateMiddleware>();

            apiStartup.Configure(app, env);
        }
    }
}

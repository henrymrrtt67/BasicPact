using Consumer;
using Microsoft.AspNetCore.Builder;
using Owin;

namespace PactTest
{
    public class TestStartup
    {
        public void Configuration(IApplicationBuilder app)
        {
            var apiStartup = new TestStartup();

            app.UseMiddleware<ProviderStateMiddleware>();

            apiStartup.Configuration(app);
        }
    }
}

using System.Threading.Tasks;
using Artemis.Web.Client.Users;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Artemis.Web.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddApiAuthorization();

            builder.Services.AddTransient<UserUtilities>();
            builder.Services.AddTransient<HttpClientAdapter>();

            await builder.Build().RunAsync();
        }
    }
}

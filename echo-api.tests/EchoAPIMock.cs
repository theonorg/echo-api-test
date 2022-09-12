using echo_api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace echo_api.tests;

class EchoApiApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();
 
        builder.ConfigureServices(services => 
        {
            services.AddScoped(sp =>
            {
                // Replace PostgreSQL with the in memory provider for tests
                return new DbContextOptionsBuilder<EchoHistoryDb>()
                            .UseInMemoryDatabase("Tests", root)
                            .UseApplicationServiceProvider(sp)
                            .Options;
            });
        });
 
        return base.CreateHost(builder);
    }
}
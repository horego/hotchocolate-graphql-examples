using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Execution.Configuration;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.WebSockets;

namespace GraphQL.ServerApp
{
    class Startup
    {
        private readonly AppSettings m_Settings;

        public Startup(AppSettings settings)
        {
            m_Settings = settings;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureGraphQlService(services);
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
            ConfigureGraphQl(appBuilder);
        }

        void ConfigureGraphQlService(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddWebSockets(c=>{})
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddSubscriptionType<Subscription>()
                .AddInMemorySubscriptions();
        }

        void ConfigureGraphQl(IApplicationBuilder app)
        {
            app
                .UseRouting()
                .UseWebSockets()
                .UseEndpoints(c => c.MapGraphQL(m_Settings.ServiceEndpointPath));
        }
    }
}

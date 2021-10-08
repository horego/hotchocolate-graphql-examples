using System;
using System.Threading.Tasks;
using GraphQL.ClientApp.ClientApi;
using Microsoft.Extensions.DependencyInjection;
using StrawberryShake;
using StrawberryShake.Transport.WebSockets;

namespace GraphQL.ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run().GetAwaiter().GetResult();
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        public async Task Run()
        {
            await Task.Delay(TimeSpan.FromSeconds(4));
            Console.WriteLine("Trying to establish subscription");

            var settings = new AppSettings();
            var services = new ServiceCollection();

            services.AddMyClient()
                .ConfigureHttpClient(c => c.BaseAddress = settings.EndpointAddress)
                .ConfigureWebSocketClient(c =>
                {
                    c.Uri = settings.EndpointAddress.ToWebSocketAddress();
                    c.ConnectionInterceptor = new DeadSocketConnectionInterceptor(() => c, SocketDied);
                });

            var serviceProvider = services.BuildServiceProvider();

            var myClient = serviceProvider.GetRequiredService<IMyClient>();
            var watchResult = myClient.TestChanged.Watch();
            watchResult.Subscribe(i =>
            {
                Console.WriteLine(i.IsErrorResult() ? "Error..." : $"Arrived: {i.Data?.OnTestChanged?.Id}");
            });
        }

        private async Task SocketDied(IWebSocketClient socket)
        {
            Console.WriteLine($"Socked died (before calling dispose). IsClosed: {socket.IsClosed}, Inner State: {socket.Socket.State}");
            //If we don't dispose the subscription or dispose the socket we get high cpu usage (Forces Socket.IsClosed = true)
            await socket.DisposeAsync();
            Console.WriteLine($"Socked died (after calling dispose). IsClosed: {socket.IsClosed}, Inner State: {socket.Socket.State}");
        }
    }
}

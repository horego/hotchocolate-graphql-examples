using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program(args).Run();
            Console.WriteLine("Server running. Press enter to exit.");
            Console.ReadLine();
        }

        private readonly string[] m_Args;
        private readonly AppSettings m_Settings;

        public Program(string[] args)
        {
            m_Args = args;
            m_Settings = new AppSettings();
        }

        void Run()
        {
            WebHost.CreateDefaultBuilder(m_Args)
                .UseKestrel(o => o.ListenAnyIP(m_Settings.ServicePort))
                .ConfigureServices(c => c.AddSingleton(m_Settings))
                .UseStartup<Startup>()
                .Build()
                .Start();
        }
    }
}

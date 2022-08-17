namespace FacebookTransactionParser
{
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Implementations;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Startup
    {
        private IHost host;

        public Startup(string[] args)
        {
            this.BuildHost(args);
        }

        public IHost GetHost()
        {
            return this.host;
        }

        public void BuildHost(string[] args)
        {
           this.host = Host.CreateDefaultBuilder(args)
                     .ConfigureServices((_, services) =>
                        {
                        services.AddSingleton<IStatementParser, StatementParser>();
                        services.AddTransient<ProgramServiceHandler>();
                        })
                     .UseSerilog(new LoggerConfiguration().WriteTo.Console().CreateLogger())
                     .Build();
        }
    }
}

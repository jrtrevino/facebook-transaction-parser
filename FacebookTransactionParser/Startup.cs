namespace FacebookTransactionParser
{
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;
    using FacebookTransactionParser.Implementations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public class Startup
    {
        public Startup(string[] args)
        {
            this.BuildAndConfigureHost(args);
        }

        public IHost AppHost { get; set; }

        public IConfigurationRoot Configuration { get; set; }

        public void BuildAndConfigureHost(string[] args)
        {
            this.Configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

            this.AppHost = Host.CreateDefaultBuilder(args)
                     .ConfigureServices((_, services) =>
                        {
                        services.AddOptions();
                        services.Configure<AppConfig>(opts => this.Configuration.GetSection("AppConfig").Bind(opts));
                        services.AddSingleton<IStatementParser, StatementParser>();
                        services.AddSingleton<IStatementProcessor, StatementProcessor>();
                        services.AddSingleton<IEmailService, EmailService>();
                        services.AddSingleton<IEmailClientFactory, EmailClientFactory>();
                        services.AddTransient<ProgramServiceHandler>();
                        })
                    .UseSerilog((hostingContext, loggerConfiguration) =>
                        loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)).Build();
        }
    }
}

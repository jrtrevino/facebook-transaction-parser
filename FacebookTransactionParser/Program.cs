namespace FacebookTransactionParser
{
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new Startup(args).GetHost();
            var service = host.Services.GetRequiredService<ProgramServiceHandler>();
            await service.BeginProcessing();
        }
    }
}
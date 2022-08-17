namespace FacebookTransactionParser
{
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new Startup(args).GetHost();
            var service = host.Services.GetRequiredService<ProgramServiceHandler>();
            service.BeginProcessing();
        }
    }
}
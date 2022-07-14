namespace FacebookTransactionParser.Entities
{
    using FacebookTransactionParser.Contracts;

    public class Logger : ILogger
    {
        private readonly string loggingPath;

        public Logger(string loggingPath)
        {
            this.loggingPath = loggingPath;

            // create directory if doesnt exist
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(loggingPath));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not create directory for log file. {e.Message}");
            }
        }

        public void LogInfo(string message)
        {
            this.WriteToFile($"INFO: {message}");
        }

        public void LogError(string message)
        {
            this.WriteToFile($"ERROR: {message}");
        }

        private void WriteToFile(string message)
        {
            try
            {
                using (var fs = File.OpenWrite(loggingPath))
                {
                    var streamWriter = new StreamWriter(fs);
                    streamWriter.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error writing to log file. Reason: {e.Message}");
            }
        }
    }
}

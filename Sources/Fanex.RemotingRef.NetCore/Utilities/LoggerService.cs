public interface ILoggerService
{
    Task Info(string message);
}

public sealed class LoggerService : ILoggerService
{
    private readonly Fanex.Logging.ILogger logger;
    private readonly ILogger<LoggerService> loggerConsole;

    public LoggerService(Fanex.Logging.ILogger logger, ILogger<LoggerService> loggerConsole)
        => (this.logger, this.loggerConsole) = (logger, loggerConsole);

    public Task Info(string message) => logger.InfoAsync(message);

}
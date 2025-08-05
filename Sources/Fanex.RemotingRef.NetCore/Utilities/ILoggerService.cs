namespace Fanex.RemotingRef.NetCore.Utilities
{
    public interface ILoggerService
    {
        Task Info(string message);
    }

    public sealed class LoggerServiceImpl : ILoggerService
    {
        private readonly Logging.ILogger logger;
        private readonly ILogger<ILoggerService> loggerConsole;

        public LoggerServiceImpl(Logging.ILogger logger, ILogger<ILoggerService> loggerConsole)
            => (this.logger, this.loggerConsole) = (logger, loggerConsole);

        public Task Info(string message) => logger.InfoAsync(message);
    }
}
using System.Collections.Concurrent;

namespace PetHelp.API.Middlewares.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    readonly CustomLoggerProviderConfiguration loggerConfig;

    readonly ConcurrentDictionary<string, CustomerLogger> loggers =
               new ConcurrentDictionary<string, CustomerLogger>();
    public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
    {
        loggerConfig = config;
    }
    public ILogger CreateLogger(string categoryName)
    {
        return (ILogger)loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
    }

    public void Dispose()
    {
        loggers.Clear();
    }
}

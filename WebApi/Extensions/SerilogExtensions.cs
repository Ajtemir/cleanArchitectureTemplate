using Serilog;

namespace WebApi.Extensions;

public static class SerilogExtensions
{
    public static ILoggingBuilder AddConfiguredSerilog(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        
        builder.ClearProviders();
        builder.AddSerilog(logger);
        
        return builder;
    }
}

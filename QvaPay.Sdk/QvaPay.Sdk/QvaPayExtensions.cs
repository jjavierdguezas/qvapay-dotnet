using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QvaPay.Sdk.Callback;
using System;

namespace QvaPay.Sdk
{
    public static class QvaPayExtensions
    {
        public static IServiceCollection AddQvaPayClient(this IServiceCollection services, Action<QvaPayClientConfiguration> configurator = null)
        {
            if (configurator is null)
            {
                services.AddSingleton<QvaPayAuthConfiguration>();
            }
            else
            {
                var config = new QvaPayClientConfiguration();
                configurator(config);

                if (!string.IsNullOrWhiteSpace(config.AppConfigJsonPrefix))
                {
                    services.AddSingleton(c => new QvaPayAuthConfiguration(c.GetService<IConfiguration>(), config.AppConfigJsonPrefix));
                }
                else if (!string.IsNullOrWhiteSpace(config.AppId) && !string.IsNullOrWhiteSpace(config.AppSecret))
                {
                    services.AddSingleton(new QvaPayAuthConfiguration(config.AppId, config.AppSecret));
                }
                else
                {
                    services.AddSingleton<QvaPayAuthConfiguration>();
                }
            }

            services.AddHttpClient<IQvaPayClient, QvaPayClient>();

            return services;
        }

        public static IQvaPayClient GetQvaPayClient(this ServiceProvider provider)
        {
            return provider.GetService<IQvaPayClient>();
        }

        public static IEndpointConventionBuilder MapQvaPayCallback(this IEndpointRouteBuilder endpoints, string pattern)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<QvaPayCallbackMiddleware>()
                .Build();

            return endpoints.Map(pattern, pipeline).WithDisplayName("QvaPay callback endpoint");
        }
    }
}

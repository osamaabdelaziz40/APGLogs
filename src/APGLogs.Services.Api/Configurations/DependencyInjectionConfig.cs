using System;
using System.Configuration;
using APGLogs.Domain.Models;
using APGLogs.Infra.CrossCutting.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace APGLogs.Services.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
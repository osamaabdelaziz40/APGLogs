using System;
using System.Configuration;
using APGLogs.Domain.Interfaces;
using APGLogs.Domain.Models;
using APGLogs.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace APGLogs.Services.Api.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.Configure<APGLogDatabaseSettings>(
                configuration.GetSection(nameof(APGLogDatabaseSettings)));

            services.AddSingleton<IAPGLogDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<APGLogDatabaseSettings>>().Value);


            //BsonSerializer.RegisterSerializer(typeof(DateTime),
            //             new DateTimeSerializer(DateTimeKind.Local));

            services.AddDbContext<APGFundamentalContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<EventStoreSqlContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
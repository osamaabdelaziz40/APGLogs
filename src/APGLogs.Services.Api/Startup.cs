using APGLogs.BackgroundJobs.Interfaces;
using APGLogs.Domain.Models;
using APGLogs.Infra.CrossCutting.Identity;
using APGLogs.Infra.Data.Repository.EventSourcing;
using APGLogs.Services.Api.Configurations;
using APGLogs.Services.Api.Middleware;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using NetDevPack.Identity;
using NetDevPack.Identity.User;
using System;

namespace APGLogs.Services.Api
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // WebAPI Config
            services.AddControllers();

            // Setting DBContexts
            services.AddDatabaseConfiguration(Configuration);

            // ASP.NET Identity Settings & JWT
            services.AddApiIdentityConfiguration(Configuration);

            // Interactive AspNetUser (logged in)
            // NetDevPack.Identity dependency
            services.AddAspNetUserConfiguration();

            // AutoMapper Settings
            services.AddAutoMapperConfiguration();

            // Swagger Config
            services.AddSwaggerConfiguration();

            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));

            

            // .NET Native DI Abstraction
            services.AddDependencyInjectionConfiguration();


            #region Hangfire 
            var Conn = Configuration.GetValue<string>(
                "APGLogDatabaseSettings:ConnectionString");

            var DB_Name = Configuration.GetValue<string>(
                "APGLogDatabaseSettings:DatabaseName");
            var options = new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new DropMongoMigrationStrategy(),
                    BackupStrategy = new NoneMongoBackupStrategy()
                }
            };
            services.AddHangfire(x => x.UseMongoStorage(connectionString: Conn, databaseName:DB_Name,options));
            services.AddHangfireServer();
            //services.AddHostedService<HangFireHostedService>();

            #endregion


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Hangfire Backgroung Task 
            //app.UseHangfireServer();
            app.UseHangfireDashboard();
            RunBackGroundJobs();

            #endregion


            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new JsonExceptionMiddleware().Invoke
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            // NetDevPack.Identity dependency
            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerSetup();
        }


        public void RunBackGroundJobs()
        {
            var TaskPerformedEveryDays = Configuration.GetValue<string>("BackgroundClearTaskSettings:TaskPerformedEveryDays");

            RecurringJob.AddOrUpdate<ISMSLogJobService>(service => service.ClearSMSLog(), Cron.Daily(int.Parse(TaskPerformedEveryDays)));
            RecurringJob.AddOrUpdate<IEmailLogJobService>(service => service.ClearEmailLog(), Cron.Daily(int.Parse(TaskPerformedEveryDays)));
            RecurringJob.AddOrUpdate<IExceptionLogJobService>(service => service.ClearExceptionLog(), Cron.Daily(int.Parse(TaskPerformedEveryDays)));
            RecurringJob.AddOrUpdate<ICommunicationLogJobService>(service => service.ClearCommunicationLog(), Cron.Daily(int.Parse(TaskPerformedEveryDays)));
            RecurringJob.AddOrUpdate<IAMSBalanceAuditJobService>(service => service.ClearAMSBalanceAudit(), Cron.Daily(int.Parse(TaskPerformedEveryDays)));
            RecurringJob.AddOrUpdate<IAMSTransactionAuditJobService>(service => service.ClearAMSTransactionAudit(), Cron.Daily(int.Parse(TaskPerformedEveryDays)));
            RecurringJob.AddOrUpdate<IPortalSessionAuditActionJobService>(service => service.ClearPortalSessionAuditAction(), Cron.Daily(int.Parse(TaskPerformedEveryDays)));
            RecurringJob.AddOrUpdate<IPortalSessionAuditJobService>(service => service.ClearPortalSessionAudit(), Cron.Daily(int.Parse(TaskPerformedEveryDays)));
            RecurringJob.AddOrUpdate<IShadowBalanceAuditJobService>(service => service.ClearShadowBalanceAudit(), Cron.Daily(int.Parse(TaskPerformedEveryDays)));
        }


    }
}

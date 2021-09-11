using APGFundamentals.DomainHelper.Services;
using APGLogs.Application.Interfaces;
using APGLogs.Application.Services;
using APGLogs.Domain.Commands;
using APGLogs.Domain.Core.Events;
using APGLogs.Domain.Events;
using APGLogs.Domain.Interfaces;
using APGLogs.DomainHelper.Interfaces;
using APGLogs.Infra.CrossCutting.Bus;
using APGLogs.Infra.Data.Context;
using APGLogs.Infra.Data.EventSourcing;
using APGLogs.Infra.Data.Repository;
using APGLogs.Infra.Data.Repository.EventSourcing;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Mediator;

namespace APGLogs.Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // Application
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            services.AddScoped<IExceptionLogAppService, ExceptionLogAppService>();
            services.AddScoped<IExceptionLogTypeAppService, ExceptionLogTypeAppService>();
            services.AddScoped<ICommunicationLogAppService, CommunicationLogAppService>();
            services.AddScoped<ISMSLogAppService, SMSLogAppService>();
            services.AddScoped<IPortalSessionAuditAppService, PortalSessionAuditAppService>();
            services.AddScoped<IPortalSessionAuditActionAppService, PortalSessionAuditActionAppService>();
            services.AddScoped<IAMSTransactionAuditAppService, AMSTransactionAuditAppService>();
            services.AddScoped<IAMSBalanceAuditAppService, AMSBalanceAuditAppService>();
            services.AddScoped<IShadowBalanceAuditAppService, ShadowBalanceAuditAppService>();
            services.AddScoped<IEmailLogAppService, EmailLogAppService>();
            services.AddScoped<ICommunicationLogTypeAppService, CommunicationLogTypeAppService>();


            // Domain - Events
            services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<CustomerUpdatedEvent>, CustomerEventHandler>();
            services.AddScoped<INotificationHandler<CustomerRemovedEvent>, CustomerEventHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<RegisterNewCustomerCommand, ValidationResult>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCustomerCommand, ValidationResult>, CustomerCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveCustomerCommand, ValidationResult>, CustomerCommandHandler>();

            // Infra - Data
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IExceptionLogRepository, ExceptionLogRepository>();
            services.AddScoped<IExceptionLogTypeRepository, ExceptionLogTypeRepository>();
            services.AddScoped<ICommunicationLogRepository, CommunicationLogRepository>();
            services.AddScoped<ISMSLogRepository, SMSLogRepository>();
            services.AddScoped<IPortalSessionAuditRepository, PortalSessionAuditRepository>();
            services.AddScoped<IPortalSessionAuditActionRepository, PortalSessionAuditActionRepository>();
            services.AddScoped<IAMSTransactionAuditRepository, AMSTransactionAuditRepository>();
            services.AddScoped<IAMSBalanceAuditRepository, AMSBalanceAuditRepository>();
            services.AddScoped<IShadowBalanceAuditRepository, ShadowBalanceAuditRepository>();
            services.AddScoped<IEmailLogRepository, EmailLogRepository>();
            services.AddScoped<ICommunicationLogTypeRepository, CommunicationLogTypeRepository>();
            services.AddScoped<APGFundamentalContext>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSqlContext>();

            //Services
            services.AddScoped<ICSVManager, CSVManager>();
        }
    }
}
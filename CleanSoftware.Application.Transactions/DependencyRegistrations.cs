using CleanSoftware.Application.Transactions.Interfaces;
using CleanSoftware.Application.Transactions.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSoftware.Application.Transactions
{
    public static class DependencyRegistrations
    {
        public static MediatRServiceConfiguration AddTransactionBehavior<TDataUnitOfWork>(
            this MediatRServiceConfiguration configuration,
            IServiceCollection services,
            ServiceLifetime dataUnitOfWorkLifetime = ServiceLifetime.Scoped)
            where TDataUnitOfWork : IDataUnitOfWork
        {
            services.Add(
                new ServiceDescriptor(
                    typeof(IDataUnitOfWork),
                    typeof(TDataUnitOfWork),
                    dataUnitOfWorkLifetime));

            configuration.AddBehavior(
                typeof(IPipelineBehavior<,>),
                typeof(TransactionPipelineService<,>));

            return configuration;
        }
    }
}

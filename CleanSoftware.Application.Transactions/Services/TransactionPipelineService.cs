using CleanSoftware.Application.Interfaces;
using CleanSoftware.Application.Transactions.Interfaces;
using MediatR;
using System.Data;

namespace CleanSoftware.Application.Transactions.Services
{
    internal class TransactionPipelineService<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IDataUnitOfWork _data;

        public TransactionPipelineService(IDataUnitOfWork data)
        {
            _data = data;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (request is ICommand<TResponse> == false)
            {
                return await next();
            }

            try
            {
                await _data.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
                var result = await next();
                await _data.CommitTransactionAsync(DateTime.UtcNow, cancellationToken);

                return result;
            }
            catch
            {
                await _data.RollbackTransactionAsync();
                throw;
            }
        }
    }
}

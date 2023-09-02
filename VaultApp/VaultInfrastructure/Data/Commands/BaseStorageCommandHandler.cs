using MediatR;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using VaultDomain;

namespace VaultInfrastructure.Data.Commands
{
    internal abstract class BaseStorageCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly DbConnectionString _connectionString;
        private readonly IMediator _mediator;

        public BaseStorageCommandHandler(DbConnectionString connectionString, IMediator mediator)
        {
            _connectionString = connectionString;
            _mediator = mediator;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

        protected SqlConnection CreateConnection() => new SqlConnection(_connectionString.Value);

        protected async Task DispatchAllEvents(IEnumerable<BaseEvent> events)
        {
            var eventTasks = new List<Task>();
            foreach (var item in events)
            {
                eventTasks.Add(SafeDispatchEvent(item));
            }
            await Task.WhenAll(eventTasks);
        }

        private async Task SafeDispatchEvent(BaseEvent baseEvent)
        {
            try
            {
                await _mediator.Publish(baseEvent);
            }
            catch (Exception ex)
            {
                // Silent error, events should not raise exceptions to top
                Debug.WriteLine(ex);
                return;
            }
        }
    }
}

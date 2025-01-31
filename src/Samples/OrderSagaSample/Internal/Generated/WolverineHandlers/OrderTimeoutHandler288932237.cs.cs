// <auto-generated/>
#pragma warning disable
using Microsoft.Extensions.Logging;
using Wolverine.Marten.Publishing;

namespace Internal.Generated.WolverineHandlers
{
    // START: OrderTimeoutHandler288932237
    public class OrderTimeoutHandler288932237 : Wolverine.Runtime.Handlers.MessageHandler
    {
        private readonly Wolverine.Marten.Publishing.OutboxedSessionFactory _outboxedSessionFactory;
        private readonly Microsoft.Extensions.Logging.ILogger<OrderSagaSample.Order> _logger;

        public OrderTimeoutHandler288932237(Wolverine.Marten.Publishing.OutboxedSessionFactory outboxedSessionFactory, Microsoft.Extensions.Logging.ILogger<OrderSagaSample.Order> logger)
        {
            _outboxedSessionFactory = outboxedSessionFactory;
            _logger = logger;
        }



        public override async System.Threading.Tasks.Task HandleAsync(Wolverine.Runtime.MessageContext context, System.Threading.CancellationToken cancellation)
        {
            await using var documentSession = _outboxedSessionFactory.OpenSession(context);
            var orderTimeout = (OrderSagaSample.OrderTimeout)context.Envelope.Message;
            string sagaId = context.Envelope.SagaId ?? orderTimeout.Id;
            if (string.IsNullOrEmpty(sagaId)) throw new Wolverine.Persistence.Sagas.IndeterminateSagaStateIdException(context.Envelope);
            
            // Try to load the existing saga document
            var order = await documentSession.LoadAsync<OrderSagaSample.Order>(sagaId, cancellation).ConfigureAwait(false);
            if (order == null)
            {
                return;
            }

            else
            {
                order.Handle(orderTimeout, _logger);
                if (order.IsCompleted())
                {
                    
                    // Register the document operation with the current session
                    documentSession.Delete(order);
                }

                else
                {
                    
                    // Register the document operation with the current session
                    documentSession.Update(order);
                }

                
                // Commit all pending changes
                await documentSession.SaveChangesAsync(cancellation).ConfigureAwait(false);

            }

        }

    }

    // END: OrderTimeoutHandler288932237
    
    
}


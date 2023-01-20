namespace EventBusRabbitMQ.OLD.Core.v2
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        public Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
        // nothing to do...
    }

}

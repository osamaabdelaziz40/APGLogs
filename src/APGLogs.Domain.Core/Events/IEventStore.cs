using NetDevPack.Messaging;

namespace APGLogs.Domain.Core.Events
{
    public interface IEventStore
    {
        void Save<T>(T theEvent) where T : Event;
    }
}
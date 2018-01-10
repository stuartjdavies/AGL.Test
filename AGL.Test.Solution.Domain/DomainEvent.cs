using System;

namespace AGL.Test.Solution.Domain
{
    public enum EventLevel { Error, Info, Waring }    

    public class DomainEvent
    {
        public DomainEvent(string eventType,
                           string source,
                           EventLevel level,
                           dynamic body)
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.Now;
            EventType = eventType;
            Source = source;
            Level = level;
            Body = body;
        }

        public string Id { get; set; }
        public DateTime Created { get; set; }                
        public string EventType { get; set; }
        public string Source { get; set; }
        public EventLevel Level { get; set; }
        public dynamic Body { get; set; }
    }
}

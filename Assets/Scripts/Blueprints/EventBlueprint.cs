namespace Blueprints
{
    using DataManager.Blueprint.BlueprintReader;

    [BlueprintReader("Event")]
    public class EventBlueprint : GenericBlueprintReaderByRow<string, EventRecord>
    {
        
    }

    public class EventRecord
    {
        public string    Id;
        public EventType EventType;
        public int       StartRound;
        public int       DelayEachRound;
    }

    public enum EventType
    {
        Quiz
    }
}
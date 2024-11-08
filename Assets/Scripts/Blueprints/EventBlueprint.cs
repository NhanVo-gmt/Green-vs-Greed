namespace Blueprints
{
    using DataManager.Blueprint.BlueprintReader;
    using UnityEngine.Serialization;

    [BlueprintReader("Event")]
    public class EventBlueprint : GenericBlueprintReaderByRow<string, EventRecord>
    {
        
    }

    public class EventRecord
    {
        public string                                   Id;
        public EventType                                EventType;
        public int                                      StartRound;
        public int                                      DelayRound;
        public int                                      LastRound;
        public BlueprintByRow<Resource, ResourceRecord> Effects;
    }

    public enum EventType
    {
        None,
        Quiz,
        Natural
    }
}
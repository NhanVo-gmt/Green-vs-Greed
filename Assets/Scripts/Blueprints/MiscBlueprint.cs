namespace Blueprints
{
    using DataManager.Blueprint.BlueprintReader;

    [BlueprintReader("Misc")]
    public class MiscBlueprint : GenericBlueprintReaderByRow<string, MiscRecord>
    {
        
    }

    [CsvHeaderKey("Id")]
    public class MiscRecord
    {
        public string Id;
        public string Icon;
    }
}
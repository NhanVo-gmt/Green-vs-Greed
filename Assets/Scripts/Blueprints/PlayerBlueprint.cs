namespace Blueprints
{
    using DataManager.Blueprint.BlueprintReader;
    using UnityEngine;

    [BlueprintReader("Player")]
    public class PlayerBlueprint : GenericBlueprintReaderByRow<int, PlayerRecord>
    {
    }

    [CsvHeaderKey("Id")]
    public class PlayerRecord
    {
        public int                                              Id;
        public PlayerType                                       PlayerType;
        public BlueprintByRow<CardResource, CardResourceRecord> Resources;
    }

}
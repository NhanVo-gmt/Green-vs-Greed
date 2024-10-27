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
        public int                                      Id;
        public PlayerType                               PlayerType;
        public BlueprintByRow<Resource, ResourceRecord> Resources;
        public BlueprintByRow<PlayerUpgrade>            PlayerUpgrades;
    }

    [CsvHeaderKey("Level")]
    public class PlayerUpgrade
    {
        public int                                      Level;
        public BlueprintByRow<Resource, PlayerUpgradeRequirement> Requirements;
    }

    [CsvHeaderKey("ResourceRequirement")]
    public class PlayerUpgradeRequirement
    {
        public Resource ResourceRequirement;
        public int      ResourceRequirementValue;
    }
}
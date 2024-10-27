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
        public BlueprintByRow<Resource, PlayerUpgrade>  PlayerUpgrades;
    }

    [CsvHeaderKey("ResourceUpgrade")]
    public class PlayerUpgrade
    {
        public Resource                                 ResourceUpgrade;
        public BlueprintByRow<PlayerUpgradeRequirement> Requirements;
    }
    
    public class PlayerUpgradeRequirement
    {
        public int Level;
        public int ResourceRequirementValue;
    }

}
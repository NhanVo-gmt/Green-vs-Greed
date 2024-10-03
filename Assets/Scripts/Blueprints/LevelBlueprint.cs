using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blueprints
{
    using DataManager.Blueprint.BlueprintReader;

    [BlueprintReader("Level")]
    public class LevelBlueprint : GenericBlueprintReaderByRow<string, LevelRecord>
    {
        
    }

    [CsvHeaderKey("Id")]
    public class LevelRecord
    {
        public string                      Id;
        public string                      Name;
        public bool                        IsUnlockedByDefault;
        public BlueprintByRow<StageRecord> StageRecords;
    }

    [CsvHeaderKey("StageIndex")]
    public class StageRecord
    {
        public int  StageIndex;
        public bool IsStageUnlockedByDefault;
    }
}

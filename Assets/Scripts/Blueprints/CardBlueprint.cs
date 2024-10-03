namespace Blueprints
{
    using System.Collections;
    using System.Collections.Generic;
    using DataManager.Blueprint.BlueprintReader;
    using UnityEngine;

    [BlueprintReader("Card")]
    public class CardBlueprint : GenericBlueprintReaderByRow<string, CardRecord>
    {
       
    }

    [CsvHeaderKey("Id")]
    public class CardRecord
    {
        public string     Id;
        public string     Name;
        public string     Description;
        public PlayerType PlayerType;
        public string     Image;
    }

    public enum PlayerType
    {
        Environment,
        Corporation
    }
}
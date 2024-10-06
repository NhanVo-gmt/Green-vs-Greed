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
        public string                             Id;
        public string                             Name;
        public string                             Description;
        public PlayerType                         PlayerType;
        public string                             Image;
        public BlueprintByRow<CardResourceRecord> Resources;
    }

    public enum PlayerType
    {
        Environment = 0,
        Corporation = 1,
    }

    [CsvHeaderKey("ResourceId")]
    public class CardResourceRecord
    {
        public CardResource ResourceId;
        public int          ResourceAmount;
    }

    public enum CardResource
    {
        Wood = 0,
        Water = 1
    }
}
namespace Blueprints
{
    using System.Collections;
    using System.Collections.Generic;
    using DataManager.Blueprint.BlueprintReader;
    using UnityEngine;
    
    public class CardBlueprint : GenericBlueprintReaderByRow<string, CardRecord>
    {
       
    }

    [CsvHeaderKey("Id")]
    public class CardRecord
    {
        public string                                   Id;
        public string                                   Name;
        public string                                   Description;
        public PlayerType                               PlayerType;
        public string                                   Image;
        public BlueprintByRow<Resource, ResourceRecord> Resources;
        public EffectType                               Effect;
        public bool                                     IsEndTurn;
        public bool                                     UseImmediately;

        [NestedBlueprint] public CardNeededEffect CardNeededEffect;
    }

    [CsvHeaderKey("CardNeededId")]
    public class CardNeededEffect
    {
        public string       CardNeededId;
        public CardDeckType CardDeckType;
        public Resource     RewardResourceId;
        public int          RewardResourceAmount;
    }

    public enum PlayerType
    {
        Environment = 0,
        Corporation = 1,
        Effect = 2,
    }

    [CsvHeaderKey("ResourceId")]
    public class ResourceRecord
    {
        public Resource ResourceId;
        public int      ResourceAmount;
    }

    public enum Resource
    {
        Wood = 0,
        Water = 1,
        Money = 2
    }

    public enum EffectType
    {
        None = 0,
        Draw = 1,
        Blind = 2,
        Permit = 3,
        DrawCardFromResource = 4,
    }
}
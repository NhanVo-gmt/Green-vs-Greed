namespace UserData.Controller
{
    using System.Collections.Generic;
    using Blueprints;
    using UnityEngine;
    using Zenject;

    public class CardManager : IInitializable
    {
        private readonly CardBlueprint CardBlueprint;

        private List<CardRecord> EnvironmentCards = new();
        private List<CardRecord> CorporationCards = new();

        public CardManager(CardBlueprint cardBlueprint)
        {
            this.CardBlueprint = cardBlueprint;
        }
        
        public void Initialize()
        {
            foreach (CardRecord record in CardBlueprint.Values)
            {
                if (record.PlayerType == PlayerType.Environment)
                {
                    EnvironmentCards.Add(record);
                }
                else
                {
                    CorporationCards.Add(record);
                }
            }
        }

        public CardRecord DrawRandomCard(PlayerType playerType)
        {
            if (playerType == PlayerType.Corporation)
            {
                return CorporationCards[Random.Range(0, CorporationCards.Count)];
            }
            
            return EnvironmentCards[Random.Range(0, EnvironmentCards.Count)];
        }
        
        
    }
}
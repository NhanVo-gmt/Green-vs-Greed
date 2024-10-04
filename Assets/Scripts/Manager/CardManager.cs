namespace UserData.Controller
{
    using System.Collections.Generic;
    using Blueprints;
    using DataManager.MasterData;
    using DataManager.UserData;
    using UnityEngine;
    using UserData.Model;
    using Zenject;

    public class CardManager : BaseDataManager<UserProfile>
    {
        private readonly CardBlueprint CardBlueprint;

        private List<CardRecord> EnvironmentCards = new();
        private List<CardRecord> CorporationCards = new();
        
        public CardManager(MasterDataManager masterDataManager, CardBlueprint cardBlueprint) : base(masterDataManager)
        {
            this.CardBlueprint = cardBlueprint;
        }

        protected override void OnDataLoaded()
        {
            base.OnDataLoaded();
            
            
            foreach (CardRecord record in CardBlueprint.Values)
            {
                Debug.LogError(record.PlayerType);
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
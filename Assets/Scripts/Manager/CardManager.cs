namespace UserData.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blueprints;
    using Cysharp.Threading.Tasks;
    using DataManager.MasterData;
    using DataManager.UserData;
    using GameFoundation.Scripts.AssetLibrary;
    using UnityEngine;
    using UserData.Model;
    using Zenject;
    using Random = UnityEngine.Random;

    public class CardManager : BaseDataManager<UserProfile>
    {
        private readonly CorporationCardBlueprint corporationCardBlueprint;
        private readonly EnvironmentCardBlueprint environmentCardBlueprint;
        private readonly EffectCardBlueprint      effectCardBlueprint;
        private readonly IGameAssets              GameAssets;

        private Dictionary<PlayerType, List<CardRecord>> Cards                     = new();
        private List<CardRecord>                         PlayerCards               = new();
        private Dictionary<Resource, List<CardRecord>>   playerResourceCards = new();

        private Resource currentResource = Resource.Wood;

        private float randomRateEffectCard = 100;

        public static Action OnCardDataLoaded;
        
        public CardManager(MasterDataManager masterDataManager, EffectCardBlueprint effectCardBlueprint, IGameAssets gameAssets, 
                           CorporationCardBlueprint corporationCardBlueprint, EnvironmentCardBlueprint environmentCardBlueprint) : base(masterDataManager)
        {
            this.corporationCardBlueprint = corporationCardBlueprint;
            this.environmentCardBlueprint = environmentCardBlueprint;
            this.effectCardBlueprint      = effectCardBlueprint;
            this.GameAssets               = gameAssets;
        }

        protected override void OnDataLoaded()
        {
            base.OnDataLoaded();

            LoadCard(PlayerType.Corporation, corporationCardBlueprint);
            LoadCard(PlayerType.Environment, environmentCardBlueprint);
            LoadCard(PlayerType.Effect, effectCardBlueprint);
            
            playerResourceCards = LoadResourceCard(environmentCardBlueprint);
            
            OnCardDataLoaded?.Invoke();
        }

        void LoadCard(PlayerType playerType, CardBlueprint cardBlueprint)
        {
            Cards.Add(playerType, new());
            foreach (CardRecord record in cardBlueprint.Values)
            {
                Cards[playerType].Add(record);
            }
            
            if (playerType != PlayerType.Effect) PlayerCards.AddRange(Cards[playerType]);
        }

        Dictionary<Resource, List<CardRecord>> LoadResourceCard(CardBlueprint cardBlueprint)
        {
            Dictionary<Resource, List<CardRecord>> resourceCardRecords = new();
            var                                    resourceEnums = Enum.GetValues(typeof(Resource)).Cast<Resource>();
            foreach (var resourceEnum in resourceEnums)
            {
                resourceCardRecords.Add(resourceEnum, new());
            }
            
            foreach (CardRecord record in cardBlueprint.Values)
            {
                foreach (var recordResource in record.Resources.Values)
                {
                    resourceCardRecords[recordResource.ResourceId].Add(record);
                }
            }

            return resourceCardRecords;
        }
        
        public List<CardRecord> GetCards(PlayerType playerType)
        {
            return Cards[playerType];
        }

        public CardRecord DrawRandomCard(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Environment:
                    return DrawRandomPlayerCard();
                case PlayerType.Corporation:
                    return DrawRandomCorporationCard();
                default:
                    return null;
            }
        }
        
        
        public CardRecord DrawRandomPlayerCard()
        {
            int rate = Random.Range(0, 100);
            if (rate <= randomRateEffectCard)
            {
                return Cards[PlayerType.Effect][Random.Range(0, Cards[PlayerType.Effect].Count)];
            }

            switch (currentResource)
            {
                case Resource.Wood:
                    currentResource = Resource.Water;
                    break;
                case Resource.Water:
                    currentResource = Resource.Wood;
                    break;
            }

            return playerResourceCards[currentResource][Random.Range(0, playerResourceCards[currentResource].Count)];
        }
        
        public CardRecord DrawRandomCorporationCard()
        {
            int rate = Random.Range(0, 100);
            if (rate <= 20)
            {
                return Cards[PlayerType.Effect][Random.Range(0, Cards[PlayerType.Effect].Count)];
            }
            
            return Cards[PlayerType.Corporation][Random.Range(0, Cards[PlayerType.Corporation].Count)];
        }
        
        
        public CardRecord DrawRandomQuizCard()
        {
            int rate = Random.Range(0, 100);
            
            PlayerType playerType = PlayerType.Corporation;
            if (rate <= 50)
            {
                playerType = PlayerType.Environment;
            }

            return Cards[playerType][Random.Range(0, Cards[playerType].Count)];
        }

        public List<CardRecord> GetPlayerCards()
        {
            return PlayerCards;
        }

        public async UniTask<Sprite> GetIcon(string id)
        {
            return await GameAssets.LoadAssetAsync<Sprite>(id);
        }
        
        public async UniTask<Sprite> GetCardImage(PlayerType type)
        {
            // Environment Card
            // Corporation Card
            return await GameAssets.LoadAssetAsync<Sprite>($"{type}Card");
        }
    }
}
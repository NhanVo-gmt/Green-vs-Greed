namespace UserData.Controller
{
    using System;
    using System.Collections.Generic;
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

        private Dictionary<PlayerType, List<CardRecord>> Cards = new();

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
            
            OnCardDataLoaded?.Invoke();
        }

        void LoadCard(PlayerType playerType, CardBlueprint cardBlueprint)
        {
            Cards.Add(playerType, new());
            foreach (CardRecord record in cardBlueprint.Values)
            {
                Cards[playerType].Add(record);
            }
        }
        
        public List<CardRecord> GetCards(PlayerType playerType)
        {
            return Cards[playerType];
        }

        public CardRecord DrawRandomCard(PlayerType playerType)
        {
            int rate = Random.Range(0, 100);
            if (rate <= 100)
            {
                return Cards[PlayerType.Effect][Random.Range(0, Cards[PlayerType.Effect].Count)];
            }

            return Cards[playerType][Random.Range(0, Cards[playerType].Count)];
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
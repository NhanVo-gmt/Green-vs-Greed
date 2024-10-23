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
        private readonly CardBlueprint CardBlueprint;
        private readonly IGameAssets GameAssets;

        private Dictionary<PlayerType, List<CardRecord>> Cards = new();

        public static Action OnCardDataLoaded;
        
        public CardManager(MasterDataManager masterDataManager, CardBlueprint cardBlueprint, IGameAssets gameAssets) : base(masterDataManager)
        {
            this.CardBlueprint = cardBlueprint;
            this.GameAssets    = gameAssets;
        }

        protected override void OnDataLoaded()
        {
            base.OnDataLoaded();
            
            foreach (CardRecord record in CardBlueprint.Values)
            {
                if (!Cards.ContainsKey(record.PlayerType))
                {
                    Cards[record.PlayerType] = new();
                }
                
                Cards[record.PlayerType].Add(record);
            }
            
            OnCardDataLoaded?.Invoke();
        }
        
        public List<CardRecord> GetCards(PlayerType playerType)
        {
            return Cards[playerType];
        }

        public CardRecord DrawRandomCard(PlayerType playerType)
        {
            int rate = Random.Range(0, 100);
            if (rate <= 20)
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
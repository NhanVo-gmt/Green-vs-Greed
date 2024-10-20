﻿namespace UserData.Controller
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

        private List<CardRecord> EnvironmentCards = new();
        private List<CardRecord> CorporationCards = new();

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
                if (record.PlayerType == PlayerType.Environment)
                {
                    EnvironmentCards.Add(record);
                }
                else
                {
                    CorporationCards.Add(record);
                }
            }
            
            OnCardDataLoaded?.Invoke();
        }
        
        public List<CardRecord> GetCards(PlayerType playerType)
        {
            if (playerType == PlayerType.Corporation)
            {
                return CorporationCards;
            }
            
            return EnvironmentCards;
        }

        public CardRecord DrawRandomCard(PlayerType playerType)
        {
            if (playerType == PlayerType.Corporation)
            {
                return CorporationCards[Random.Range(0, CorporationCards.Count)];
            }
            
            return EnvironmentCards[Random.Range(0, EnvironmentCards.Count)];
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
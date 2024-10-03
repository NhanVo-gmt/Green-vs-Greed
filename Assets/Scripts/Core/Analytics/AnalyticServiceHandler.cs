namespace Analytic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Core.AnalyticServices;
    using DataManager.MasterData;
    using DataManager.UserData;
    using UnityEngine;
    using Zenject;

    public class AnalyticServiceHandler : BaseDataManager<AnalyticData>, IInitializable, ITickable, IDisposable
    {
        #region Inject

        private readonly IAnalyticServices analyticServices;
        private readonly SignalBus         signalBus;

        #endregion
        
        
        public AnalyticServiceHandler(MasterDataManager masterDataManager, IAnalyticServices analyticServices, SignalBus signalBus) : base(masterDataManager)
        {
            this.analyticServices = analyticServices;
            this.signalBus        = signalBus;
        }

        protected override void OnDataLoaded()
        {
            base.OnDataLoaded();
            if (!this.Data.IsFirstSession) return;

            this.Data.IsFirstSession = true;
            this.analyticServices.Track(this.CreateCommonEvent<FirstGameStart>());
        }

        public void Initialize()
        {
            this.analyticServices.Start();
            SubscribeSignals();
        }

        public void SubscribeSignals()
        {
            
        }

        public T CreateCommonEvent<T>() where T : BaseEvent, new()
        {
            var baseEvent = new T();
            baseEvent.StepId         = this.Data.GetNextStepId();
            baseEvent.LevelCompleted = this.Data.LevelCompleted;
            baseEvent.SessionCount   = this.Data.SessionCount;
            baseEvent.UserPlayTime   = this.Data.UserPlayTime;
            baseEvent.CoinCollected  = this.Data.CoinCollected;
            baseEvent.AdCount        = this.Data.AdsWatchCount;

            return baseEvent;
        }
        
        public void Tick()
        {
            if (this.Data == null) return;

            this.Data.UserPlayTime += Time.deltaTime;
        }
        
        public void Dispose()
        {
            
        }
    }

}
namespace Analytic
{
    using System.Collections.Generic;
    using DataManager.LocalData;
    using DataManager.UserData;
    using UnityEngine;

    public class AnalyticData : ILocalData, IUserData
    {
        public int    CurrentStepId;
        public bool   IsFirstSession        = false;
        public bool   IsFirstLevelCompleted = false;
        public bool   IsFirstAdsLoaded      = false;
        public int    SessionCount;
        public double UserPlayTime;
        public int    LevelCompleted;
        public int    AdsWatchCount;
        public int    CoinCollected;
        public double HintCollected;

        public int GetNextStepId() { return CurrentStepId++; }
    }
    
}

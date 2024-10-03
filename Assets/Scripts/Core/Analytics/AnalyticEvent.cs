namespace Analytic
{
    using System.Collections;
    using System.Collections.Generic;
    using Core.AnalyticServices.Data;
    using UnityEngine;

    public abstract class BaseEvent : IEvent
    {
        public int    StepId;
        public int    LevelCompleted;
        public float  SessionCount;
        public double UserPlayTime;
        public int    CoinCollected;
        public int    AdCount;
    }

    public class FirstGameStart : BaseEvent
    {
        
    }

    public class LevelProgression : BaseEvent
    {
        public int    Progress;
        public string Status;
    }

    public class Ad : BaseEvent
    {
        public string AdPlacement;
        public string AdType;
        public int    AdPlacementCount;
        public int    AdTypeCount;
        
    }
}
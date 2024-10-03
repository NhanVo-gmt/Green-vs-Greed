namespace UserData.Model
{
    using System;
    using System.Collections.Generic;
    using Blueprints;
    using DataManager.LocalData;
    using DataManager.UserData;
    using Newtonsoft.Json;

    public  class UserProfile : IUserData, ILocalData
    {
        public string                       CurrentLevelId    { get; set; } = "";
        public int                          CurrentLevelIndex { get; set; } = 0;
        public Dictionary<string, LevelLog> levelLogs;
    }
    
    public class LevelLog
    {
        public string Id;
        public State  LevelState;

        [JsonIgnore] public LevelRecord LevelRecord;
        [JsonIgnore] public Action      OnCompleted;

        public void Finish()
        {
            ChangeState(State.Complete);
            OnCompleted?.Invoke();
        }

        public void ChangeState(State newState)
        {
            if (LevelState >= newState) return;

            LevelState = newState;
        }
    }


    public enum State
    {
        InActive,
        Active,
        Complete
    }
}
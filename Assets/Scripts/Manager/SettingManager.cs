namespace Setting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DataManager.MasterData;
    using DataManager.UserData;
    using GameFoundation.Scripts.Models;
    using GameFoundation.Scripts.Utilities;
    using UnityEngine;

    public class SettingManager : BaseDataManager<UserSetting>
    {
        public Action OnDataLoadedCompleted;

        public SettingManager(MasterDataManager masterDataManager) : base(masterDataManager)
        {

        }

        protected override void OnDataLoaded()
        {
            base.OnDataLoaded();
            OnDataLoadedCompleted?.Invoke();
        }

        public bool GetMusicState()
        {
            return Data.MusicEnable;
        }
        
        public void SetMusicState(bool isEnable)
        {
            this.Data.MusicEnable = isEnable;
        }
        
        public bool GetSoundState()
        {
            return Data.SoundEnable;
        }
        
        public void SetSoundState(bool isEnable)
        {
            this.Data.SoundEnable = isEnable;
        }
    }

}
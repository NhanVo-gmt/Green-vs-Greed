namespace Setting
{
    using System.Collections;
    using System.Collections.Generic;
    using DataManager.LocalData;
    using DataManager.UserData;
    using UnityEngine;

    public class UserSetting : IUserData, ILocalData
    {
        public bool MusicEnable  = true;
        public bool SoundEnable  = true;
        public bool HapticEnable = true;
    }

}
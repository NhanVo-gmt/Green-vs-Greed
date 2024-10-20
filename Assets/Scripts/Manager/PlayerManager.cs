namespace UserData.Controller
{
    using Blueprints;
    using DataManager.MasterData;
    using DataManager.UserData;
    using UnityEngine;
    using UserData.Model;

    public class PlayerManager : BaseDataManager<UserProfile>
    {
        
        #region Inject

        private readonly PlayerBlueprint playerBlueprint;

        #endregion
        
        public PlayerManager(MasterDataManager masterDataManager, PlayerBlueprint playerBlueprint) : base(masterDataManager)
        {
            this.playerBlueprint = playerBlueprint;
        }

        public PlayerRecord GetPlayerRecord(int id)
        {
            return playerBlueprint[id];
        }
    }
}
namespace UserData.Controller
{
    using System;
    using System.Collections.Generic;
    using DataManager.MasterData;
    using DataManager.UserData;
    using UserData.Model;
    using System.Linq;
    using Blueprints;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundationBridge;
    using UnityEngine;

    public class LevelManager : BaseDataManager<UserProfile>
    {
        #region Inject

        private readonly LevelBlueprint    levelBlueprint;
        private readonly ScreenManager     screenManager;
        private readonly GameSceneDirector gameSceneDirector;

        #endregion

        public bool IsGameOver { get; private set; } = false;
        
        public LevelManager(MasterDataManager masterDataManager, LevelBlueprint levelBlueprint, ScreenManager screenManager, 
                            GameSceneDirector gameSceneDirector) : base(masterDataManager)
        {
            this.levelBlueprint    = levelBlueprint;
            this.screenManager     = screenManager;
            this.gameSceneDirector = gameSceneDirector;
        }

        protected override void OnDataLoaded()
        {
            base.OnDataLoaded();

            if (String.IsNullOrWhiteSpace(this.Data.CurrentLevelId))
            {
                LoadDefaultLevel();
            }

            if (this.Data.levelLogs == null)
            {
                CreateLevelLogSave();
            }
            else
            {
                LoadLevelLogSave();
            }
        }

        void LoadDefaultLevel()
        {
            this.Data.CurrentLevelId = levelBlueprint.FirstOrDefault().Value.Id;
        }
        
        private void CreateLevelLogSave()
        {
            this.Data.levelLogs = new();
            foreach (var level in GetAllLevels())
            {
                this.Data.levelLogs[level.Id] = new()
                {
                    Id            = level.Id,
                    LevelRecord   = level,
                    LevelState    = State.Active,
                };
            }
        }

        private void LoadLevelLogSave()
        {
            foreach (var levelLog in this.Data.levelLogs.Values)
            {
                LevelRecord levelRecord = GetLevelRecord(levelLog.Id);
                levelLog.LevelRecord = levelRecord;
            }
        }

        public List<LevelRecord> GetAllLevels()
        {
            return levelBlueprint.Values.ToList();
        }

        public LevelRecord GetCurrentLevel()
        {
            return levelBlueprint[this.Data.CurrentLevelId];
        }

        public int GetCurrentLevelIndex()
        {
            return this.Data.CurrentLevelIndex;
        }

        public LevelLog GetCurrentLevelLog()
        {
            return this.Data.levelLogs[GetCurrentLevel().Id];
        }

        public LevelRecord GetLevelRecord(string Id)
        {
            return levelBlueprint[Id];
        }

        #region In Game
        
        public async UniTask SelectLevel(LevelRecord levelRecord, int index)
        {
            IsGameOver = false;
            
            GetCurrentLevelLog().OnCompleted -= ShowCompletedScreen;
                
            this.Data.CurrentLevelId         =  levelRecord.Id;
            this.Data.CurrentLevelIndex      =  index;
            GetCurrentLevelLog().OnCompleted += ShowCompletedScreen;
            
            await LoadSelectedLevelScene();
        }

        public void FinishLevel()
        {
            GetCurrentLevelLog().Finish();
        }

        public void ShowCompletedScreen()
        {
            IsGameOver = true;
            this.screenManager.OpenScreen<GameCompletePopupPresenter, LevelLog>(GetCurrentLevelLog());
        }

        public void ShowLoseScreen()
        {
            IsGameOver = true;
            this.screenManager.OpenScreen<GameLosePresenter>();
        }

        #endregion

        #region Scene

        public void ReloadLevel()
        {
            LoadSelectedLevelScene().Forget();
        }

        async UniTask LoadSelectedLevelScene()
        {
            await this.gameSceneDirector.LoadLevelScene(this.Data.CurrentLevelId, this.Data.CurrentLevelIndex);
        }

        #endregion
    }
    
}

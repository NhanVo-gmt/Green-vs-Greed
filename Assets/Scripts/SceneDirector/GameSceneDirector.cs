namespace GameFoundationBridge
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Zenject;

    public static class SceneName
    {
        public const string Loading          = "0.LoadingScene";
        public const string StartScene       = "1.StartScene";
        public const string LevelSelectScene = "2.LevelSelectScene";
        public const string GameScene        = "3.GameScene";
    }

    public class GameSceneDirector : SceneDirector
    {
        private LoadingScreenView loadingScreenView;
        public GameSceneDirector(SignalBus signalBus, IGameAssets gameAssets, LoadingScreenView loadingScreenView) :
            base(signalBus, gameAssets)
        {
            CurrentSceneName       = SceneName.Loading;
            this.loadingScreenView = loadingScreenView;
        }

        #region shortcut
        
        public UniTask LoadStartScene()
        {
            return this.LoadSingleSceneBySceneManagerAsync(SceneName.StartScene);
        }

        public async UniTask LoadLevelSelectScene()
        {
            await loadingScreenView.Show();
            
            await this.LoadSingleSceneBySceneManagerAsync(SceneName.LevelSelectScene);
        }
        
        public async UniTask LoadLevelScene(string id, int index)
        {
            await loadingScreenView.Show();
            
            string levelName = $"Level{id}Stage{index}";
            await this.LoadMultipleSceneBySceneManagerAsync(SceneName.GameScene, SceneName.GameScene, levelName);
        }

        #endregion
    }
}
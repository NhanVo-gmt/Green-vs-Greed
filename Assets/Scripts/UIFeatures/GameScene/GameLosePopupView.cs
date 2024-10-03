using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundationBridge;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class GameLosePopupView : BaseView
{
    public Button restartLevelButton;
    public Button goToMenuButton;
}

[PopupInfo(nameof(GameLosePopupView), true, false)]
public class GameLosePresenter : BasePopupPresenter<GameLosePopupView>
{
    private readonly GameSceneDirector gameSceneDirector;
    private readonly LevelManager levelManager;
    
    public GameLosePresenter(SignalBus signalBus, GameSceneDirector gameSceneDirector, LevelManager levelManager) : base(signalBus)
    {
        this.gameSceneDirector = gameSceneDirector;
        this.levelManager      = levelManager;
    }
    
    public override UniTask BindData()
    {
        this.View.restartLevelButton.onClick.AddListener(() =>
        {
            this.View.restartLevelButton.onClick.RemoveAllListeners();
            this.levelManager.ReloadLevel();
        });
        
        this.View.goToMenuButton.onClick.AddListener(() =>
        {
            this.View.goToMenuButton.onClick.RemoveAllListeners();
            this.gameSceneDirector.LoadLevelSelectScene().Forget();
        });
        
        return UniTask.CompletedTask;
    }
}

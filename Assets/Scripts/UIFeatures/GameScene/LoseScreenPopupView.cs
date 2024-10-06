using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using GameFoundationBridge;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoseScreenPopupView : BaseView
{
    public Button restartBtn;
    public Button menuBtn;
}

[PopupInfo(nameof(LoseScreenPopupView), true, false)]
public class LoseScreenPopupPresenter : BasePopupPresenter<LoseScreenPopupView>
{
    private readonly GameSceneDirector gameSceneDirector;
    
    public LoseScreenPopupPresenter(SignalBus signalBus, GameSceneDirector gameSceneDirector) : base(signalBus)
    {
        this.gameSceneDirector = gameSceneDirector;
    }
    
    public override UniTask BindData()
    {
        this.View.restartBtn.onClick.AddListener(Restart);
        this.View.menuBtn.onClick.AddListener(GoToMenu);
        
        return UniTask.CompletedTask;
    }

    public override void Dispose()
    {
        base.Dispose();
        
        this.View.restartBtn.onClick.RemoveAllListeners();
        this.View.menuBtn.onClick.RemoveAllListeners();
    }

    public void Restart()
    {
        gameSceneDirector.LoadGameScene().Forget();
    }

    public void GoToMenu()
    {
        gameSceneDirector.LoadStartScene().Forget();
    }
}


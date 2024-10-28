using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using GameFoundation.Scripts.Utilities.LogService;
using GameFoundationBridge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameOverScreenPopupModel
{
    public bool isWin = true;

    public GameOverScreenPopupModel(bool isWin)
    {
        this.isWin = isWin;
    }
}

public class GameOverScreenPopupView : BaseView
{
    public TextMeshProUGUI text;
    public Button          restartBtn;
    public Button          menuBtn;
}

[PopupInfo(nameof(GameOverScreenPopupView), true, false)]
public class GameOverScreenPopupPresenter : BasePopupPresenter<GameOverScreenPopupView, GameOverScreenPopupModel>
{
    private readonly GameSceneDirector gameSceneDirector;
    
    public GameOverScreenPopupPresenter(SignalBus signalBus, ILogService logService, GameSceneDirector gameSceneDirector) : base(signalBus, logService)
    {
        this.gameSceneDirector = gameSceneDirector;
    }
    
    public override UniTask BindData(GameOverScreenPopupModel model)
    {
        if (model.isWin)
        {
            this.View.text.text = "You Win !!!";
        }
        else this.View.text.text = "You Lose !!!";
        
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


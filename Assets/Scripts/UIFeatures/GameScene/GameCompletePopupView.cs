using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blueprints;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.MVP;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundation.Scripts.Utilities.LogService;
using GameFoundation.Scripts.Utilities.ObjectPool;
using GameFoundationBridge;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using UserData.Model;
using Zenject;

public class GameCompletePopupView : BaseView
{
    public Button claimBtn;
}

[PopupInfo(nameof(GameCompletePopupView), false, false)]
public class GameCompletePopupPresenter : BasePopupPresenter<GameCompletePopupView, LevelLog>
{
    #region Inject

    private readonly GameSceneDirector gameSceneDirector;

    #endregion

    private LevelLog model;
    
    public GameCompletePopupPresenter(SignalBus signalBus, ILogService logService, GameSceneDirector gameSceneDirector) : base(signalBus, logService)
    {
        this.gameSceneDirector = gameSceneDirector;
    }
    
    public override async UniTask BindData(LevelLog model)
    {
        this.model = model;
        
        this.View.claimBtn.onClick.AddListener(() =>
        {
            this.View.claimBtn.onClick.RemoveAllListeners();

            gameSceneDirector.LoadLevelSelectScene().Forget();
            this.CloseView();
        });
    }

    protected override void OnViewReady()
    {
        base.OnViewReady();
        MasterAudio.Instance.PlayWinSound();
    }
    
}

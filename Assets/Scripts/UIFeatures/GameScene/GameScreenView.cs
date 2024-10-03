using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using GameFoundationBridge;
using TMPro;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class GameScreenView : BaseView
{
    public Button questionButton;
    public Button settingButton;
}

[ScreenInfo(nameof(GameScreenView))]
public class GameScreenPresenter : BaseScreenPresenter<GameScreenView>
{
    private readonly GameSceneDirector gameSceneDirector;
    private readonly IScreenManager    screenManager;
    private readonly LevelManager      levelManager;
    
    
    public GameScreenPresenter(SignalBus signalBus, GameSceneDirector gameSceneDirector, IScreenManager screenManager, LevelManager levelManager) : base(signalBus)
    {
        this.gameSceneDirector = gameSceneDirector;
        this.screenManager     = screenManager;
        this.levelManager      = levelManager;
    }

    protected override void OnViewReady()
    {
        base.OnViewReady();
        this.OpenViewAsync().Forget();
    }

    public override UniTask BindData()
    {
        this.View.questionButton.onClick.AddListener(OpenQuestionScreen);
        this.View.settingButton.onClick.AddListener(OpenSettingScreen);
        return UniTask.CompletedTask;
    }

    public override void Dispose()
    {
        this.View.questionButton.onClick.RemoveAllListeners();
        this.View.settingButton.onClick.RemoveAllListeners();
    }

    void OpenQuestionScreen()
    {
        // todo
    }

    void OpenSettingScreen()
    {
        this.screenManager.OpenScreen<GameSettingPopupPresenter>();
    }
}

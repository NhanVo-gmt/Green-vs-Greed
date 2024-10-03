using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundationBridge;
using UnityEngine;
using UnityEngine.UI;
using UserData.Model;
using Zenject;

public class StartGameScreenView : BaseView
{
    [Header("Header")]
    public Button startButton;
    public Button quitButton;
}

[ScreenInfo(nameof(StartGameScreenView))]
public class StartGameScreenPresenter : BaseScreenPresenter<StartGameScreenView>
{
    #region Inject
    
    private readonly GameSceneDirector gameSceneDirector;

    #endregion

    public StartGameScreenPresenter(SignalBus signalBus, GameSceneDirector gameSceneDirector) : base(signalBus)
    {
        this.gameSceneDirector = gameSceneDirector;
    }

    public override UniTask BindData()
    {
        this.View.startButton.onClick.AddListener(() =>
        {
            this.View.startButton.onClick.RemoveAllListeners();
            GoToLevelScreen();
        });
        
        this.View.quitButton.onClick.AddListener(() =>
        {
            this.View.quitButton.onClick.RemoveAllListeners();
            Application.Quit();
        });
        
        return UniTask.CompletedTask;
    }
    protected override void OnViewReady()
    {
        base.OnViewReady();
        this.OpenViewAsync().Forget();
    }

    void GoToLevelScreen()
    {
        this.gameSceneDirector.LoadLevelSelectScene().Forget();
    }

    public override void Dispose()
    {
        base.Dispose();

        this.View.startButton.onClick.RemoveAllListeners();
        this.View.quitButton.onClick.RemoveAllListeners();
    }
}

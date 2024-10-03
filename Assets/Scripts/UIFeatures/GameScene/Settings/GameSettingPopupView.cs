using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using Setting;
using UnityEngine.UI;
using Zenject;

public class GameSettingPopupView : BaseView
{
    public Button musicButton;
    public Button soundButton;
    public Button hapticButton;
    public Button supportButton;
    public Button closeButton;
}

[PopupInfo(nameof(GameSettingPopupView), true, false)]
public class GameSettingPopupPresenter : BasePopupPresenter<GameSettingPopupView>
{
    private readonly SettingManager settingManager;
    
    public GameSettingPopupPresenter(SignalBus signalBus, SettingManager settingManager) : base(signalBus)
    {
        this.settingManager = settingManager;
    }
    
    public override UniTask BindData()
    {
        this.View.soundButton.onClick.AddListener(MasterAudio.Instance.ToggleSound);
        this.View.musicButton.onClick.AddListener(MasterAudio.Instance.ToggleMusic);
        this.View.closeButton.onClick.AddListener(CloseView);
        return UniTask.CompletedTask;
    }

    public override void Dispose()
    {
        base.Dispose();
        
        this.View.soundButton.onClick.RemoveListener(MasterAudio.Instance.ToggleSound);
        this.View.musicButton.onClick.RemoveListener(MasterAudio.Instance.ToggleMusic);
        this.View.hapticButton.onClick.RemoveAllListeners();
        // this.View.supportButton.onClick.RemoveAllListeners();
        this.View.closeButton.onClick.RemoveListener(CloseView);
    }
}


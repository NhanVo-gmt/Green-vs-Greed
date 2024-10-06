using Blueprints;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using GameFoundation.Scripts.Utilities.LogService;
using TMPro;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class CardDetailsPopupView : BaseView
{
    public TextMeshProUGUI Title;
    public Image           Card;
    public Image           HeaderImg;
    public TextMeshProUGUI Description;
    public Button          backBtn;
}

[PopupInfo(nameof(CardDetailsPopupView), true, false)]
public class CardDetailsPopupPresenter : BasePopupPresenter<CardDetailsPopupView, CardRecord>
{
    private readonly CardManager    cardManager;
    private readonly IScreenManager screenManager;

    private CardRecord cardRecord;
    
    public CardDetailsPopupPresenter(SignalBus signalBus, ILogService logService, CardManager cardManager, IScreenManager screenManager) : base(signalBus, logService)
    {
        this.cardManager   = cardManager;
        this.screenManager = screenManager;
    }
    
    public override UniTask BindData(CardRecord record)
    {
        this.cardRecord = record;

        this.View.Title.text       = record.Name;
        this.View.Description.text = record.Description;
        cardManager.GetIcon($"{record.PlayerType}Card")
            .ContinueWith(sprite => this.View.Card.sprite = sprite).Forget();
        
        cardManager.GetIcon(record.Image)
            .ContinueWith(sprite => this.View.HeaderImg.sprite = sprite).Forget();
        
        this.View.backBtn.onClick.AddListener(RemoveScreen);
        
        return UniTask.CompletedTask;
    }

    public override void Dispose()
    {
        this.View.backBtn.onClick.RemoveAllListeners();
    }

    void RemoveScreen()
    {
        screenManager.CloseCurrentScreen();
    }
    
}
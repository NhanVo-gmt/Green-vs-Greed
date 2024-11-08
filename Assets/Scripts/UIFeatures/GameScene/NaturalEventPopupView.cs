using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blueprints;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.AssetLibrary;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundation.Scripts.Utilities.LogService;
using TMPro;
using UIFeatures.GameScene;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using Watermelon;
using Zenject;

public class NaturalEventModel
{
    public EventRecord  eventRecord;
    public EventManager eventManager;

    public NaturalEventModel(EventRecord eventRecord, EventManager eventManager)
    {
        this.eventRecord  = eventRecord;
        this.eventManager = eventManager;
    }
}

public class NaturalEventPopupView : BaseView
{
    public Image           img;
    public TextMeshProUGUI des;
}

[PopupInfo(nameof(NaturalEventPopupView), false, false)]
public class NaturalEventPopupPresenter : BasePopupPresenter<NaturalEventPopupView, NaturalEventModel>
{
    private readonly IGameAssets gameAssets;
    
    private NaturalEventModel model;
    
    public NaturalEventPopupPresenter(IGameAssets gameAssets, SignalBus signalBus, ILogService logService) : base(signalBus, logService)
    {
        this.gameAssets = gameAssets;
    }
    
    public override UniTask BindData(NaturalEventModel popupModel)
    {
        this.model = popupModel;
        
        UpdateImage().Forget();
        this.View.des.text = model.eventRecord.Description;

        Tween.DelayedCall(3f, Finish);
        
        return UniTask.CompletedTask;
    }

    async UniTask UpdateImage()
    {
        this.View.img.sprite = await gameAssets.LoadAssetAsync<Sprite>(model.eventRecord.Image);
    }

    public void Finish()
    {
        model.eventManager.EndEvent(false);
        CloseView();
    }
}

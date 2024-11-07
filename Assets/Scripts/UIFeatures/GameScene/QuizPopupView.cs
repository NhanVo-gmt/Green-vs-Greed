using System.Collections;
using System.Collections.Generic;
using Blueprints;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundation.Scripts.Utilities.LogService;
using TMPro;
using UIFeatures.GameScene;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class QuizModel
{
    public CardRecord   record;
    public string[]     answers;
    public EventManager eventManager;

    public QuizModel(CardRecord record, string[] answers, EventManager eventManager)
    {
        this.record       = record;
        this.answers      = answers;
        this.eventManager = eventManager;
    }
}

public class QuizPopupView : BaseView
{
    public Image    questionImg;
    public QuizButton[] answerBtns;
}

[PopupInfo(nameof(QuizPopupView), false, false)]
public class QuizPopupPresenter : BasePopupPresenter<QuizPopupView, QuizModel>
{
    private readonly CardManager cardManager;
    
    private QuizModel model;
    
    public QuizPopupPresenter(CardManager cardManager, SignalBus signalBus, ILogService logService) : base(signalBus, logService)
    {
        this.cardManager = cardManager;
    }
    
    public override UniTask BindData(QuizModel popupModel)
    {
        this.model = popupModel;
        
        UpdateUI();
        
        return UniTask.CompletedTask;
    }

    public override void Dispose()
    {
        base.Dispose();
        
        for (int i = 0; i < this.View.answerBtns.Length; i++)
        {
            this.View.answerBtns[i].OnClick -= ChooseAnswer;
            this.View.answerBtns[i].Dispose();
        }
    }

    void UpdateUI()
    {
        cardManager.GetIcon(model.record.Image).ContinueWith(img => this.View.questionImg.sprite = img).Forget();
        
        for (int i = 0; i < this.model.answers.Length; i++)
        {
            this.View.answerBtns[i].BindData(i, this.model.answers[i]);
            this.View.answerBtns[i].OnClick += ChooseAnswer;
        }
    }

    void ChooseAnswer(string answer)
    {
        if (answer == model.record.Name)
        {
            Debug.Log("Right");
            this.model.eventManager.EndEvent(true);
            
        }
        else
        {
            Debug.Log("Wrong");
            this.model.eventManager.EndEvent(false);
        }
        
        CloseView();
    }
}

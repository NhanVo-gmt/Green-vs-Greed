using System.Collections;
using System.Collections.Generic;
using Blueprints;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
using GameFoundation.Scripts.Utilities.LogService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserData.Controller;
using Zenject;

public class QuizModel
{
    public CardRecord record;
    public string[]   answers;

    public QuizModel(CardRecord record, string[] answers)
    {
        this.record  = record;
        this.answers = answers;
    }
}

public class QuizPopupView : BaseView
{
    public Image    questionImg;
    public Button[] answerBtns;
    public TextMeshProUGUI[]   answerTexts;
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
            this.View.answerBtns[i].onClick.RemoveAllListeners();
        }
    }

    void UpdateUI()
    {
        cardManager.GetIcon(model.record.Image).ContinueWith(img => this.View.questionImg.sprite = img).Forget();
        
        for (int i = 0; i < this.View.answerTexts.Length; i++)
        {
            this.View.answerTexts[i].SetText($"{i}. {model.answers[i]}");
            this.View.answerBtns[i].onClick.AddListener(() => ChooseAnswer(model.answers[i]));
        }
    }

    void ChooseAnswer(string answer)
    {
        if (answer == model.record.Name)
        {
            Debug.Log("Right");
        }
        else
        {
            Debug.Log("Wrong");
        }
        
        CloseView();
    }
}

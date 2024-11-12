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
using Watermelon;
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
    public Image        questionImg;
    public QuizButton[] answerBtns;
    public GameObject   winGO;
    public GameObject   loseGO;
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
        
        this.View.winGO.SetActive(false);
        this.View.loseGO.SetActive(false);
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
        UpdateUIAnswer();
        bool right = answer == model.record.Name;
        if (right)
        {
            this.View.winGO.SetActive(true);
            SoundManager.Instance.PlayOneShot(SoundType.RightAnswer);
            Debug.Log("Right");
        }
        else
        {
            this.View.loseGO.SetActive(true);
            SoundManager.Instance.PlayOneShot(SoundType.WrongAnswer);
            Debug.Log("Wrong");
        }
        
        Tween.DelayedCall(2f, () =>
        {
            this.model.eventManager.EndEvent(right);
            CloseView();
        });
    }

    void UpdateUIAnswer()
    {
        for (int i = 0; i < this.model.answers.Length; i++)
        {
            if (this.View.answerBtns[i].answer == model.record.Name)
            {
                this.View.answerBtns[i].UpdateAnswerColor(Color.green);
            }
            else
            {
                this.View.answerBtns[i].UpdateAnswerColor(Color.red);
            }
        }
    }
}

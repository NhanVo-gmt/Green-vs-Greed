using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blueprints;
using Cysharp.Threading.Tasks;
using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
using GameFoundation.Scripts.Utilities.Extension;
using UnityEngine;
using UserData.Controller;
using Zenject;
using EventType = Blueprints.EventType;
using Random = UnityEngine.Random;

public class EventData
{
    public EventRecord eventRecord { get; private set; }

    private bool isStarted   = false;
    private int  roundElapse = 0;

    public bool canStart { get; private set; } = false;

    public EventData(EventRecord eventRecord)
    {
        this.eventRecord = eventRecord;
        isStarted        = false;
        canStart         = false;
    }

    public void NewRound(int round)
    {
        if (round >= eventRecord.StartRound)
        {
            isStarted = true;
            Debug.Log($"[Event Manager]: Unlock Event {eventRecord.EventType.ToString()}");
        }

        if (isStarted)
        {
            roundElapse--;
            if (roundElapse <= 0)
            {
                canStart = true;
            }
        }
    }
    

    public void StartEvent()
    {
        canStart    = false;
        roundElapse = eventRecord.DelayRound;
    }
}

public class EventManager : MonoBehaviour
{
    [Inject] private EventBlueprint eventBlueprint;
    [Inject] private CardManager cardManager;
    [Inject] private IScreenManager screenManager;

    private List<EventData> GameEventDatas   = new();
    private List<EventType>     CurrentEventList = new();

    public bool startingEvent { get; private set; } = false;

    private void Awake()
    {
        this.GetCurrentContainer().Inject(this);
        
        CreateEvents();
    }

    public void OnNewRound(int newRound)
    {
        CurrentEventList.Clear();
        
        foreach (var gameEventData in GameEventDatas)
        {
            gameEventData.NewRound(newRound);

            if (gameEventData.canStart)
            {
                CurrentEventList.Add(gameEventData.eventRecord.EventType);
            }
        }
        
        StartRandomEvent();
    }

    void CreateEvents()
    {
        foreach (var eventRecord in eventBlueprint.Values)
        {
            EventData gameEventData = new(eventRecord);
            GameEventDatas.Add(gameEventData);
        }
    }

    public void StartRandomEvent()
    {
        if (CurrentEventList.Count <= 0) return;

        startingEvent = true;
        StartEvent(CurrentEventList[Random.Range(0, CurrentEventList.Count)]);
    }

    public void StartEvent(EventType eventType)
    {
        switch (eventType)
        {
            case EventType.Quiz:
                StartQuizEvent();
                break;
        }
    }

    public void StartQuizEvent()
    {
        CardRecord   record = cardManager.DrawRandomPlayerCard();
        List<string> answerList = new();
        answerList.Add(record.Name);
        answerList.Add(record.Name);
        answerList.Add(record.Name);
        answerList.Add(record.Name);

        QuizModel model = new(record, answerList.ShuffleSource().ToArray(), this);
        
        screenManager.OpenScreen<QuizPopupPresenter, QuizModel>(model);
    }

    public void EndEvent()
    {
        startingEvent = false;
    }
}

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
    public enum EventPhase
    {
        Lock,
        Wait,
        CanStart,
        Playing,
    }
    
    public EventRecord eventRecord { get; private set; }
    public EventPhase  phase;
    
    private int  roundElapse = 0;

    public Action<EventData> OnFinished;

    public EventData(EventRecord eventRecord)
    {
        this.eventRecord = eventRecord;
        phase            = EventPhase.Lock;
    }

    public void NewRound(int round)
    {
        switch (phase)
        {
            case EventPhase.Lock:
                if (round >= eventRecord.StartRound)
                {
                    EnableEvent();
                }
                break;
            case EventPhase.Wait:
                if (roundElapse <= 0)
                {
                    roundElapse = eventRecord.DelayRound;
                    ChangePhase(EventPhase.CanStart);
                }
                break;
            case EventPhase.CanStart:
                if (roundElapse != eventRecord.DelayRound)
                {
                    ChangePhase(EventPhase.Wait);
                }
                break;
            case EventPhase.Playing:
                if (roundElapse <= 0)
                {
                    FinishEvent();
                }
                break;
        }
        roundElapse--;
    }

    public void EnableEvent()
    {
        ChangePhase(EventPhase.CanStart);
        Debug.Log($"[Event Manager]: Unlock Event {eventRecord.Id}");
    }
    
    public void StartEvent()
    {
        ChangePhase(EventPhase.Playing);
        roundElapse = eventRecord.LastRound;
        Debug.Log($"[Event Manager]: Play Event {eventRecord.Id}");
    }
    
    public void FinishEvent()
    {
        ChangePhase(EventPhase.Wait);
        roundElapse = eventRecord.DelayRound;
        
        OnFinished?.Invoke(this);
        Debug.Log($"[Event Manager]: Finish Event {eventRecord.Id}");
    }

    public void ChangePhase(EventPhase newPhase)
    {
        phase = newPhase;
    }
}

public class EventManager : MonoBehaviour
{
    [Inject] private EventBlueprint eventBlueprint;
    [Inject] private CardManager cardManager;
    [Inject] private IScreenManager screenManager;

    private List<EventData> GameEventDatas = new();
    public  List<EventData> CurrentEvents { get; private set; } = new();

    public bool      eventPlaying { get; private set; } = false;
    public EventType CurrentEvent = EventType.Quiz;

    public Action<EventType> OnRewardEvent;

    private void Awake()
    {
        this.GetCurrentContainer().Inject(this);
        
        CreateEvents();
    }

    public void OnNewRound(int newRound)
    {
        List<EventData> roundEvents = new();
        
        foreach (var gameEventData in GameEventDatas)
        {
            gameEventData.NewRound(newRound);

            if (gameEventData.phase == EventData.EventPhase.CanStart)
            {
                roundEvents.Add(gameEventData);
            }
        }
        
        StartRandomEvent(roundEvents);
    }

    void CreateEvents()
    {
        foreach (var eventRecord in eventBlueprint.Values)
        {
            EventData gameEventData = new(eventRecord);
            GameEventDatas.Add(gameEventData);
        }
    }

    public void StartRandomEvent(List<EventData> roundEvents)
    {
        if (roundEvents.Count <= 0) return;

        eventPlaying = true;
        
        EventData eventData = roundEvents[Random.Range(0, roundEvents.Count)];
        eventData.OnFinished += FinishEvent;
        CurrentEvents.Add(eventData);
        
        StartEvent(eventData);
    }

    void FinishEvent(EventData eventData)
    {
        eventData.OnFinished -= FinishEvent;
        CurrentEvents.Remove(eventData);
    }

    public void StartEvent(EventData data)
    {
        data.StartEvent();
        switch (data.eventRecord.EventType)
        {
            case EventType.Quiz:
                StartQuizEvent();
                break;
            case EventType.Natural:
                screenManager.OpenScreen<NaturalEventPopupPresenter, NaturalEventModel>(new(data.eventRecord, this));
                break;
        }
    }

    public void StartQuizEvent()
    {
        CardRecord       record      = cardManager.DrawRandomPlayerCard();
        List<CardRecord> playerCards = new(cardManager.GetPlayerCards());
        playerCards.ShuffleSource();
        
        List<string>     answerList  = new();
        answerList.Add(record.Name);

        int index = -1;
        while (answerList.Count < 4)
        {
            index++;
            if (playerCards[index].Name == record.Name) continue;
            
            answerList.Add(playerCards[index].Name);
        }

        QuizModel model = new(record, answerList.ShuffleSource().ToArray(), this);
        
        screenManager.OpenScreen<QuizPopupPresenter, QuizModel>(model);
    }

    public void EndEvent(bool isWin)
    {
        eventPlaying = false;

        if (!isWin) return;
        
        OnRewardEvent?.Invoke(CurrentEvent);
    }
}

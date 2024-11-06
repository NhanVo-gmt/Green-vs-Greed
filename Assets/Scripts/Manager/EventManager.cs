using System;
using System.Collections;
using System.Collections.Generic;
using Blueprints;
using GameFoundation.Scripts.Utilities.Extension;
using UnityEngine;
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
            if (roundElapse == 0)
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

    private List<EventData> GameEventDatas   = new();
    private List<EventType>     CurrentEventList = new();

    private void Awake()
    {
        this.GetCurrentContainer().Inject(this);
        
        CreateEvents();
    }

    private void Start()
    {
        GameManager.Instance.OnNewRound += OnNewRound;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnNewRound -= OnNewRound;
    }

    private void OnNewRound(int newRound)
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
        
        StartEvent(CurrentEventList[Random.Range(0, CurrentEventList.Count)]);
    }

    public void StartEvent(EventType eventType)
    {
        switch (eventType)
        {
            case EventType.Quiz:
                break;
        }
    }
}

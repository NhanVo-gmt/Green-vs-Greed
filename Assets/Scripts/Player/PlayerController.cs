using System.Collections.Generic;
using Blueprints;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public int             playerIndex = 0;
    public PlayerStateName currentState;

    [Header("Card Index")]
    public List<CardSlot> CardSlots = new();

    #region State

    private StateMachine stateMachine;

    public Player_IdleState playerIdleState { get; private set; }
    public Player_PickState playerPickState { get; private set; }

    #endregion
    

    private void Awake()
    {
        stateMachine = new();

        playerIdleState = new(stateMachine, this, PlayerStateName.Idle);
        playerPickState = new(stateMachine, this, PlayerStateName.Pick);
        
        stateMachine.Initialize(playerIdleState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public void StartTurn()
    {
        stateMachine.ChangeState(playerPickState);
    }

    public void DrawCard(CardRecord cardRecord)
    {
        foreach (CardSlot slot in CardSlots)
        {
            if (slot.CanGetCard())
            {
                slot.DrawCard(cardRecord);
                return;
            }
        }
    }
}

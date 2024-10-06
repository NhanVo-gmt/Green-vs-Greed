public class Player_DrawState : PlayerState
{
    public Player_DrawState(StateMachine stateMachine, PlayerController player, PlayerStateName stateName) : base(stateMachine, player, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        stateMachine.ChangeState(player.playerPickState);
    }
}
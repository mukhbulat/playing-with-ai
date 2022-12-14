namespace Units.Clients.Director.EnemyAI.States
{
    public class HideState : State
    {
        public HideState(EnemyStateMachine stateMachine, SimpleEnemyAI enemy) : base(stateMachine, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Enemy.StartHiding();
        }
    }
}
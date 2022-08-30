namespace Units.Clients.Director.EnemyAI.States
{
    public class PatrolState : State
    {
        public PatrolState(EnemyStateMachine stateMachine, SimpleEnemyAI enemy) : base(stateMachine, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (Enemy.CheckPlayerState == CheckPlayerState.InCloseRange)
            {
                StateMachine.ChangeState(Enemy.Chase);
            }

            if (IsCurrentActionEnded)
            {
                IsCurrentActionEnded = false;
                Enemy.MoveToNextWaypoint();
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}
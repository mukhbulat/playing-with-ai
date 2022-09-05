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
            
            IsCurrentActionEnded = false;
            Enemy.StartPatrolling();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (Enemy.CheckPlayerState == CheckPlayerState.InCloseRange)
            {
                StateMachine.ChangeState(Enemy.Chase);
            }
        }

        protected override void OnCurrentActionEnded()
        {
            base.OnCurrentActionEnded();
            IsCurrentActionEnded = false;
            Enemy.StartPatrolling();
        }
    }
}
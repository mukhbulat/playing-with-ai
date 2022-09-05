namespace Units.Clients.Director.EnemyAI.States
{
    public class SeekState : State
    {
        public SeekState(EnemyStateMachine stateMachine, SimpleEnemyAI enemy) : base(stateMachine, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Enemy.StartSeeking();
            IsCurrentActionEnded = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (Enemy.CheckPlayerState != CheckPlayerState.NotVisible)
            {
                StateMachine.ChangeState(Enemy.Chase);
            }
        }

        protected override void OnCurrentActionEnded()
        {
            base.OnCurrentActionEnded();
            if (IsCurrentActionEnded)
            {
                StateMachine.ChangeState(Enemy.Patrol);
                return;
            }
        }
    }
}
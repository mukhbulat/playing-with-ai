namespace Units.Clients.Director.EnemyAI.States
{
    public class AttackState : State
    {
        public AttackState(EnemyStateMachine stateMachine, SimpleEnemyAI enemy) : base(stateMachine, enemy)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (Enemy.CheckPlayerState != CheckPlayerState.InAttackRange)
            {
                StateMachine.ChangeState(Enemy.Chase);
            }
        }

        protected override void OnCurrentActionEnded()
        {
            base.OnCurrentActionEnded();
            Enemy.StartAttacking(); 
            IsCurrentActionEnded = false;
        }
    }
}
namespace Units.Clients.Director.EnemyAI
{
    public class State
    {
        public State(EnemyStateMachine stateMachine, SimpleEnemyAI enemy)
        {
            StateMachine = stateMachine;
            Enemy = enemy;
        }
        
        protected EnemyStateMachine StateMachine;
        protected SimpleEnemyAI Enemy;
        protected bool IsCurrentActionEnded;

        public virtual void Enter()
        {
            Enemy.CurrentActionEnded += OnCurrentActionEnded;
            IsCurrentActionEnded = true;
        }

        protected virtual void OnCurrentActionEnded()
        {
            IsCurrentActionEnded = true;
        }

        public virtual void LogicUpdate()
        {
            if (Enemy.LowHealth && StateMachine.CurrentState != Enemy.Hide)
            {
                StateMachine.ChangeState(Enemy.Hide);
            }
        }

        public virtual void Exit()
        {
            Enemy.CurrentActionEnded -= OnCurrentActionEnded;
        }
    }
}
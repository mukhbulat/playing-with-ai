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

        public virtual void Enter()
        {
            Enemy.CurrentActionEnded += OnCurrentActionEnded;
        }

        protected void OnCurrentActionEnded()
        {
            
        }

        public virtual void LogicUpdate()
        {
            
        }

        public virtual void Exit()
        {
            Enemy.CurrentActionEnded -= OnCurrentActionEnded;
        }
    }
}
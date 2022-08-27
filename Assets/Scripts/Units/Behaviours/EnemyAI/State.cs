namespace Units.Behaviours.EnemyAI
{
    public class State
    {
        public State(EnemyStateMachine stateMachine, SimpleEnemyAIBehaviour enemyBehaviour)
        {
            StateMachine = stateMachine;
            EnemyBehaviour = enemyBehaviour;
        }
        
        protected EnemyStateMachine StateMachine;
        // Change to interface afap
        protected SimpleEnemyAIBehaviour EnemyBehaviour;

        public void Enter()
        {
            
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
namespace Units.Clients.Director.EnemyAI.States
{
    public class ChaseState : State
    {
        public ChaseState(EnemyStateMachine stateMachine, SimpleEnemyAI enemy) : base(stateMachine, enemy)
        {
        }
    }
}
using UnityEngine;

namespace Units.Clients.Director.EnemyAI.States
{
    public class ChaseState : State
    {
        public ChaseState(EnemyStateMachine stateMachine, SimpleEnemyAI enemy) : base(stateMachine, enemy)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Enemy.StartChasing();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Enemy.CheckPlayerState == CheckPlayerState.NotVisible)
            {
                StateMachine.ChangeState(Enemy.Patrol);
                Debug.Log("chase -> patrol");
            }

            if (Enemy.CheckPlayerState == CheckPlayerState.InAttackRange)
            {
                StateMachine.ChangeState(Enemy.Attack);
            }
        }
    }
}
using UnityEngine;

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
                Debug.Log("Player is not in attack range, changing to chase");
                StateMachine.ChangeState(Enemy.Chase);
            }
            
            if (!IsCurrentActionEnded) return;
            Enemy.StartAttack(); 
            IsCurrentActionEnded = false;
        }
    }
}
using System;
using Grid.Controllers;
using Units.Behaviours.EnemyAI.States;
using UnityEngine;

namespace Units.Behaviours.EnemyAI
{
    public class SimpleEnemyAIBehaviour : MonoBehaviour
    {
        private Pathfinding _pathfinding;

        private EnemyStateMachine _stateMachine;
        private State _attack, _chase, _hide, _patrol;

        private void Start()
        {
            _pathfinding = FindObjectOfType<Grid.Behaviours.GeneralPathfindingGrid>().Pathfinding;

            _stateMachine = new EnemyStateMachine();
            _attack = new AttackState(_stateMachine, this);
            _chase = new ChaseState(_stateMachine, this);
            _hide = new HideState(_stateMachine, this);
            _patrol = new PatrolState(_stateMachine, this);
            
            _stateMachine.Initialize(_patrol);
        }

        private void Update()
        {
            _stateMachine.CurrentState.Update();
        }
    }
}
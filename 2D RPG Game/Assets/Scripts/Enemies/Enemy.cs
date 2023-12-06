using System;
using UnityEngine;

namespace Enemy
{
    public class Enemy : Entity
    {
        public EnemyStateMachine StateMachine { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            StateMachine = new EnemyStateMachine();
        }

        protected override void Update()
        {
            base.Update();
                
            StateMachine.CurrentState.Update();
        }
    }
}

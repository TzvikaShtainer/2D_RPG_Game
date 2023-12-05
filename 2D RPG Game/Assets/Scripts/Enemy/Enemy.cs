using System;
using UnityEngine;

namespace Enemy
{
    public class Enemy : Entity
    {
        public EnemyStateMachine StateMachine { get; private set; }

        #region States
        
        

        #endregion

        private void Awake()
        {
            StateMachine = new EnemyStateMachine();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            StateMachine.CurrentState.Update();
        }
    }
}

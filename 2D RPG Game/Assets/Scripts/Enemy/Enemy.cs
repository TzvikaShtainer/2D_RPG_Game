using System;
using UnityEngine;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public Rigidbody2D Rb { get; private set; }
        public Animator Anim{ get; private set; }
        
        public EnemyStateMachine StateMachine { get; private set; }

        private void Awake()
        {
            StateMachine = new EnemyStateMachine();
        }

        private void Start()
        {
            Anim = GetComponentInChildren<Animator>();
            Rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            StateMachine.CurrentState.Update();
        }
    }
}

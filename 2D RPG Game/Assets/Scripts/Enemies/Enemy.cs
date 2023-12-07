using System;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Enemy
{
    public class Enemy : Entity
    {
        [SerializeField] protected LayerMask whatIsPlayer;
        
        [Header("Move Info")]
        public float moveSpeed = 2f;
        public float idleTime = 1f;
        public float battleTime;

        [Header("Attack Info")] 
        public float attackDistance = 2f;
        public float attackCooldown;
        [HideInInspector] public float lastTimeAttacked;
        
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
            
            //Debug.Log(IsPlayerDetected().collider.gameObject.name);
        }

        public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
        
        public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position,
            UnityEngine.Vector2.right * FacingDir, 50, whatIsPlayer);

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * FacingDir, transform.position.y));
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemySkeleton : Enemy
    {

        #region States
        public EnemySkeletonIdleState SkeletonIdleState { get; private set; }
        public EnemySkeletonMoveState SkeletonMoveState { get; private set; }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            SkeletonIdleState = new EnemySkeletonIdleState(this, StateMachine, "Idle", this);
            SkeletonMoveState = new EnemySkeletonMoveState(this, StateMachine, "Move", this);
        }

        protected override void Start()
        {
            base.Start();
            
            StateMachine.Initialize(SkeletonIdleState);
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemySkeleton : Enemy
    {

        #region States
        public EnemySkeletonIdleState SkeletonIdleState { get; private set; }
        public EnemySkeletonMoveState SkeletonMoveState { get; private set; }
        public EnemySkeletonBattleState SkeletonBattleState { get; private set; }
        public EnemySkeletonAttackState SkeletonAttackState { get; private set; }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            SkeletonIdleState = new EnemySkeletonIdleState(this, StateMachine, "Idle", this);
            SkeletonMoveState = new EnemySkeletonMoveState(this, StateMachine, "Move", this);
            SkeletonBattleState = new EnemySkeletonBattleState(this, StateMachine, "Move", this); //cuz he moves to the player until attack
            SkeletonAttackState = new EnemySkeletonAttackState(this, StateMachine, "Attack", this);
        }

        protected override void Start()
        {
            base.Start();
            
            StateMachine.Initialize(SkeletonIdleState);
        }
    }
}

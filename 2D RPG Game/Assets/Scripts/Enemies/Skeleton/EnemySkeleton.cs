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
        public EnemySkeletonStunnedState SkeletonStunnedState { get; private set; }
        public EnemySkeletonDeadState SkeletonDeadState { get; private set; }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            SkeletonIdleState = new EnemySkeletonIdleState(this, StateMachine, "Idle", this);
            SkeletonMoveState = new EnemySkeletonMoveState(this, StateMachine, "Move", this);
            SkeletonBattleState = new EnemySkeletonBattleState(this, StateMachine, "Move", this); //cuz he moves to the player until attack
            SkeletonAttackState = new EnemySkeletonAttackState(this, StateMachine, "Attack", this);
            SkeletonStunnedState = new EnemySkeletonStunnedState(this, StateMachine, "Stunned", this);
            SkeletonDeadState = new EnemySkeletonDeadState(this, StateMachine, "Idle", this);
        }

        protected override void Start()
        {
            base.Start();
            
            StateMachine.Initialize(SkeletonIdleState);
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.U))
            {
                StateMachine.ChangeState(SkeletonStunnedState);
            }
        }

        public bool CanBeStunned()
        {
            if (base.CanBeStunned())
            {
                StateMachine.ChangeState(SkeletonStunnedState);
                return true;
            }

            return false;
        }

        public override void Die()
        {
            base.Die();
            
            StateMachine.ChangeState(SkeletonDeadState);
        }
    }
}

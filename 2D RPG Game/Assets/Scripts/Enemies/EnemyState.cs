using UnityEngine;

namespace Enemy
{
    public class EnemyState
    {
        protected EnemyStateMachine EnemyStateMachine;
        protected Enemy EnemyBase;
        protected Rigidbody2D Rb;

        protected bool triggerCalled;
        protected float stateTimer;
        private string animBoolName;

        public EnemyState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName)
        {
            this.EnemyBase = enemyBase;
            this.EnemyStateMachine = enemyStateMachine;
            this.animBoolName = animBoolName;
        }

        public virtual void Update()
        {
            stateTimer -= Time.deltaTime;
        }
        
        public virtual void Enter()
        {
            triggerCalled = false;
            Rb = EnemyBase.Rb;
            EnemyBase.Anim.SetBool(animBoolName, true);
        }

        public virtual void Exit()
        { 
            EnemyBase.Anim.SetBool(animBoolName, false);
            EnemyBase.AssignLastAnimName(animBoolName);
        }

        public virtual void AnimationFinishTrigger()
        {
            Debug.Log("AnimationFinishTrigger");
            triggerCalled = true;
        }
    }
}

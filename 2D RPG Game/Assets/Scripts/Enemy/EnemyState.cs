using UnityEngine;

namespace Enemy
{
    public class EnemyState
    {
        protected EnemyStateMachine EnemyStateMachine;
        protected Enemy Enemy;

        protected bool triggerCalled;
        protected float stateTimer;
        private string animBoolName;

        public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine, string animBoolName)
        {
            this.Enemy = enemy;
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
            Enemy.Anim.SetBool(animBoolName, true);
        }

        public virtual void Exit()
        { 
            Enemy.Anim.SetBool(animBoolName, false);
        }
    }
}

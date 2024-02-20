using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.C))
            StateMachine.ChangeState(Player.blackHoleState);
        
        if(Input.GetKeyDown(KeyCode.Mouse1) && HaNoSword() && Player.Skill.Sword.swordUnlocked)
            StateMachine.ChangeState(Player.AimSwordState);
        
        if(Input.GetKeyDown(KeyCode.X) && Player.Skill.Parry.parryUnlocked)
            StateMachine.ChangeState(Player.CounterAttackState);
            
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z))
            StateMachine.ChangeState(Player.PrimaryAttackState);
        
        if(!Player.IsGroundDetected())
            StateMachine.ChangeState(Player.AirState);
        
        if (Input.GetKeyDown(KeyCode.Space) && Player.IsGroundDetected())
            StateMachine.ChangeState(Player.JumpState);
    }

    private bool HaNoSword()
    {
        if (!Player.Sword)
            return true;

        Player.Sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}

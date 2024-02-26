using UnityEngine;

public class LandingState:State
{

    bool sprintAction;


    public LandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
	{
		base.Enter();

        character.animator.SetBool("_Land", true);
        
    }

    public override void HandleInput()
    {
        base.HandleInput();

        sprintAction = Input.GetKey(KeyCode.LeftShift) && moveDirection != Vector3.zero;

    }

    public override void LogicUpdate()
    {
        
        base.LogicUpdate();


       

        
    }

    public override void ChangeState()
    {
        base.ChangeState();


        if (sprintAction)
        {
            stateMachine.ChangeState(character.sprintingState);
        }

        if (!sprintAction)
        {
            stateMachine.ChangeState(character.standingState);
        }

    }


    public override void Exit()
    {
        base.Exit();
        character.animator.SetBool("_Land", false);
    }

}


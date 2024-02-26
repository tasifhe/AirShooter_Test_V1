using UnityEngine;

public class FallState : State
{

    bool sprintAction;
    bool grounded;

    float _gravityValue;

    public FallState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
	{
		base.Enter();
        grounded = character.controller.isGrounded;
        character.animator.SetBool("_Fall", true);

        _gravityValue = character.gravityValue;
        //_gravityValue = -5;
    }

    public override void HandleInput()
    {
        base.HandleInput();

       

    }

    public override void LogicUpdate()
    {
        
        base.LogicUpdate();

        grounded = character.controller.isGrounded;
        Yvelocity.y += _gravityValue * Time.deltaTime;

        if (grounded && Yvelocity.y < 0)
        {
            Yvelocity.y = -2f;
        }

        character.controller.Move(Yvelocity * Time.deltaTime);

    }

    public override void ChangeState()
    {
        base.ChangeState();

        if (grounded)
        {
            stateMachine.ChangeState(character.landingState);
        }
        

    }


    public override void Exit()
    {
        base.Exit();
        character.animator.SetBool("_Fall", false);
    }

}


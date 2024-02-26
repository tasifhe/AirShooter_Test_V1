using UnityEngine;

public class JumpingState:State
{
    bool grounded;

    float gravityValue;
    float jumpHeight;
    float playerSpeed;
    float jumpRotationSpeed;

    Vector3 airVelocity;

    public JumpingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}
   
    public override void Enter()
	{
        base.Enter();

        grounded = false;
        gravityValue = character.gravityValue;
        jumpHeight = character.jumpHeight;
        playerSpeed = character.playerSpeed;
        Yvelocity.y = 0;
        jumpRotationSpeed = 10f;

        character.animator.SetFloat("_Speed", 0);
        character.animator.SetBool("_Jump", true);
        Jump();
    }
	public override void HandleInput()
	{
		base.HandleInput();

        input = moveAction.ReadValue<Vector2>();
       
    }

	public override void LogicUpdate()
    {
        base.LogicUpdate();
        

        if (!grounded)
        {           
            airVelocity = new Vector3(input.x, 0, input.y).normalized;            
            airVelocity = airVelocity.x * character.cameraTransform.right + airVelocity.z * character.cameraTransform.forward;
            airVelocity.y = 0f;

            if(airVelocity.magnitude >= 0.1f)
            {                
                character.transform.forward = Vector3.Slerp(character.transform.forward, airVelocity, jumpRotationSpeed * Time.deltaTime);
            }    

            character.controller.Move(Yvelocity * Time.deltaTime + (airVelocity * character.airControl + Yvelocity * (1 - character.airControl)) * playerSpeed * Time.deltaTime);
        }
        Yvelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
    }



    public override void ChangeState()
    {
        base.ChangeState();

        if (grounded)
        {
            stateMachine.ChangeState(character.landingState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();		
    }

    void Jump()
    {
        Yvelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
       
    }

    void AirMovement()
    {
      
    }

    public override void Exit()
    {
        base.Exit();

        character.animator.SetBool("_Jump", false);
    }

}


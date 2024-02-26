using UnityEngine;

public class StandingState: State
{
    
    
    float playerSpeed;
    float rotationSpeed;
    float _gravityValue;


    float turnSmoothTime;
    Vector3 currentVelocity;
    Vector3 cVelocity;


    bool grounded;
    bool sprintAction;
    bool jumpAction;
    bool aimAction;
    bool onWater;

    public StandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
    {
        base.Enter();
      
        input = Vector2.zero; //Raw movement input
        moveDirection = Vector3.zero; //Movement Vector3 "Camera Relative"
        currentVelocity = Vector3.zero; // Modified moveDirection;
        //Yvelocity.y = 0;    
       

        playerSpeed = character.playerSpeed;
        rotationSpeed = character.rotationSpeed;
        grounded = character.controller.isGrounded;
        _gravityValue = character.gravityValue;    
    }

    public override void HandleInput()
    {
        base.HandleInput();


        input = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y).normalized;

        sprintAction = Input.GetKey(KeyCode.LeftShift) && moveDirection != Vector3.zero;
        jumpAction = Input.GetKeyDown(KeyCode.Space);
        aimAction = Input.GetMouseButton(1);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        grounded = character.controller.isGrounded;
        onWater = character.characterAdditional_Swim.GetSwimState();
        Yvelocity.y += _gravityValue * Time.deltaTime;

        if (grounded && Yvelocity.y < 0)
        {
            Yvelocity.y = -5f;
        }
       
        if(moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + character.cameraTransform.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(character.transform.eulerAngles.y, targetAngle, ref turnSmoothTime, rotationSpeed);
            character.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            currentVelocity = Vector3.SmoothDamp(currentVelocity, movementDirection, ref cVelocity, character.velocityDampTime);
            character.controller.Move(currentVelocity.normalized * playerSpeed * Time.deltaTime);
        }

        character.controller.Move(Yvelocity * Time.deltaTime);
        
    }


    public override void ChangeState()
    {
        base.ChangeState();

        if (sprintAction)
        {
            stateMachine.ChangeState(character.sprintingState);
        }

        if (jumpAction)
        {
            stateMachine.ChangeState(character.jumpingState);
        }
       
        if (!grounded)
        {
            stateMachine.ChangeState(character.fallState);
        }

        if (aimAction)
        {
            stateMachine.ChangeState(character.strafeWalkState);
        }

        if (onWater)
        {
            stateMachine.ChangeState(character.swimState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();      
    }

    public override void UpdateAnimation()
    {
        base.UpdateAnimation();

        character.animator.SetFloat("_Speed", input.magnitude, character.speedDampTime, Time.deltaTime);

    }

    public override void Exit()
    {
        base.Exit();
        ExitTransition();
        
    }


    void ExitTransition()
    {
        //if (jumpAction) character.animator.SetFloat("_Speed", 0);
    }

}

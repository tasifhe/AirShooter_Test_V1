using UnityEngine;

public class StrafeWalkState : State
{
    
    
    float playerSpeed;
    float rotationSpeed;
    float _gravityValue;

    float horizontal;
    float vertical;

    float turnSmoothTime;
    Vector3 currentVelocity;
    Vector3 cVelocity;


    bool grounded;
    bool sprintAction;
    bool jumpAction;
    bool aimCanceledAction;
    bool aimState;

    public StrafeWalkState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
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
        aimState = true;

        playerSpeed = character.playerSpeed;
        rotationSpeed = 10f;
        grounded = character.controller.isGrounded;
        _gravityValue = character.gravityValue;    


    }

    public override void HandleInput()
    {
        base.HandleInput();


        input = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y).normalized;

        horizontal = moveDirection.x;
        vertical = moveDirection.z;


        sprintAction = Input.GetKey(KeyCode.LeftShift) && moveDirection != Vector3.zero;
        jumpAction = Input.GetKeyDown(KeyCode.Space);
        aimCanceledAction = Input.GetMouseButtonUp(1);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        grounded = character.controller.isGrounded;
        Yvelocity.y += _gravityValue * Time.deltaTime;

        if (grounded && Yvelocity.y < 0)
        {
            Yvelocity.y = -5f;
        }
       
        if(moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + character.cameraTransform.eulerAngles.y;     
            Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            currentVelocity = Vector3.SmoothDamp(currentVelocity, movementDirection, ref cVelocity, character.velocityDampTime);
            character.controller.Move(currentVelocity.normalized * (playerSpeed-1.5f) * Time.deltaTime);
        }

        character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.Euler(0f, character.cameraTransform.eulerAngles.y, 0f), rotationSpeed * Time.deltaTime);

        character.controller.Move(Yvelocity * Time.deltaTime);
        
    }


    public override void ChangeState()
    {
        base.ChangeState();

        if (aimCanceledAction)
        {
            stateMachine.ChangeState(character.standingState);
        }
       
        if (!grounded)
        {
            stateMachine.ChangeState(character.fallState);
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
        character.animator.SetFloat("_Horizontal", horizontal, character.speedDampTime, Time.deltaTime);
        character.animator.SetFloat("_Vertical", vertical, character.speedDampTime, Time.deltaTime);
        character.animator.SetBool("_AimState", aimState);

    }

    public override void Exit()
    {
        base.Exit();
        character.animator.SetBool("_AimState", false);

    }


   

}

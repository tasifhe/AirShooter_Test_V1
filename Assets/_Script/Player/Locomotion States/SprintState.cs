using UnityEngine;

public class SprintState : State
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

    public SprintState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
    {
        base.Enter();      
       

        input = Vector2.zero;
        moveDirection = Vector3.zero;
        currentVelocity = Vector3.zero;
        //Yvelocity.y = 0;

      

        playerSpeed = character.sprintSpeed;
        rotationSpeed = character.sprintRotationSpeed;
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

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        grounded = character.controller.isGrounded;
        Yvelocity.y += _gravityValue * Time.deltaTime;

        if (grounded && Yvelocity.y < 0)
        {
            Yvelocity.y = -10f;
        }
        
        

        if (moveDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + character.cameraTransform.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(character.transform.eulerAngles.y, targetAngle, ref turnSmoothTime, rotationSpeed);
            character.transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 targetDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            currentVelocity = Vector3.SmoothDamp(currentVelocity, targetDirection, ref cVelocity, character.velocityDampTime);
            character.controller.Move(currentVelocity.normalized * playerSpeed * Time.deltaTime);
        }

        character.controller.Move(Yvelocity * Time.deltaTime);

        
    }


    public override void ChangeState()
    {
        base.ChangeState();

        if (!sprintAction)
        {
            stateMachine.ChangeState(character.standingState);
        }

        if (jumpAction)
        {
            stateMachine.ChangeState(character.sprintjumpingState);
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

        character.animator.SetFloat("_Speed", input.magnitude + 0.5f, character.speedDampTime, Time.deltaTime);

    }

    public override void Exit()
    {
        base.Exit();      


    }

}

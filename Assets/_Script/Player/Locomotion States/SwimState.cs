using UnityEngine;

public class SwimState : State
{
    
    
    float playerSpeed;
    float rotationSpeed;
    float _gravityValue;


    float turnSmoothTime;
    Vector3 currentVelocity;
    Vector3 cVelocity;

    float horizontal;
    float vertical;
    bool grounded;
    bool onWater;
    float floatingPoint;
    float floatingoffset;
    float buoyancy;
    float floatingLerpSpeed;

    public SwimState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
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
       

        playerSpeed = character.playerSpeed;
        rotationSpeed = character.rotationSpeed;
        grounded = character.controller.isGrounded;
        _gravityValue = 0f;

        character.animator.SetBool("_SwimState", true);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        input = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(input.x, 0, input.y).normalized;

        horizontal = moveDirection.x;
        vertical = moveDirection.z;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        grounded = character.controller.isGrounded;
        //Yvelocity.y += _gravityValue * Time.deltaTime;

        onWater = character.characterAdditional_Swim.GetSwimState();
        floatingPoint = character.characterAdditional_Swim.GetWaterSurfaceInfo();
        floatingLerpSpeed = character.characterAdditional_Swim.floatingLerpSpeed;
        floatingoffset = character.characterAdditional_Swim.WaterBuoyancy;

        if (grounded && Yvelocity.y < 0)
        {
            //Yvelocity.y = -5f;
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

        if (!onWater)
        {
            character.controller.Move(Yvelocity * Time.deltaTime);
        }

        if (onWater)
        {
            // Lerp the character's y-position to the floating point using Mathf.Lerp
            float targetY = floatingPoint + floatingoffset ;
            float currentY = character.transform.position.y;
            float newY = Mathf.Lerp(currentY, targetY, floatingLerpSpeed * Time.deltaTime);
            character.transform.position = new Vector3(character.transform.position.x, newY, character.transform.position.z);
        }
    }


    public override void ChangeState()
    {
        base.ChangeState();


        if (!onWater)
        {
            stateMachine.ChangeState(character.standingState);
        }
       
        
       

    }

   
    public override void UpdateAnimation()
    {
        base.UpdateAnimation();

        //character.animator.SetFloat("_Speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        character.animator.SetFloat("_Horizontal", horizontal, character.speedDampTime, Time.deltaTime);
        character.animator.SetFloat("_Vertical", vertical, character.speedDampTime, Time.deltaTime);

    }

    public override void Exit()
    {
        base.Exit();
        character.animator.SetBool("_SwimState", false);
    }



}

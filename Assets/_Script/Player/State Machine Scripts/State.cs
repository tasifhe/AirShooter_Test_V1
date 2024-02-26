using UnityEngine;
using UnityEngine.InputSystem;

public class State
{
    public Character character;
    public StateMachine stateMachine;

  
    //protected Vector3 inputDirection; 
    protected Vector2 input; // Raw horizontal and vertical Movement Input From Keyboard
    protected Vector3 moveDirection; // Direct horizontal and vertical Movement direction from "input" Vector 
    protected Vector3 Yvelocity; // Direct Gravity,Jump vector controlls Y alone; 


    public InputAction moveAction;
   
  

    public State(Character _character, StateMachine _stateMachine)
	{
        character = _character;
        stateMachine = _stateMachine;

        moveAction = character.playerInput.actions["Move"];
       

    }

    public virtual void Enter()
    {
        character.playerStateText.text = this.ToString();
    }

    public virtual void HandleInput() { }    

    public virtual void LogicUpdate() { }   

    public virtual void ChangeState() { }   

    public virtual void PhysicsUpdate() { }   

    public virtual void UpdateAnimation() { }

    public virtual void Exit() { }
    
}


using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Character : MonoBehaviour
{
   
    [Header("Controls")]
    public float playerSpeed = 5.0f;
    public float sprintSpeed = 7.0f;
    public float crouchSpeed = 2.0f;
    [Space(10)]
    public float rotationSpeed = 5f;
    public float sprintRotationSpeed = 3;
    [Space(10)]
    public float jumpHeight = 0.8f; 
    public float gravityMultiplier = 2;
    public float crouchColliderHeight = 1.35f;

    [Header("Animation Smoothing")]
    [Range(0, 1)]
    public float speedDampTime = 0.1f;
    [Range(0, 1)]
    public float velocityDampTime = 0.9f;
    [Range(0, 1)]
    public float rotationDampTime = 0.2f;
    [Range(0, 1)]
    public float airControl = 0.5f;

    public StateMachine movementSM;
    public StandingState standingState;
    public SprintState sprintingState;
    public JumpingState jumpingState;
    //public CrouchingState crouching;
    public LandingState landingState;
    public SprintJumpingState sprintjumpingState;
    public FallState fallState;
    public StrafeWalkState strafeWalkState;
    public SwimState swimState;

    [HideInInspector]
    public float gravityValue = -9.81f;
    [HideInInspector]
    public float normalColliderHeight;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Transform cameraTransform;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector3 playerVelocity;

    [Space(20)]
    [SerializeField] public TextMeshProUGUI playerStateText;
    [SerializeField] public CharacterAdditional_Swim characterAdditional_Swim;

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        movementSM = new StateMachine();

        standingState = new StandingState(this, movementSM);
        sprintingState = new SprintState(this, movementSM);
        jumpingState = new JumpingState(this, movementSM);
        sprintjumpingState = new SprintJumpingState(this, movementSM);
        landingState = new LandingState(this, movementSM);
        fallState = new FallState(this, movementSM);
        strafeWalkState = new StrafeWalkState(this, movementSM);
        swimState = new SwimState(this, movementSM);

        //crouching = new CrouchingState(this, movementSM);
        //combatting = new CombatState(this, movementSM);
        //attacking = new AttackState(this, movementSM);

        movementSM.Initialize(standingState);

        normalColliderHeight = controller.height;
        gravityValue *= gravityMultiplier;
    }

    private void Update()
    {
        movementSM.currentState.HandleInput();
        movementSM.currentState.LogicUpdate();
        movementSM.currentState.UpdateAnimation();
        movementSM.currentState.ChangeState();
    }

    private void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }
   
}

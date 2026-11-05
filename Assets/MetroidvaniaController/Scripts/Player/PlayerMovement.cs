using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    [SerializeField] private InputActionReference moveAction; // Vector2
    [SerializeField] private InputActionReference jumpAction; // Button
    [SerializeField] private InputActionReference dashAction; // Button

    private InputActionMap defaultActions;
    private InputAction moveInput;
    private InputAction jumpInput;
    private InputAction dashInput;

    float horizontalMove = 0f;
    bool jump = false;
    bool dash = false;

    void Awake()
    {
        defaultActions = new InputActionMap("PlayerMovement");
        var defaultMove = defaultActions.AddAction("Move", InputActionType.Value,
            expectedControlLayout: "Vector2");
        defaultMove.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        defaultMove.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        defaultMove.AddBinding("<Gamepad>/leftStick");
        defaultMove.AddBinding("<Gamepad>/dpad");
        var defaultJump = defaultActions.AddAction("Jump", InputActionType.Button, "<Keyboard>/space");
        defaultJump.AddBinding("<Gamepad>/buttonSouth");
        var defaultDash = defaultActions.AddAction("Dash", InputActionType.Button, "<Keyboard>/leftShift");
        defaultDash.AddBinding("<Gamepad>/buttonEast");

        moveInput = (moveAction != null ? moveAction.action : null) ?? defaultMove;
        jumpInput = (jumpAction != null ? jumpAction.action : null) ?? defaultJump;
        dashInput = (dashAction != null ? dashAction.action : null) ?? defaultDash;
    }

    void OnEnable()
    {
        moveInput.Enable();
        jumpInput.Enable();
        dashInput.Enable();
    }

    void OnDisable()
    {
        moveInput.Disable();
        jumpInput.Disable();
        dashInput.Disable();
        horizontalMove = 0f;
        jump = false;
        dash = false;
    }

    void OnDestroy()
    {
        defaultActions.Dispose();
    }

    void Update()
    {
        horizontalMove = moveInput.ReadValue<Vector2>().x * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (jumpInput.WasPressedThisFrame()) jump = true;
        if (dashInput.WasPressedThisFrame()) dash = true;
    }

    public void OnFall()     => animator.SetBool("IsJumping", true);
    public void OnLanding()  => animator.SetBool("IsJumping", false);

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
        jump = false;
        dash = false;
    }
}

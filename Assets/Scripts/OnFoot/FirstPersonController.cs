using UnityEngine;
using UnityEngine.InputSystem;

/// First-person walking and mouse-look for the on-foot mode. Movement runs on
/// real time, not in-game time: the player doesn't walk faster when the sim
/// clock is accelerated.
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Child transform holding the camera; it pitches up/down while this object turns left/right")]
    public Transform cameraPivot;

    [Header("Movement")]
    [Tooltip("Walking speed in meters per second")]
    public float walkSpeedMetersPerSecond = 3.5f;

    [Tooltip("Speed while Sprint is held, in meters per second")]
    public float sprintSpeedMetersPerSecond = 6f;

    [Tooltip("Downward acceleration in meters per second squared")]
    public float gravityMetersPerSecondSquared = 20f;

    [Header("Look")]
    [Tooltip("Degrees of turn per pixel of mouse movement")]
    public float lookDegreesPerPixel = 0.1f;

    [Range(0f, 89f)]
    [Tooltip("How far up or down the player can look, in degrees")]
    public float maxPitchDegrees = 85f;

    /// While true the player can still look around but not walk (e.g. while holding a frame).
    public bool MovementLocked { get; set; }

    CharacterController body;
    InputAction moveAction;
    InputAction lookAction;
    InputAction sprintAction;
    float pitchDegrees;
    float verticalSpeed;

    void Awake()
    {
        body = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Player/Move", true);
        lookAction = InputSystem.actions.FindAction("Player/Look", true);
        sprintAction = InputSystem.actions.FindAction("Player/Sprint", true);
    }

    void OnEnable()
    {
        // Project-wide actions are normally enabled already; this is a no-op then.
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        SetCursorLocked(true);
    }

    void OnDisable()
    {
        SetCursorLocked(false);
    }

    void Update()
    {
        UpdateCursorLock();
        if (Cursor.lockState == CursorLockMode.Locked)
            Look();
        Move();
    }

    void UpdateCursorLock()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;

        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            SetCursorLocked(false);
        else if (mouse != null && mouse.leftButton.wasPressedThisFrame && Cursor.lockState != CursorLockMode.Locked)
            SetCursorLocked(true);
    }

    void Look()
    {
        Vector2 turn = lookAction.ReadValue<Vector2>() * lookDegreesPerPixel;
        transform.Rotate(0f, turn.x, 0f);
        pitchDegrees = Mathf.Clamp(pitchDegrees - turn.y, -maxPitchDegrees, maxPitchDegrees);
        cameraPivot.localRotation = Quaternion.Euler(pitchDegrees, 0f, 0f);
    }

    void Move()
    {
        Vector2 input = MovementLocked ? Vector2.zero : Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);
        float speed = sprintAction.IsPressed() ? sprintSpeedMetersPerSecond : walkSpeedMetersPerSecond;
        Vector3 horizontal = (transform.right * input.x + transform.forward * input.y) * speed;

        if (body.isGrounded && verticalSpeed < 0f)
            verticalSpeed = -kGroundStickSpeed;
        verticalSpeed -= gravityMetersPerSecondSquared * Time.deltaTime;

        body.Move((horizontal + Vector3.up * verticalSpeed) * Time.deltaTime);
    }

    static void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    // Small constant downward speed while grounded, so the controller stays
    // snapped to the ground when walking down slopes.
    const float kGroundStickSpeed = 2f;
}

using UnityEngine;
using UnityEngine.InputSystem;

/// Finds what the player is looking at (one ray from the camera centre per
/// frame) and forwards the Interact (E) and Secondary (F) keys to it.
public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    public Camera viewCamera;
    public PlayerHud hud;
    public PlayerInventory inventory;

    [Header("Reach")]
    [Tooltip("How far from the camera the player can use things, in meters")]
    public float reachMeters = 2.5f;

    [Tooltip("Layers the look ray can hit")]
    public LayerMask lookMask = ~0;

    public Interactable CurrentTarget { get; private set; }

    InputAction interactAction;
    InputAction secondaryAction;
    Collider lastHitCollider;
    string shownPrompt;
    string shownSecondaryPrompt;

    void Awake()
    {
        interactAction = InputSystem.actions.FindAction("Player/Interact", true);
        secondaryAction = InputSystem.actions.FindAction("Player/Secondary", true);
    }

    void OnEnable()
    {
        // Project-wide actions are normally enabled already; this is a no-op then.
        interactAction.Enable();
        secondaryAction.Enable();
    }

    void Update()
    {
        UpdateTarget();
        UpdatePrompt();

        if (!HasUsableTarget())
            return;

        // WasPressedThisFrame reads the key itself, ignoring the Hold
        // interaction the template put on Interact — a tap should be enough.
        if (interactAction.WasPressedThisFrame())
            CurrentTarget.Interact(this);
        else if (secondaryAction.WasPressedThisFrame() && CurrentTarget.SecondaryPrompt != null)
            CurrentTarget.SecondaryInteract(this);
    }

    void UpdateTarget()
    {
        Transform eye = viewCamera.transform;
        Collider hitCollider = null;
        if (Physics.Raycast(eye.position, eye.forward, out RaycastHit hit, reachMeters, lookMask, QueryTriggerInteraction.Collide))
            hitCollider = hit.collider;

        // The component lookup only runs when the ray moves onto a different
        // collider, not every frame.
        if (hitCollider == lastHitCollider)
            return;

        lastHitCollider = hitCollider;
        CurrentTarget = hitCollider != null ? hitCollider.GetComponentInParent<Interactable>() : null;
    }

    // Prompts are constant strings, so the comparison is cheap and the HUD
    // text is only rebuilt when what's on offer changes.
    void UpdatePrompt()
    {
        bool usable = HasUsableTarget();
        string prompt = usable ? CurrentTarget.Prompt : null;
        string secondaryPrompt = usable ? CurrentTarget.SecondaryPrompt : null;
        if (prompt == shownPrompt && secondaryPrompt == shownSecondaryPrompt)
            return;

        shownPrompt = prompt;
        shownSecondaryPrompt = secondaryPrompt;
        hud.SetPrompt(BuildPromptText(prompt, secondaryPrompt));
    }

    bool HasUsableTarget()
    {
        return CurrentTarget != null && CurrentTarget.isActiveAndEnabled && CurrentTarget.CanInteract;
    }

    static string BuildPromptText(string prompt, string secondaryPrompt)
    {
        if (secondaryPrompt == null)
            return prompt == null ? null : kInteractKeyHint + prompt;
        if (prompt == null)
            return kSecondaryKeyHint + secondaryPrompt;
        return kInteractKeyHint + prompt + kPromptGap + kSecondaryKeyHint + secondaryPrompt;
    }

    const string kInteractKeyHint = "[E]  ";
    const string kSecondaryKeyHint = "[F]  ";
    const string kPromptGap = "        ";
}

using UnityEngine;
using UnityEngine.InputSystem;

/// The smoker in the player's hand. The use button (the template's "Attack"
/// action — left mouse) puffs smoke; a puff that reaches a hive calms it.
public class Smoker : MonoBehaviour
{
    [Header("References")]
    public PlayerInteractor interactor;
    public ParticleSystem puffParticles;

    [Header("Puff")]
    [Tooltip("Smoke particles emitted per puff")]
    public int particlesPerPuff = 14;

    [Tooltip("Minimum real seconds between puffs")]
    public float puffCooldownSeconds = 0.4f;

    InputAction useAction;
    float nextPuffTime;

    void Awake()
    {
        useAction = InputSystem.actions.FindAction("Player/Attack", true);
    }

    void OnEnable()
    {
        // Project-wide actions are normally enabled already; this is a no-op then.
        useAction.Enable();
    }

    void Update()
    {
        // While the cursor is free the click belongs to the editor, not the smoker.
        if (Cursor.lockState != CursorLockMode.Locked)
            return;
        if (!useAction.WasPressedThisFrame() || Time.time < nextPuffTime)
            return;

        nextPuffTime = Time.time + puffCooldownSeconds;
        Puff();
    }

    void Puff()
    {
        puffParticles.Emit(particlesPerPuff);

        // The puff reaches as far as the player can interact. Aiming at a frame
        // in an open hive counts as smoking that hive.
        HiveInteraction hive = FindTargetHive(interactor.CurrentTarget);
        if (hive != null && hive.isActiveAndEnabled)
            hive.ReceiveSmoke(interactor);
    }

    static HiveInteraction FindTargetHive(Interactable target)
    {
        if (target is HiveInteraction hive)
            return hive;
        if (target is HiveFrame frame)
            return frame.hive;
        return null;
    }
}

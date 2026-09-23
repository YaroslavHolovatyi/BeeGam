using System.Collections;
using UnityEngine;

/// Lets the on-foot player work a hive: smoke calms it, Interact lifts the lid
/// off or puts it back. Once it's open, the HiveFrame components inside are
/// what the player inspects.
[RequireComponent(typeof(HiveController))]
public class HiveInteraction : Interactable
{
    [Header("References")]
    public GameClock clock;

    [Tooltip("The lid that lifts off when the hive is opened")]
    public Transform lid;

    [Header("Smoke")]
    [Tooltip("How long one puff keeps the colony calm, in in-game minutes. Placeholder")]
    public float calmMinutes = 30f;

    [Header("Lid")]
    [Tooltip("Where the lid rests while the hive is open, relative to the hive, in meters")]
    public Vector3 openLidLocalPosition = new Vector3(0.62f, 0.04f, 0f);

    [Tooltip("Real seconds the lid takes to come off or go back on")]
    public float lidMoveSeconds = 0.6f;

    public bool IsOpen { get; private set; }

    public bool IsCalm => clock.TotalGameHours < calmUntilGameHours;

    /// True while one of this hive's frames is out; set by HiveFrame.
    public bool IsFrameOut { get; set; }

    public HiveController Colony => hive;

    public override string Prompt => IsOpen ? kClosePrompt : kOpenPrompt;

    // The lid can't go back on with a frame out.
    public override bool CanInteract => !IsFrameOut;

    // Offered only while the hive is open and there's honey above the reserve.
    public override string SecondaryPrompt =>
        IsOpen && hive.HarvestableKg >= kMinHarvestKg ? kHarvestPrompt : null;

    public override void SecondaryInteract(PlayerInteractor player)
    {
        float harvestedKg = hive.Harvest();
        player.inventory.carriedHoneyKg += harvestedKg;
        player.hud.ShowMessage(
            $"You cut {harvestedKg:0.00} kg of capped honey from the frames, leaving the bees their reserve.\n" +
            $"Carrying {player.inventory.carriedHoneyKg:0.00} kg. Sell it at the honey stand.");
    }

    HiveController hive;
    Vector3 closedLidLocalPosition;
    float calmUntilGameHours = float.NegativeInfinity;
    Coroutine lidMove;

    void Awake()
    {
        hive = GetComponent<HiveController>();
        closedLidLocalPosition = lid.localPosition;
    }

    public override void Interact(PlayerInteractor player)
    {
        if (IsOpen)
            Close();
        else
            Open(player);
    }

    /// Called by the smoker when a puff reaches this hive.
    public void ReceiveSmoke(PlayerInteractor player)
    {
        calmUntilGameHours = clock.TotalGameHours + calmMinutes / 60f;
        player.hud.ShowMessage("You puff smoke at the hive. The bees settle down.");
    }

    void Open(PlayerInteractor player)
    {
        IsOpen = true;
        MoveLid(openLidLocalPosition);

        // What an unsmoked hive does to the player is still open
        // (QUESTIONS.md #10); for now it only changes the message.
        string breedName = hive.breed != null ? hive.breed.breedName : "Unknown breed";
        string mood = IsCalm
            ? "The bees are calm and busy on the frames."
            : "The bees boil up over the frames, agitated. Smoke would have calmed them.";
        player.hud.ShowMessage($"{breedName} colony. {mood}");
    }

    void Close()
    {
        IsOpen = false;
        MoveLid(closedLidLocalPosition);
    }

    // A coroutine rather than Update, so a hive costs nothing per frame while
    // its lid is still.
    void MoveLid(Vector3 targetLocalPosition)
    {
        if (lidMove != null)
            StopCoroutine(lidMove);
        lidMove = StartCoroutine(MoveLidRoutine(targetLocalPosition));
    }

    // Goes via a point above the resting spot beside the hive, so the lid
    // never passes through the hive body on the way.
    IEnumerator MoveLidRoutine(Vector3 targetLocalPosition)
    {
        Vector3 waypoint = new Vector3(openLidLocalPosition.x, closedLidLocalPosition.y + kLidLiftMeters, openLidLocalPosition.z);
        float legSeconds = Mathf.Max(kMinLegSeconds, lidMoveSeconds * 0.5f);

        yield return MoveLidBetween(lid.localPosition, waypoint, legSeconds);
        yield return MoveLidBetween(waypoint, targetLocalPosition, legSeconds);
        lidMove = null;
    }

    IEnumerator MoveLidBetween(Vector3 from, Vector3 to, float seconds)
    {
        for (float t = 0f; t < 1f; t += Time.deltaTime / seconds)
        {
            lid.localPosition = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        lid.localPosition = to;
    }

    const string kOpenPrompt = "Open hive";
    const string kClosePrompt = "Put the lid back";
    const string kHarvestPrompt = "Harvest honey";
    const float kMinHarvestKg = 0.05f;
    const float kLidLiftMeters = 0.25f;
    const float kMinLegSeconds = 0.05f;
}

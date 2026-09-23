using System.Collections;
using UnityEngine;

/// One frame in a hive. With the hive open, Interact lifts it out and holds it
/// up to the player's eyes; Interact again puts it back. The comb shows the
/// colony's honey stores, and the readout covers bees, brood and queen — a
/// first pass at inspection (QUESTIONS.md #11, #12), nothing diagnosed yet.
public class HiveFrame : Interactable
{
    [Header("References")]
    public HiveInteraction hive;

    [Tooltip("Capped-honey band of the comb, resized from the colony's stores when the frame is pulled")]
    public Transform honeyBand;

    [Tooltip("Brood band of the comb; fills whatever the honey doesn't")]
    public Transform broodBand;

    [Header("Comb")]
    [Tooltip("Position of this frame in the box, counted from 1, shown in the readout")]
    public int frameNumber = 1;

    [Tooltip("Share of the colony's honey on this frame relative to the average frame; outer frames hold more")]
    public float honeyShare = 1f;

    [Tooltip("Height of the comb below the top bar, in meters")]
    public float combHeightMeters = 0.4f;

    [Header("Handling")]
    [Tooltip("Where the frame is held, relative to the camera, in meters")]
    public Vector3 holdLocalPosition = new Vector3(0f, 0.2f, 0.45f);

    [Tooltip("Real seconds for each half of pulling the frame out or putting it back")]
    public float moveLegSeconds = 0.3f;

    public bool IsHeld { get; private set; }

    public override string Prompt => IsHeld ? kPutBackPrompt : kPullPrompt;

    public override bool CanInteract => hive.IsOpen && move == null && (IsHeld || !hive.IsFrameOut);

    // Looking down at the frames of an open hive is where harvesting happens,
    // so the hive's harvest is offered here too — but not with a frame in hand.
    public override string SecondaryPrompt => IsHeld ? null : hive.SecondaryPrompt;

    public override void SecondaryInteract(PlayerInteractor player)
    {
        hive.SecondaryInteract(player);
    }

    Transform slotParent;
    Vector3 slotLocalPosition;
    Quaternion slotLocalRotation;
    Collider[] colliders;
    FirstPersonController holder;
    Coroutine move;

    void Awake()
    {
        slotParent = transform.parent;
        slotLocalPosition = transform.localPosition;
        slotLocalRotation = transform.localRotation;
        colliders = GetComponentsInChildren<Collider>();
    }

    public override void Interact(PlayerInteractor player)
    {
        if (IsHeld)
            move = StartCoroutine(PutBackRoutine());
        else
            Pull(player);
    }

    void Pull(PlayerInteractor player)
    {
        IsHeld = true;
        hive.IsFrameOut = true;

        // Hands are full: no walking while holding a frame. Triggers stay
        // hittable by the look ray but can't snag the character controller.
        holder = player.GetComponent<FirstPersonController>();
        if (holder != null)
            holder.MovementLocked = true;
        SetCollidersTrigger(true);

        float honeyFill = UpdateComb();
        player.hud.ShowMessage(DescribeFrame(honeyFill));
        move = StartCoroutine(PullRoutine(player.viewCamera.transform));
    }

    // Straight up until the comb clears the box, then to the player's hands.
    IEnumerator PullRoutine(Transform hands)
    {
        Vector3 lifted = slotLocalPosition + Vector3.up * kLiftMeters;
        yield return MoveLocal(slotLocalPosition, slotLocalRotation, lifted, slotLocalRotation);

        transform.SetParent(hands, true);
        yield return MoveLocal(transform.localPosition, transform.localRotation, holdLocalPosition, kHoldLocalRotation);
        move = null;
    }

    // The reverse: back above the slot, then straight down into it.
    IEnumerator PutBackRoutine()
    {
        Vector3 lifted = slotLocalPosition + Vector3.up * kLiftMeters;
        transform.SetParent(slotParent, true);
        yield return MoveLocal(transform.localPosition, transform.localRotation, lifted, slotLocalRotation);
        yield return MoveLocal(lifted, slotLocalRotation, slotLocalPosition, slotLocalRotation);

        IsHeld = false;
        hive.IsFrameOut = false;
        SetCollidersTrigger(false);
        if (holder != null)
            holder.MovementLocked = false;
        holder = null;
        move = null;
    }

    IEnumerator MoveLocal(Vector3 fromPosition, Quaternion fromRotation, Vector3 toPosition, Quaternion toRotation)
    {
        float seconds = Mathf.Max(kMinLegSeconds, moveLegSeconds);
        for (float t = 0f; t < 1f; t += Time.deltaTime / seconds)
        {
            float eased = Mathf.SmoothStep(0f, 1f, t);
            transform.localPosition = Vector3.Lerp(fromPosition, toPosition, eased);
            transform.localRotation = Quaternion.Slerp(fromRotation, toRotation, eased);
            yield return null;
        }
        transform.localPosition = toPosition;
        transform.localRotation = toRotation;
    }

    /// Resizes the comb bands from the colony's stores, returns this frame's honey fill (0–1).
    float UpdateComb()
    {
        HiveController colony = hive.Colony;
        float storesFraction = colony.honeyCapacityKg > 0f ? colony.honeyStoredKg / colony.honeyCapacityKg : 0f;
        float honeyFill = Mathf.Clamp01(storesFraction * honeyShare);

        SetBand(honeyBand, 0f, honeyFill);
        SetBand(broodBand, honeyFill, 1f);
        return honeyFill;
    }

    // Stretches a band over [fromFraction, toFraction] of the comb, measured down from the top bar.
    void SetBand(Transform band, float fromFraction, float toFraction)
    {
        float heightMeters = (toFraction - fromFraction) * combHeightMeters;
        bool visible = heightMeters > kMinBandMeters;
        band.gameObject.SetActive(visible);
        if (!visible)
            return;

        float centreDepth = kTopBarDepthMeters + fromFraction * combHeightMeters + heightMeters * 0.5f;
        band.localScale = new Vector3(band.localScale.x, heightMeters, band.localScale.z);
        band.localPosition = new Vector3(band.localPosition.x, -centreDepth, band.localPosition.z);
    }

    void SetCollidersTrigger(bool isTrigger)
    {
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].isTrigger = isTrigger;
    }

    string DescribeFrame(float honeyFill)
    {
        HiveController colony = hive.Colony;
        float beeCoverage = Mathf.Clamp01((float)colony.currentPopulation / colony.maxPopulation);
        return $"Frame {frameNumber}: about {honeyFill:P0} of the comb is capped honey; bees cover about {beeCoverage:P0} of it.\n" +
               $"{DescribeBrood(colony.health)} {DescribeQueen(colony.queenStatus)}";
    }

    static string DescribeBrood(float health)
    {
        if (health >= kSolidBroodHealth)
            return "The brood is solid and healthy.";
        if (health >= kPatchyBroodHealth)
            return "The brood is patchy.";
        return "The brood is sparse and sickly.";
    }

    static string DescribeQueen(QueenStatus status)
    {
        switch (status)
        {
            case QueenStatus.Healthy:
                return "Fresh eggs in the cells: the queen is laying.";
            case QueenStatus.Weak:
                return "Only a few eggs: the queen may be failing.";
            default:
                return "No eggs anywhere: the colony may be queenless.";
        }
    }

    // Turned so the comb faces the camera.
    static readonly Quaternion kHoldLocalRotation = Quaternion.Euler(0f, 90f, 0f);

    const string kPullPrompt = "Pull frame";
    const string kPutBackPrompt = "Put frame back";
    const float kLiftMeters = 0.45f;
    const float kTopBarDepthMeters = 0.01f;
    const float kMinBandMeters = 0.002f;
    const float kMinLegSeconds = 0.05f;
    const float kSolidBroodHealth = 0.8f;
    const float kPatchyBroodHealth = 0.5f;
}

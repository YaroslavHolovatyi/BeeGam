using UnityEngine;
using UnityEngine.UI;

/// The on-foot HUD stub: crosshair, the interact prompt, a short-lived
/// message line, the in-game clock, and carried honey + gold. Deliberately
/// plain — HUD philosophy (QUESTIONS.md #32) is still open.
public class PlayerHud : MonoBehaviour
{
    [Header("References")]
    public Text promptText;
    public Text messageText;
    public Text clockText;
    public Text walletText;
    public GameClock clock;
    public EconomyManager economy;
    public PlayerInventory inventory;

    [Header("Messages")]
    [Tooltip("How long a message stays on screen, in real seconds")]
    public float messageSeconds = 5f;

    float messageHideTime;
    int shownClockStep = -1;
    int shownHoneyGrams = -1;
    int shownGoldCents = -1;

    void Awake()
    {
        SetPrompt(null);
        messageText.enabled = false;
    }

    void Update()
    {
        if (messageText.enabled && Time.unscaledTime >= messageHideTime)
            messageText.enabled = false;

        UpdateClock();
        UpdateWallet();
    }

    // Compared in whole grams and cents, so the string is rebuilt only when
    // the shown numbers actually change (after a harvest or a sale).
    void UpdateWallet()
    {
        if (walletText == null || economy == null || inventory == null)
            return;

        int honeyGrams = Mathf.RoundToInt(inventory.carriedHoneyKg * 1000f);
        int goldCents = Mathf.RoundToInt(economy.goldBalance * 100f);
        if (honeyGrams == shownHoneyGrams && goldCents == shownGoldCents)
            return;

        shownHoneyGrams = honeyGrams;
        shownGoldCents = goldCents;
        walletText.text = $"Honey  {inventory.carriedHoneyKg:0.00} kg\nGold  {economy.goldBalance:0.00}";
    }

    // Rebuilds the clock string only when the shown time changes, not every frame.
    void UpdateClock()
    {
        if (clock == null || clockText == null)
            return;

        int step = Mathf.FloorToInt(clock.TotalGameHours * kClockStepsPerHour);
        if (step == shownClockStep)
            return;

        shownClockStep = step;
        clockText.text = $"Day {clock.Day}   {GameClock.FormatTimeOfDay(clock.HourOfDay)}";
    }

    /// Shows the prompt under the crosshair; null hides it.
    public void SetPrompt(string prompt)
    {
        promptText.enabled = prompt != null;
        promptText.text = prompt ?? string.Empty;
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.enabled = true;
        messageHideTime = Time.unscaledTime + messageSeconds;
    }

    // The clock readout moves in 10-minute steps.
    const float kClockStepsPerHour = 6f;
}

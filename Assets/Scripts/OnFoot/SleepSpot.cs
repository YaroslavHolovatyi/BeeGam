using UnityEngine;

/// Where the player goes to sleep: skips in-game time to the next morning.
/// The hives are credited for the skipped hours through the normal ticker.
public class SleepSpot : Interactable
{
    [Header("References")]
    public GameClock clock;

    public override string Prompt => kSleepPrompt;

    public override void Interact(PlayerInteractor player)
    {
        float sleptHours = clock.SleepUntilMorning();
        player.hud.ShowMessage(
            $"You slept {sleptHours:0.#} hours. Day {clock.Day}, {GameClock.FormatTimeOfDay(clock.HourOfDay)}.");
    }

    const string kSleepPrompt = "Sleep until morning";
}

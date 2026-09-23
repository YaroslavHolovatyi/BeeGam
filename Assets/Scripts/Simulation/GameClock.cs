using UnityEngine;

/// In-game time. Runs faster than real time and is the only clock the
/// simulation reads; HiveTicker listens to HoursAdvanced to step the hives.
public class GameClock : MonoBehaviour
{
    [Header("Pace")]
    [Min(1f)]
    [Tooltip("Real seconds one in-game day (24 h) lasts. Placeholder until day length is decided (GDD worksheet, Appendix F #80)")]
    public float realSecondsPerGameDay = 1200f;

    [Header("Start")]
    [Min(1)]
    public int startDay = 1;

    [Range(0f, 24f)]
    [Tooltip("In-game hour the game starts at")]
    public float startHour = 6f;

    [Header("Sleep")]
    [Range(0f, 24f)]
    [Tooltip("In-game hour the player wakes at after sleeping")]
    public float wakeHour = 6f;

    /// Raised whenever in-game time moves forward, with the in-game hours that passed.
    public event System.Action<float> HoursAdvanced;

    /// In-game hours since day 1, 00:00.
    public float TotalGameHours { get; private set; }

    public int Day => Mathf.FloorToInt(TotalGameHours / 24f) + 1;

    /// In-game hour of the current day, 0–24.
    public float HourOfDay => TotalGameHours - (Day - 1) * 24f;

    void Awake()
    {
        TotalGameHours = (startDay - 1) * 24f + startHour;
    }

    void Update()
    {
        Advance(Time.deltaTime * 24f / realSecondsPerGameDay);
    }

    public void Advance(float gameHours)
    {
        if (gameHours <= 0f)
            return;

        TotalGameHours += gameHours;
        HoursAdvanced?.Invoke(gameHours);
    }

    /// Skips to the next wake-up hour, returns the in-game hours slept.
    public float SleepUntilMorning()
    {
        float target = (Day - 1) * 24f + wakeHour;
        if (target <= TotalGameHours)
            target += 24f;

        float sleptHours = target - TotalGameHours;
        Advance(sleptHours);
        return sleptHours;
    }

    /// Formats an hour of the day (0–24) as "HH:MM".
    public static string FormatTimeOfDay(float hourOfDay)
    {
        int minuteOfDay = Mathf.FloorToInt(hourOfDay * 60f) % (24 * 60);
        return $"{minuteOfDay / 60:00}:{minuteOfDay % 60:00}";
    }
}

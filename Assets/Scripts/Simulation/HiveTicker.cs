using System.Collections.Generic;
using UnityEngine;

/// Steps every hive on a fixed in-game-time interval, so simulation cost
/// scales with in-game time rather than with frames × hives. Hives register
/// themselves in OnEnable.
public class HiveTicker : MonoBehaviour
{
    [Header("References")]
    public GameClock clock;

    [Header("Stepping")]
    [Min(0.01f)]
    [Tooltip("In-game hours per simulation step. Smaller = smoother readouts, more steps when sleeping")]
    public float tickIntervalGameHours = 0.25f;

    static readonly List<HiveController> hives = new List<HiveController>();

    float pendingGameHours;

    public static void Register(HiveController hive)
    {
        if (!hives.Contains(hive))
            hives.Add(hive);
    }

    public static void Unregister(HiveController hive)
    {
        hives.Remove(hive);
    }

    // Static state survives between Play sessions when domain reload is off.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        hives.Clear();
    }

    void OnEnable()
    {
        clock.HoursAdvanced += OnHoursAdvanced;
    }

    void OnDisable()
    {
        clock.HoursAdvanced -= OnHoursAdvanced;
    }

    void OnHoursAdvanced(float gameHours)
    {
        float step = Mathf.Max(kMinTickGameHours, tickIntervalGameHours);
        pendingGameHours += gameHours;

        while (pendingGameHours >= step)
        {
            // The clock has already moved to the end of the pending time;
            // sample the time of day at the middle of this step.
            float stepStartHours = clock.TotalGameHours - pendingGameHours;
            float hourOfDay = Mathf.Repeat(stepStartHours + step * 0.5f, 24f);

            for (int i = 0; i < hives.Count; i++)
                hives[i].Tick(step, hourOfDay);

            pendingGameHours -= step;
        }
    }

    const float kMinTickGameHours = 0.01f;
}

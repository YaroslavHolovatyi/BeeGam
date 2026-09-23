using UnityEngine;

public enum QueenStatus
{
    Healthy,
    Weak,
    Missing
}

[RequireComponent(typeof(Transform))]
public class HiveController : MonoBehaviour
{
    [Header("Breed")]
    public BeeBreedData breed;

    [Header("Population")]
    public int maxPopulation = 20000;
    public int currentPopulation = 10000;

    [Header("Health")]
    [Range(0f, 1f)]
    public float health = 1f;
    public QueenStatus queenStatus = QueenStatus.Healthy;

    [Header("Honey")]
    public float honeyCapacityKg = 20f;
    public float honeyStoredKg = 0f;

    [Tooltip("Fraction of stored honey left behind on harvest so the colony has reserves")]
    [Range(0f, 1f)]
    public float harvestReserveFraction = 0.2f;

    void OnEnable()
    {
        HiveTicker.Register(this);
    }

    void OnDisable()
    {
        HiveTicker.Unregister(this);
    }

    /// Advances the colony by an in-game interval. Called by HiveTicker, never per frame.
    public void Tick(float gameHours, float hourOfDay)
    {
        AccumulateHoney(gameHours, hourOfDay);
    }

    void AccumulateHoney(float gameHours, float hourOfDay)
    {
        if (breed == null || queenStatus == QueenStatus.Missing)
            return;

        // Bees only forage in daylight (PLAN.md: dormant at night).
        if (hourOfDay < kForageStartHour || hourOfDay >= kForageEndHour)
            return;

        float populationFactor = Mathf.Clamp01((float)currentPopulation / maxPopulation);
        float honeyPerHour = breed.honeyYieldMultiplier * populationFactor * health * kBaseHoneyKgPerForagingHour;

        honeyStoredKg = Mathf.Min(honeyCapacityKg, honeyStoredKg + honeyPerHour * gameHours);
    }

    /// Honey above the reserve fraction, in kg — what Harvest would take right now.
    public float HarvestableKg => Mathf.Max(0f, honeyStoredKg - honeyCapacityKg * harvestReserveFraction);

    /// Harvests all honey above the reserve fraction, returns the amount harvested in kg.
    public float Harvest()
    {
        float harvestable = HarvestableKg;
        honeyStoredKg -= harvestable;
        return harvestable;
    }

    // Placeholders for the sim-balance pass: a full-strength, healthy colony at
    // yield 1.0 makes about 5 kg over a 13-hour foraging day.
    const float kBaseHoneyKgPerForagingHour = 5f / 13f;
    const float kForageStartHour = 7f;
    const float kForageEndHour = 20f;
}

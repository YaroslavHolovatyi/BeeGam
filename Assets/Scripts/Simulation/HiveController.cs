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

    void Update()
    {
        AccumulateHoney(Time.deltaTime);
    }

    void AccumulateHoney(float deltaTimeSeconds)
    {
        if (breed == null || queenStatus == QueenStatus.Missing)
            return;

        float populationFactor = Mathf.Clamp01((float)currentPopulation / maxPopulation);
        float honeyPerSecond = breed.honeyYieldMultiplier * populationFactor * health * kBaseHoneyPerSecond;

        honeyStoredKg = Mathf.Min(honeyCapacityKg, honeyStoredKg + honeyPerSecond * deltaTimeSeconds);
    }

    /// Harvests all honey above the reserve fraction, returns the amount harvested in kg.
    public float Harvest()
    {
        float reserve = honeyCapacityKg * harvestReserveFraction;
        float harvestable = Mathf.Max(0f, honeyStoredKg - reserve);
        honeyStoredKg -= harvestable;
        return harvestable;
    }

    const float kBaseHoneyPerSecond = 0.0001f;
}

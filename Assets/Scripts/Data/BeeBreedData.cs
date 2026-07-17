using UnityEngine;

[CreateAssetMenu(fileName = "NewBeeBreed", menuName = "BeeKeeper/Bee Breed")]
public class BeeBreedData : ScriptableObject
{
    public string breedName = "Italian";

    [Tooltip("Honey produced per bee per in-game day, at full health, relative to baseline 1.0")]
    public float honeyYieldMultiplier = 1f;

    [Range(0f, 1f)]
    [Tooltip("Higher = calmer, less likely to sting/swarm when disturbed")]
    public float temperament = 0.5f;

    [Range(0f, 1f)]
    public float diseaseResistance = 0.5f;

    [Range(0f, 1f)]
    [Tooltip("Higher = better winter survival and cold-weather activity")]
    public float coldTolerance = 0.5f;

    [Tooltip("Max distance in meters bees of this breed will forage from the hive")]
    public float foragingRangeMeters = 1500f;
}

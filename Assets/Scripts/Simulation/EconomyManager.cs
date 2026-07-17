using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    public float goldBalance = 100f;
    public float honeyPricePerKg = 15f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public float SellHoney(float amountKg)
    {
        float earned = amountKg * honeyPricePerKg;
        goldBalance += earned;
        return earned;
    }

    public bool TrySpend(float amount)
    {
        if (amount > goldBalance)
            return false;

        goldBalance -= amount;
        return true;
    }
}

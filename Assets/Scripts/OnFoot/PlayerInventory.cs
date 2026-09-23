using UnityEngine;

/// What the on-foot player is carrying. Just harvested honey for now; tools,
/// frames and materials join it as the slice grows.
public class PlayerInventory : MonoBehaviour
{
    [Header("Honey")]
    [Tooltip("Harvested honey the player is carrying, in kg")]
    public float carriedHoneyKg = 0f;
}

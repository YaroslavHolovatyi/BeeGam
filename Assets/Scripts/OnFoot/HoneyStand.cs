using UnityEngine;

/// A roadside honey stand: sells everything the player is carrying at the
/// economy's price. A stand-in for however selling ends up working
/// (QUESTIONS.md #14–#16 — currency, pricing, contracts).
public class HoneyStand : Interactable
{
    [Header("References")]
    public EconomyManager economy;

    public override string Prompt => kSellPrompt;

    public override void Interact(PlayerInteractor player)
    {
        float honeyKg = player.inventory.carriedHoneyKg;
        if (honeyKg < kMinSaleKg)
        {
            player.hud.ShowMessage("You have no honey to sell. Harvest some from an open hive with F.");
            return;
        }

        float earned = economy.SellHoney(honeyKg);
        player.inventory.carriedHoneyKg = 0f;
        player.hud.ShowMessage($"Sold {honeyKg:0.00} kg of honey for {earned:0.00} gold.");
    }

    const string kSellPrompt = "Sell honey";
    const float kMinSaleKg = 0.005f;
}

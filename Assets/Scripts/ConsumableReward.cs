using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Reward", menuName = "Rewards/Consumable")]
public class ConsumableReward : Reward
{
    public int amount;
    public enum ConsumableType { Currency, Hexes }
    public ConsumableType type;

    public override void Apply()
    {
        switch (type)
        {
            case ConsumableType.Currency:
                // Add currency to player
                break;
            case ConsumableType.Hexes:
                // Add hexes to player
                break;
        }
    }
}
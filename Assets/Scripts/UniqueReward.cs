using UnityEngine;

[CreateAssetMenu(fileName = "New Unique Reward", menuName = "Rewards/Unique")]
public class UniqueReward : Reward
{
    public enum UniqueType { Skin, SpecialItem }
    public UniqueType type;

    public override void Apply()
    {
        switch (type)
        {
            case UniqueType.Skin:
                // Unlock skin for player
                break;
            case UniqueType.SpecialItem:
                // Give special item to player
                break;
        }
    }
}
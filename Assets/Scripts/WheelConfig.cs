using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Wheel Config", menuName = "Wheel of Luck/Wheel Config")]
public class WheelConfig : ScriptableObject
{
    [Header("Wheel Settings")]
    public int totalPositions = 8;
    public int consumablePositions = 6;
    public int itemPositions = 2;
    public int freeSpins = 1;
    public float wheelSpeed = 1f;

    [Header("Reward Settings")]
    public List<Reward> initialCustomRewards;
    public int customFalloutRewards = 3;
    public AnimationCurve customFalloutCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public int GetTotalRewardPositions()
    {
        return consumablePositions + itemPositions;
    }

    public bool IsValidConfiguration()
    {
        return GetTotalRewardPositions() == totalPositions;
    }
}
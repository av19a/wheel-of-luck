using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Wheel Config", menuName = "Wheel of Luck/Wheel Config")]
public class WheelConfig : ScriptableObject
{
    public int totalPositions = 8;
    public int consumablePositions = 6;
    public int itemPositions = 2;
    public int freeSpins = 1;
    public float wheelSpeed = 1f;
    public List<Reward> initialCustomRewards;
}
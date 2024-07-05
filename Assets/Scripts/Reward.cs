using UnityEngine;

public abstract class Reward : ScriptableObject
{
    public string rewardName;
    public Sprite icon;
    public float probability;

    public abstract void Apply();
}
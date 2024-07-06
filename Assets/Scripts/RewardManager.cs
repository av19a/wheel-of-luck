using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RewardManager : MonoBehaviour
{
    [SerializeField] private List<Reward> rewardPool;
    [SerializeField] private WheelConfig config;

    public List<Reward> GetInitialRewards()
    {
        List<Reward> initialRewards = new List<Reward>();

        for (int i = 0; i < config.totalPositions; i++)
        {
            initialRewards.Add(GetRewardForPosition(i));
        }

        return initialRewards;
    }

    private Reward GetRewardForPosition(int position)
    {
        if (position < config.initialCustomRewards.Count)
        {
            return config.initialCustomRewards[position];
        }
        return GetRandomReward();
    }

    public void ClaimReward(Reward reward)
    {
        reward.Apply();
        if (reward is UniqueReward)
        {
            RemoveUniqueReward(reward);
        }
    }

    private void RemoveUniqueReward(Reward reward)
    {
        rewardPool.Remove(reward);
    }

    public Reward GetNewReward()
    {
        return GetRandomReward();
    }

    private Reward GetRandomReward()
    {
        float totalProbability = rewardPool.Sum(r => r.probability);
        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        foreach (Reward reward in rewardPool)
        {
            cumulativeProbability += reward.probability;
            if (randomValue <= cumulativeProbability)
            {
                return reward;
            }
        }

        return rewardPool[rewardPool.Count - 1];
    }
}
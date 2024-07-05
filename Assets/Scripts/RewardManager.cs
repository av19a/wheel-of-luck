using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RewardManager : MonoBehaviour
{
    public List<Reward> rewardPool;

    public List<Reward> GetInitialRewards()
    {
        WheelConfig config = GetComponent<WheelOfLuck>().config;
        List<Reward> initialRewards = new List<Reward>();

        for (int i = 0; i < config.totalPositions; i++)
        {
            if (i < config.initialCustomRewards.Count)
            {
                initialRewards.Add(config.initialCustomRewards[i]);
            }
            else
            {
                initialRewards.Add(GetRandomReward());
            }
        }

        return initialRewards;
    }

    public void ClaimReward(Reward reward)
    {
        reward.Apply();
        if (reward is UniqueReward)
        {
            rewardPool.Remove(reward);
        }
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
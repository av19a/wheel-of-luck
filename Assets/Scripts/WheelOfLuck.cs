using UnityEngine;
using System.Collections.Generic;

public class WheelOfLuck : MonoBehaviour
{
    public WheelConfig config;
    [HideInInspector]
    public List<Reward> currentRewards = new List<Reward>();
    [HideInInspector]
    public int spinCount = 0;
    private RewardManager rewardManager;
    private WheelSpinner spinner;
    private UIManager uiManager;
    [HideInInspector]
    public float currentRotation = 0f;

    private void Awake()
    {
        rewardManager = GetComponent<RewardManager>();
        spinner = GetComponent<WheelSpinner>();
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        InitializeWheel();
    }

    private void InitializeWheel()
    {
        currentRewards = rewardManager.GetInitialRewards();
        uiManager.SetupWheelDisplay(currentRewards);
    }

    public void Spin()
    {
        if (spinCount < config.freeSpins)
        {
            PerformSpin();
        }
        // else if (PlayerHasEnoughCurrency())
        // {
        //     DeductSpinCost();
        //     PerformSpin();
        // }
        else
        {
            uiManager.ShowInsufficientCurrencyMessage();
        }
    }

    private void PerformSpin()
    {
        spinCount++;
        spinner.Spin(OnSpinComplete);
    }

    private void OnSpinComplete(Reward reward)
    {
        rewardManager.ClaimReward(reward);
        uiManager.ShowRewardClaimedMessage(reward);
        Reward newReward = rewardManager.GetNewReward();
        currentRewards[currentRewards.IndexOf(reward)] = newReward;
        uiManager.UpdateWheelDisplay(currentRewards);
    }
    
    private List<Reward> GetRandomRewards()
    {
        List<Reward> randomRewards = new List<Reward>();
        List<Reward> tempRewards = new List<Reward>(currentRewards);

        for (int i = 0; i < config.totalPositions; i++)
        {
            if (tempRewards.Count == 0)
            {
                tempRewards = new List<Reward>(currentRewards);
            }

            int randomIndex = Random.Range(0, tempRewards.Count);
            randomRewards.Add(tempRewards[randomIndex]);
            tempRewards.RemoveAt(randomIndex);
        }

        return randomRewards;
    }

    private bool PlayerHasEnoughCurrency()
    {
        // Implement currency check logic
        return true;
    }

    private void DeductSpinCost()
    {
        // Implement spin cost deduction logic
    }
}
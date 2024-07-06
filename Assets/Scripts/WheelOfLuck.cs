using UnityEngine;
using System.Collections.Generic;

public class WheelOfLuck : MonoBehaviour
{
    [SerializeField] public WheelConfig config;
    [SerializeField] public List<Reward> mandatoryRewards = new List<Reward>();
    public IReadOnlyList<Reward> MandatoryRewards => mandatoryRewards;

    private RewardManager rewardManager;
    private WheelSpinner spinner;
    private UIManager uiManager;

    public List<Reward> CurrentRewards { get; private set; } = new List<Reward>();
    public int SpinCount { get; private set; } = 0;
    public float CurrentRotation { get; set; } = 0;
    public int CurrentMandatoryRewardIndex { get; private set; } = 0;

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
        CurrentRewards = rewardManager.GetInitialRewards();
        ApplyMandatoryRewardIfNeeded();
        uiManager.SetupWheelDisplay(CurrentRewards);
    }

    private void ApplyMandatoryRewardIfNeeded()
    {
        if (mandatoryRewards.Count > 0 && CurrentRewards.Count > 0)
        {
            CurrentRewards[0] = mandatoryRewards[0];
            CurrentMandatoryRewardIndex = 0;
        }
    }

    public void Spin()
    {
        if (CanSpin())
        {
            PerformSpin();
        }
        else
        {
            uiManager.ShowInsufficientCurrencyMessage();
        }
    }

    private bool CanSpin()
    {
        return SpinCount < config.freeSpins || PlayerHasEnoughCurrency();
    }

    private void PerformSpin()
    {
        SpinCount++;
        if (SpinCount > config.freeSpins)
        {
            DeductSpinCost();
        }
        spinner.Spin(OnSpinComplete);
    }

    private void OnSpinComplete(Reward reward)
    {
        rewardManager.ClaimReward(reward);
        uiManager.ShowRewardClaimedMessage(reward);
        UpdateRewards();
    }

    private void UpdateRewards()
    {
        List<Reward> updatedRewards = GetUpdatedRewards();
        CurrentRewards = updatedRewards;
        uiManager.UpdateWheelDisplay(updatedRewards);
    }

    private List<Reward> GetUpdatedRewards()
    {
        List<Reward> updatedRewards = new List<Reward>(CurrentRewards);

        if (CurrentMandatoryRewardIndex < mandatoryRewards.Count)
        {
            UpdateMandatoryReward(updatedRewards);
        }
        else
        {
            UpdateRandomRewards(updatedRewards);
        }

        return updatedRewards;
    }

    private void UpdateMandatoryReward(List<Reward> updatedRewards)
    {
        int claimedIndex = updatedRewards.FindIndex(r => r == mandatoryRewards[CurrentMandatoryRewardIndex]);
        claimedIndex = claimedIndex == -1 ? 0 : claimedIndex;

        CurrentMandatoryRewardIndex++;

        if (CurrentMandatoryRewardIndex < mandatoryRewards.Count)
        {
            updatedRewards[claimedIndex] = mandatoryRewards[CurrentMandatoryRewardIndex];
        }
        else
        {
            updatedRewards[claimedIndex] = GetRandomReward();
        }
    }

    private void UpdateRandomRewards(List<Reward> updatedRewards)
    {
        for (int i = 0; i < updatedRewards.Count; i++)
        {
            updatedRewards[i] = GetRandomReward();
        }
    }

    private Reward GetRandomReward()
    {
        return rewardManager.GetNewReward();
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
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
    
    public List<Reward> mandatoryRewards = new List<Reward>();
    public int currentMandatoryRewardIndex = 0;

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
        if (mandatoryRewards.Count > 0)
        {
            // Ensure we have at least one reward in currentRewards
            if (currentRewards.Count == 0)
            {
                currentRewards.Add(mandatoryRewards[0]);
            }
            else
            {
                currentRewards[0] = mandatoryRewards[0];
            }
            currentMandatoryRewardIndex = 0;
        }
        uiManager.SetupWheelDisplay(currentRewards);
    }

     public void Spin()                                                                                                                                        
     {                                                                                                                                                         
         if (spinCount < config.freeSpins)                                                                                                                     
         {                                                                                                                                                     
             PerformSpin();                                                                                                                                    
         }                                                                                                                                                     
         else if (PlayerHasEnoughCurrency())                                                                                                                
         {                                                                                                                                                  
             DeductSpinCost();                                                                                                                              
             PerformSpin();                                                                                                                                 
         }                                                                                                                                                  
         else                                                                                                                                                  
         {                                                                                                                                                     
             uiManager.ShowInsufficientCurrencyMessage();                                                                                                      
         }                                                                                                                                                     
     }                                                                                                                                                         
                                                                                                                                                               
     private void PerformSpin()                                                                                                                                
     {                                                                                                                                                         
         spinCount++;                                                                                                                                          
         spinner.Spin(OnSpinComplete, config.customFalloutRewards, config.customFalloutCurve);                                                                 
     }

    private void OnSpinComplete(Reward reward)
    {
        rewardManager.ClaimReward(reward);
        uiManager.ShowRewardClaimedMessage(reward);
        List<Reward> newRewards = GetUpdatedRewards();
        uiManager.UpdateWheelDisplay(newRewards);
    }
    
    private List<Reward> GetUpdatedRewards()
    {
        List<Reward> updatedRewards = new List<Reward>(currentRewards);

        if (currentMandatoryRewardIndex < mandatoryRewards.Count)
        {
            // Find the index of the current mandatory reward
            int claimedIndex = updatedRewards.FindIndex(r => r == mandatoryRewards[currentMandatoryRewardIndex]);

            // If the current mandatory reward is not found, use the first available slot
            if (claimedIndex == -1)
            {
                claimedIndex = 0;
            }

            currentMandatoryRewardIndex++;

            if (currentMandatoryRewardIndex < mandatoryRewards.Count)
            {
                // Replace with the next mandatory reward
                updatedRewards[claimedIndex] = mandatoryRewards[currentMandatoryRewardIndex];
            }
            else
            {
                // If we've used all mandatory rewards, replace with a random one
                updatedRewards[claimedIndex] = GetRandomReward();
            }
        }
        else
        {
            // If all mandatory rewards have been used, update rewards randomly
            for (int i = 0; i < updatedRewards.Count; i++)
            {
                updatedRewards[i] = GetRandomReward();
            }
        }

        return updatedRewards;
    }
    
    private Reward GetRandomReward()
    {
        return currentRewards[Random.Range(0, currentRewards.Count)];
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

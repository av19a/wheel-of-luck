using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class WheelSpinner : MonoBehaviour
{
    public float spinDuration = 4f;
    public AnimationCurve spinCurve;

    private bool isSpinning = false;
    private UIManager uiManager;
    private WheelOfLuck wheelOfLuck;

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
        wheelOfLuck = GetComponent<WheelOfLuck>();
    }

    public void Spin(Action<Reward> onComplete, int customFalloutRewards, AnimationCurve customFalloutCurve)
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinCoroutine(onComplete, customFalloutRewards, customFalloutCurve));
        }
    }

    private IEnumerator SpinCoroutine(Action<Reward> onComplete, int customFalloutRewards, AnimationCurve customFalloutCurve)
    {
        isSpinning = true;
        float elapsedTime = 0f;
        float startRotation = wheelOfLuck.currentRotation;
        float endRotation;

        // If there are mandatory rewards left, calculate the end rotation to land on the mandatory reward
        if (wheelOfLuck.currentMandatoryRewardIndex < wheelOfLuck.mandatoryRewards.Count)
        {
            endRotation = CalculateRotationForMandatoryReward(startRotation);
        }
        else
        {
            // If no mandatory rewards left, use the original random rotation
            float additionalRotation = CalculateAdditionalRotation(customFalloutRewards, customFalloutCurve);
            endRotation = startRotation + additionalRotation;
        }

        while (elapsedTime < spinDuration)
        {
            float t = elapsedTime / spinDuration;
            float curveValue = spinCurve.Evaluate(t);
            float currentRotation = Mathf.Lerp(startRotation, endRotation, curveValue);
            uiManager.RotateWheel(currentRotation);
            wheelOfLuck.currentRotation = currentRotation % 360f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        uiManager.RotateWheel(endRotation);
        wheelOfLuck.currentRotation = endRotation % 360f;

        Reward selectedReward = uiManager.GetClosestRewardToPointer(wheelOfLuck.currentRotation);
        isSpinning = false;
        onComplete(selectedReward);
    }        
     
    private float CalculateRotationForMandatoryReward(float startRotation)
    {
        int rewardCount = wheelOfLuck.currentRewards.Count;
        float anglePerItem = 360f / rewardCount;
        
        // Find the index of the current mandatory reward
        int targetIndex = wheelOfLuck.currentRewards.FindIndex(r => r == wheelOfLuck.mandatoryRewards[wheelOfLuck.currentMandatoryRewardIndex]);
        
        if (targetIndex == -1)
        {
            Debug.LogError("Mandatory reward not found on the wheel!");
            return startRotation + 360f * 5; // Default to 5 full rotations if something went wrong
        }

        // Calculate the angle to the target reward
        float targetAngle = (rewardCount - targetIndex) * anglePerItem;
        
        // Calculate how much we need to rotate to reach the target
        float rotationNeeded = (targetAngle - startRotation + 360f) % 360f;
        
        // Add additional full rotations for effect (e.g., 5 full rotations plus the needed rotation)
        return startRotation + rotationNeeded + 360f * 5;
    }
                                                                                                                                                               
     private float CalculateAdditionalRotation(int customFalloutRewards, AnimationCurve customFalloutCurve)                                                    
     {                                                                                                                                                         
         float baseRotation = Random.Range(720f, 1080f); // 2-3 full rotations                                                                     
         float customFallout = customFalloutCurve.Evaluate(Random.value) * 360f * customFalloutRewards;                                            
         return baseRotation + customFallout;                                                                                                                  
     }
}
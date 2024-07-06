using UnityEngine;
using System.Collections;
using System;

public class WheelSpinner : MonoBehaviour
{
    [SerializeField] private float spinDuration = 4f;
    [SerializeField] private AnimationCurve spinCurve;
    [SerializeField] private WheelConfig wheelConfig;

    private bool isSpinning = false;
    private UIManager uiManager;
    private WheelOfLuck wheelOfLuck;

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
        wheelOfLuck = GetComponent<WheelOfLuck>();
    }

    public void Spin(Action<Reward> onComplete)
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinCoroutine(onComplete));
        }
    }

    private IEnumerator SpinCoroutine(Action<Reward> onComplete)
    {
        isSpinning = true;
        float startRotation = wheelOfLuck.CurrentRotation;
        float endRotation = CalculateEndRotation(startRotation);

        yield return PerformSpinAnimation(startRotation, endRotation);

        Reward selectedReward = DetermineSelectedReward();
        isSpinning = false;
        onComplete?.Invoke(selectedReward);
    }

    private float CalculateEndRotation(float startRotation)
    {
        if (wheelOfLuck.CurrentMandatoryRewardIndex < wheelOfLuck.MandatoryRewards.Count)
        {
            return CalculateRotationForMandatoryReward(startRotation);
        }
        else
        {
            return CalculateRandomEndRotation(startRotation);
        }
    }

    private float CalculateRotationForMandatoryReward(float startRotation)
    {
        int rewardCount = wheelOfLuck.CurrentRewards.Count;
        float anglePerItem = 360f / rewardCount;
        
        int targetIndex = wheelOfLuck.CurrentRewards.FindIndex(r => r == wheelOfLuck.MandatoryRewards[wheelOfLuck.CurrentMandatoryRewardIndex]);
        
        if (targetIndex == -1)
        {
            Debug.LogError("Mandatory reward not found on the wheel!");
            return startRotation + 360f * 5; // Default to 5 full rotations
        }

        float targetAngle = (rewardCount - targetIndex) * anglePerItem;
        float rotationNeeded = (targetAngle - startRotation + 360f) % 360f;
        
        return startRotation + rotationNeeded + 360f * 5; // 5 full rotations plus needed rotation
    }

    private float CalculateRandomEndRotation(float startRotation)
    {
        float baseRotation = UnityEngine.Random.Range(720f, 1080f); // 2-3 full rotations
        float customFallout = wheelConfig.customFalloutCurve.Evaluate(UnityEngine.Random.value) * 360f * wheelConfig.customFalloutRewards;
        return startRotation + baseRotation + customFallout;
    }

    private IEnumerator PerformSpinAnimation(float startRotation, float endRotation)
    {
        float elapsedTime = 0f;
        while (elapsedTime < spinDuration)
        {
            float t = elapsedTime / spinDuration;
            float curveValue = spinCurve.Evaluate(t);
            float currentRotation = Mathf.Lerp(startRotation, endRotation, curveValue);
            UpdateWheelRotation(currentRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UpdateWheelRotation(endRotation);
    }

    private void UpdateWheelRotation(float rotation)
    {
        uiManager.RotateWheel(rotation);
        wheelOfLuck.CurrentRotation = rotation % 360f;
    }

    private Reward DetermineSelectedReward()
    {
        return uiManager.GetClosestRewardToPointer(wheelOfLuck.CurrentRotation);
    }
}
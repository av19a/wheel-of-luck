using UnityEngine;
using System.Collections;
using System;

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
        float elapsedTime = 0f;
        float startRotation = wheelOfLuck.currentRotation;
        float additionalRotation = UnityEngine.Random.Range(720f, 1080f); // 2-3 full rotations
        float endRotation = startRotation + additionalRotation;

        while (elapsedTime < spinDuration)
        {
            float t = elapsedTime / spinDuration;
            float curveValue = spinCurve.Evaluate(t);
            float currentRotation = Mathf.Lerp(startRotation, endRotation, curveValue);
            uiManager.RotateWheel(currentRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        uiManager.RotateWheel(endRotation);
        wheelOfLuck.currentRotation = endRotation % 360f;

        Reward selectedReward = uiManager.GetClosestRewardToPointer(wheelOfLuck.currentRotation);
        isSpinning = false;
        onComplete(selectedReward);
    }
}
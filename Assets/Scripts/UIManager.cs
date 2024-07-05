using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public RectTransform wheelDisplay;
    public GameObject rewardItemPrefab;
    public Button spinButton;
    public Text messageText;
    public float wheelRadius = 150f;
    public float rewardIconSize = 50f;
    public GameObject pointerPrefab;

    private List<RewardPosition> rewardPositions = new List<RewardPosition>();
    private List<GameObject> rewardObjects = new List<GameObject>();
    private GameObject pointer;

    private struct RewardPosition
    {
        public Reward reward;
        public float angle;
    }

    private void Start()
    {
        spinButton.onClick.AddListener(OnSpinButtonClicked);
        CreatePointer();
    }

    private void CreatePointer()
    {
        pointer = Instantiate(pointerPrefab, wheelDisplay.parent);
        RectTransform pointerTransform = pointer.GetComponent<RectTransform>();
        pointerTransform.anchorMin = new Vector2(0.5f, 0.5f);
        pointerTransform.anchorMax = new Vector2(0.5f, 0.5f);
        pointerTransform.anchoredPosition = new Vector2(0, wheelRadius + 100);
    }
    
    public void SetupWheelDisplay(List<Reward> rewards)
    {
        UpdateWheelDisplay(rewards);
    }


    public void UpdateWheelDisplay(List<Reward> rewards)
    {
        ClearCurrentDisplay();
        rewardPositions.Clear();

        int rewardCount = rewards.Count;
        float angleStep = 360f / rewardCount;

        for (int i = 0; i < rewardCount; i++)
        {
            GameObject rewardItem = Instantiate(rewardItemPrefab, wheelDisplay);
            RectTransform rectTransform = rewardItem.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(rewardIconSize, rewardIconSize);

            float angle = i * angleStep;
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Sin(radians) * wheelRadius;
            float y = Mathf.Cos(radians) * wheelRadius;
            rectTransform.anchoredPosition = new Vector2(x, y);

            rectTransform.localRotation = Quaternion.Euler(0, 0, -angle);

            rewardItem.GetComponent<Image>().sprite = rewards[i].icon;

            rewardPositions.Add(new RewardPosition { reward = rewards[i], angle = angle });
            rewardObjects.Add(rewardItem);
        }
    }

    private void ClearCurrentDisplay()
    {
        foreach (GameObject item in rewardObjects)
        {
            Destroy(item);
        }
        rewardObjects.Clear();
    }

    private void OnSpinButtonClicked()
    {
        GetComponent<WheelOfLuck>().Spin();
    }

    public void ShowRewardClaimedMessage(Reward reward)
    {
        messageText.text = $"You won: {reward.rewardName}!";
    }

    public void ShowInsufficientCurrencyMessage()
    {
        messageText.text = "Not enough currency to spin!";
    }

    public void RotateWheel(float angle)
    {
        wheelDisplay.localRotation = Quaternion.Euler(0, 0, -angle);
    }
    
    public Reward GetClosestRewardToPointer(float currentRotation)
    {
        float normalizedRotation = (360 - currentRotation) % 360;
        float closestAngle = float.MaxValue;
        Reward closestReward = null;

        foreach (var rewardPosition in rewardPositions)
        {
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(normalizedRotation, rewardPosition.angle));
            if (angleDifference < closestAngle)
            {
                closestAngle = angleDifference;
                closestReward = rewardPosition.reward;
            }
        }

        return closestReward;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform wheelDisplay;
    [SerializeField] private GameObject rewardItemPrefab;
    [SerializeField] private Button spinButton;
    [SerializeField] private Text messageText;
    [SerializeField] private float wheelRadius = 150f;
    [SerializeField] private float rewardIconSize = 50f;
    [SerializeField] private GameObject pointerPrefab;

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
        InitializeUI();
    }

    private void InitializeUI()
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
        CreateRewardItems(rewards);
    }

    private void CreateRewardItems(List<Reward> rewards)
    {
        int rewardCount = rewards.Count;
        float angleStep = 360f / rewardCount;

        for (int i = 0; i < rewardCount; i++)
        {
            CreateRewardItem(rewards[i], i * angleStep);
        }
    }

    private void CreateRewardItem(Reward reward, float angle)
    {
        GameObject rewardItem = Instantiate(rewardItemPrefab, wheelDisplay);
        SetupRewardItemTransform(rewardItem, angle);
        SetupRewardItemImage(rewardItem, reward);
        
        rewardPositions.Add(new RewardPosition { reward = reward, angle = angle });
        rewardObjects.Add(rewardItem);
    }

    private void SetupRewardItemTransform(GameObject rewardItem, float angle)
    {
        RectTransform rectTransform = rewardItem.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(rewardIconSize, rewardIconSize);

        float radians = angle * Mathf.Deg2Rad;
        float x = Mathf.Sin(radians) * wheelRadius;
        float y = Mathf.Cos(radians) * wheelRadius;
        rectTransform.anchoredPosition = new Vector2(x, y);

        rectTransform.localRotation = Quaternion.Euler(0, 0, -angle);
    }

    private void SetupRewardItemImage(GameObject rewardItem, Reward reward)
    {
        rewardItem.GetComponent<Image>().sprite = reward.icon;
    }

    private void ClearCurrentDisplay()
    {
        foreach (GameObject item in rewardObjects)
        {
            Destroy(item);
        }
        rewardObjects.Clear();
        rewardPositions.Clear();
    }

    private void OnSpinButtonClicked()
    {
        // This should probably be handled through an event system instead
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
        float anglePerItem = 360f / rewardPositions.Count;
        int closestIndex = Mathf.RoundToInt(normalizedRotation / anglePerItem) % rewardPositions.Count;
        return rewardPositions[closestIndex].reward;
    }
}
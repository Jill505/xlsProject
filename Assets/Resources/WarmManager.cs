using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;

public class WarmManager : MonoBehaviour
{
    public CaneUnit nowLoadingCaneUnit;
    public GameCore gameCore;

    public RectTransform warmReferenceSpace;
    public GameObject warmPrefab;
    
    public void loadCaneUnitWarmSpawn()
    {
        Debug.Log("載入"+ gameCore.uiCore.nowLoadingCaneUnit.loadingCane.warmCount+ "隻蟲子");
        for (int i = 0; i < gameCore.uiCore.nowLoadingCaneUnit.loadingCane.warmCount; i++)
        {
            GenerateButton();
        }
    }

    public void GenerateButton()
    {
        if (warmPrefab == null || warmReferenceSpace == null)
        {
            Debug.LogError("請在 Inspector 指定 warmPrefab 和 warmReferenceSpace！");
            return;
        }

        // 預設中心點，可根據需要改為滑鼠位置或其他錨點
        Vector2 basePosition = Vector2.zero;

        // 隨機偏移位置
        float randomX = Random.Range(basePosition.x - 60f, basePosition.x + 180f);
        float randomY = Random.Range(basePosition.y - 290f, basePosition.y + 150f);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        // 產生 Button 實例
        GameObject newButtonObj = Instantiate(warmPrefab, warmReferenceSpace);

        // 設定 RectTransform 屬性
        RectTransform rectTransform = newButtonObj.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        //rectTransform.sizeDelta = new Vector2(1f, 1f);
        rectTransform.anchoredPosition = randomPosition;
    }

    public void cleanAllWarm()
    {
        Debug.Log("清除螢幕上所有蟲子");

        //destroy所有子物件
        for (int i = warmReferenceSpace.childCount - 1; i >= 0; i--)
        {
            Transform child = warmReferenceSpace.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void spawnWarm()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

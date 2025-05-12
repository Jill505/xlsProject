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
        Debug.Log("���J"+ gameCore.uiCore.nowLoadingCaneUnit.loadingCane.warmCount+ "���Τl");
        for (int i = 0; i < gameCore.uiCore.nowLoadingCaneUnit.loadingCane.warmCount; i++)
        {
            GenerateButton();
        }
    }

    public void GenerateButton()
    {
        if (warmPrefab == null || warmReferenceSpace == null)
        {
            Debug.LogError("�Цb Inspector ���w warmPrefab �M warmReferenceSpace�I");
            return;
        }

        // �w�]�����I�A�i�ھڻݭn�אּ�ƹ���m�Ψ�L���I
        Vector2 basePosition = Vector2.zero;

        // �H��������m
        float randomX = Random.Range(basePosition.x - 60f, basePosition.x + 180f);
        float randomY = Random.Range(basePosition.y - 290f, basePosition.y + 150f);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        // ���� Button ���
        GameObject newButtonObj = Instantiate(warmPrefab, warmReferenceSpace);

        // �]�w RectTransform �ݩ�
        RectTransform rectTransform = newButtonObj.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        //rectTransform.sizeDelta = new Vector2(1f, 1f);
        rectTransform.anchoredPosition = randomPosition;
    }

    public void cleanAllWarm()
    {
        Debug.Log("�M���ù��W�Ҧ��Τl");

        //destroy�Ҧ��l����
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

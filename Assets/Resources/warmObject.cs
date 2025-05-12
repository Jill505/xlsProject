using UnityEngine;

public class warmObject : MonoBehaviour
{
    public WarmManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = GameObject.Find("GameCore").GetComponent<WarmManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void beingClick()
    {
        //Call Manager
        manager.gameCore.uiCore.nowLoadingCaneUnit.loadingCane.warmCount -= 1;

        //destoryItself
        Destroy(gameObject);
    }
}

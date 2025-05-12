using UnityEngine;

public class CaneFunction : MonoBehaviour
{
    public GameObject caneCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseCaneCanvas()
    {
        GameObject.Find("GameCore").GetComponent<WarmManager>().cleanAllWarm();
        caneCanvas.SetActive(false);
    }
}

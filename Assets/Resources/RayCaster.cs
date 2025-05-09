using UnityEngine;

public class RayCaster : MonoBehaviour
{
    public UiCore uiCore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiCore = GameObject.Find("GameCore").GetComponent<UiCore>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.transform.gameObject.tag  == "FarmLand")
                {
                    // execute
                    Debug.Log("Triggered");
                    //call畫面 傳入該FarmLand的position/blablabla
                    //open canvas by cane data
                    uiCore.openCaneCanvas(hit.transform.gameObject.GetComponent<CaneUnit>());
                }

            }

        }
    }


}
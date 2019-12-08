using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class drawCircle : MonoBehaviour
{
    public Image spellbook;
    public Button circle;
    public float timer;
    public TrailRenderer trail;
    public GameObject cubeCollider;
    private void Start()
    {
        circle.onClick.AddListener(mouseDraw);
    }
    private void Update()
    {

        mouseDraw();
    }
    
    public void mouseDraw()
    {
        if(Input.GetMouseButton(0))
        {
            var change = FindObjectOfType<Camera>().ScreenToViewportPoint(Input.mousePosition);
            circle.GetComponent<RectTransform>().pivot = new Vector3(change.x, change.y, circle.transform.position.z);
            //cubeCollider.transform.position =  new Vector3(change.x, change.y, cubeCollider.transform.position.z);


            timer += 1* Time.deltaTime;
            for (int i = 0; i <300; i++)
            {
                if (timer >= 2)
                {
                    Debug.Log("big ass spell happening");
                    
                    
                    timer = 0;
                }
            }
            
            
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player;
    public int heightOffset;
    public int sideOffset;
    public int forawrdOffset;
    public float delay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 newCameraPosition = new Vector3(player.transform.position.x + sideOffset, heightOffset, player.transform.position.z + forawrdOffset);

        transform.position = Vector3.Lerp(transform.position, newCameraPosition, delay);
        
        
    }
}

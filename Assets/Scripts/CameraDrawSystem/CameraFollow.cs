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
        transform.position = new Vector3(player.transform.position.x + sideOffset, (player.transform.position.y + 1) + heightOffset, player.transform.position.z + forawrdOffset);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 newCameraPosition = new Vector3((player.transform.position.x -14+25) + sideOffset, (player.transform.position.y+ 20) + heightOffset, (player.transform.position.z+1-12) + forawrdOffset);

        transform.position = Vector3.Lerp(transform.position, newCameraPosition, delay);
    }

    public void ResetCamera()
    {
        transform.position = new Vector3((player.transform.position.x - 14 + 25) + sideOffset, (player.transform.position.y + 20) + heightOffset, (player.transform.position.z + 1 - 12) + forawrdOffset);
    }
}

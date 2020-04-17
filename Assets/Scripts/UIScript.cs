using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{

    PlayerMovement player;
    public Transform healthBar;

    float healthPercent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        healthPercent = (player.health / player.startingHealth);

        healthBar.localScale = new Vector3(healthPercent, 1f);

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public ParticleSystem currentEffect;
    public float destroyTimer;

    private void Start()
    {
        currentEffect = gameObject.GetComponentInChildren<ParticleSystem>();
        destroyTimer = currentEffect.duration;
    }
    private void Update()
    {

        destroyTimer -= 0.1f;
        if(destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}

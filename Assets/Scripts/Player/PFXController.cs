using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class PFXController : MonoBehaviour
{

    private ParticleSystem PFX;

    void Start()
    {
        PFX = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!PFX.isPlaying) Destroy(gameObject);
    }


}

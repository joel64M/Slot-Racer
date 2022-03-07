using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSound : MonoBehaviour
{

    [SerializeField] AudioClip ac;
    private void OnEnable()
    {
        if (GetComponentInParent<AudioSource>())
        {
            if(ac!=null)
            GetComponentInParent<AudioSource>().clip = ac;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float rotSpeed = 5f;
    void Start()
    {
        LeanTween.rotateAround(gameObject, Vector3.up, 360, rotSpeed).setLoopClamp();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyMovement : MonoBehaviour
{
    [SerializeField]public float speed = 0.25f; 
    [SerializeField]private bool isActivated = false;

    void Update()
    {
        if (isActivated)
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        }
    }

    void OnBecameVisible()
    {
        isActivated = true;
    }
}
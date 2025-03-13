using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGuyMovement : MonoBehaviour
{
    [SerializeField]public float speed = 0.25f; 
    [SerializeField]private bool isActivated = false;
    public Transform player; 

    void Update()
    {
        if (isActivated)
        {
          
            if (IsPlayerInCameraView())
            {
                Vector3 direction = (player.position - transform.position).normalized; 
                transform.position += direction * speed * Time.deltaTime; 
            }
        }
    }

    void OnBecameVisible()
    {
        isActivated = true;
    }

    private bool IsPlayerInCameraView()
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(player.position);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }
}
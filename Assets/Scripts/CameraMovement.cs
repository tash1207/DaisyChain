using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    Vector3 initialPosition = new Vector3(0, 0, -10);
    Vector3 downPosition = new Vector3(0, -6, -10);

    void Update()
    {
        if (playerTransform.position.y < -3)
        {
            transform.position = downPosition;
        }
        else
        {
            transform.position = initialPosition;
        }
    }
}

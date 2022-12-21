using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerBoxCollider : MonoBehaviour
{
    public static event Action<GameObject> OnCameraCollision;
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("there was a collision with the marker!");
        if (collider.gameObject.tag == "MainCamera")
        {
            Debug.Log("collision with camera");
            OnCameraCollision?.Invoke(gameObject);
        }
    }
}

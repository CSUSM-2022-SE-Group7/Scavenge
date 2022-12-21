using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private Text distanceText;
    [SerializeField] private Transform player;

    private GameObject _marker;

    private void Start()
    {
        ARController.OnActiveMarker += SetMarker;
        
        InvokeRepeating("UpdateText", 0, 1);
    }

   

    private void UpdateText()
    {
        if (_marker == null) return;
        
        Vector3 pointA = _marker.transform.position;
        Vector3 pointB = player.transform.position;
        
        var distance = Distance3d(pointA, pointB);

        Debug.Log("distance=" + distance);
        distanceText.text = "distance: " + distance;
    }
    

    private static double Distance3d(Vector3 pointA, Vector3 pointB)
    {
        var x = Math.Pow((pointA.x - pointB.x), 2);
        var y = Math.Pow((pointA.y - pointB.y), 2);
        var z = Math.Pow((pointA.z - pointB.z), 2);

        return Math.Sqrt(x + y + z);
    }

    private void SetMarker(GameObject activeMarker)
    {
        _marker = activeMarker;
    }
}

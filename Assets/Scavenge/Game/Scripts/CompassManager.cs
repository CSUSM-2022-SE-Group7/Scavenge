using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassManager : MonoBehaviour
{
    public RectTransform Compass_ScriptTransform;

    public GameObject sourceMarker;
    public GameObject bar;
    public Transform cameraObjectTransform;

    private List<GameObject> anchors = new List<GameObject>();
    private List<GameObject> markers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < anchors.Count; i++)
        {
            if (!anchors[i].activeSelf)
            {
                SetMarkerPosition((RectTransform)markers[i].transform, anchors[i].transform.position);
            }
            else if (markers[i].activeSelf)
            {
                markers[i].SetActive(false);
            }
        }
    }

    public void SetAnchorList(List<GameObject> anchors)
    {
        this.anchors = anchors;
        for (int i = 0; i < anchors.Count; i++)
        {
            GameObject obj = Object.Instantiate(sourceMarker, bar.transform);
            obj.SetActive(true);
            markers.Add(obj);
        }
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition){
        Vector3 directionToTarget = worldPosition - cameraObjectTransform.position;
        float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(cameraObjectTransform.transform.forward.x, cameraObjectTransform.transform.forward.z));
        float compassPositionX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
        if (compassPositionX == -1 || compassPositionX == 1)
        {
            markerTransform.gameObject.SetActive(false);
        } 
        else
        {
            markerTransform.gameObject.SetActive(true);         
        }

        float mappedPosition = Compass_ScriptTransform.rect.width / 2 * compassPositionX;
        markerTransform.anchoredPosition = new Vector2(mappedPosition, 0);
    }
}

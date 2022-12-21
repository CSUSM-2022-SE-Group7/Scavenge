using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using LocalV2;
using LocalVersion.Game;
using Qualcomm.Snapdragon.Spaces.Samples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ARController : SampleController
{
    [SerializeField] private GameObject markerPlacementIndicatorPrefab;
    [SerializeField] private GameObject markerPrefab;
    [SerializeField] private InputActionReference TriggerAction;
    
    private float _placementDistance = 1f;
    private Transform _cameraTransform;
    private GameObject _markerPlacementIndicator;
    private List<GameObject> _sessionMarkers = new List<GameObject>();
    
    private bool _canPlaceMarkers;
    private bool _canRemoveMarkers;
    
    public static event Action<GameObject> OnActiveMarker;
    public static event Action OnMarkerFound;
    public static event Action<int> OnMarkerPlaced;
    
    public void Awake()
    {
        _cameraTransform = Camera.main.transform;
        _markerPlacementIndicator = Instantiate(
            markerPlacementIndicatorPrefab,
            transform,
            true);
    }

    public override void Update()
    {
        base.Update();

        if (GazePointer.activeSelf)
        {
            _markerPlacementIndicator.SetActive(true);
            _markerPlacementIndicator.transform.position = _cameraTransform.position +
                                                          _cameraTransform.forward * _placementDistance;
        }
        else
        {
            if (_markerPlacementIndicator.activeSelf)
            {
                _markerPlacementIndicator.SetActive(false);
            }
        }
    }
    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LateOnEnable());
    }

    private IEnumerator LateOnEnable()
    {
        yield return new WaitForSeconds(0.1f);
        TriggerAction.action.performed += OnTriggerAction;
        CustomGazeInteractor.OnMarkerHit += RemoveMarker;
        SeekerBoxCollider.OnCameraCollision += ShowMarker;
    }

    public override void OnDisable()
    {
        TriggerAction.action.performed -= OnTriggerAction;
        CustomGazeInteractor.OnMarkerHit -= RemoveMarker;
        SeekerBoxCollider.OnCameraCollision -= ShowMarker;
    }

    private void OnTriggerAction(InputAction.CallbackContext context)
    {
        if (_canPlaceMarkers)
        {
            Debug.Log("Calling InstantiateMarker");
            InstantiateMarker();
        }
    }

    public void InstantiateMarker()
    {
        var targetPosition = _markerPlacementIndicator.transform;
        var sessionMarker = Instantiate(markerPrefab, targetPosition.position, Quaternion.identity);

        Quaternion desiredRotation = new Quaternion(90, 0, 0, 0);
        
        sessionMarker.transform.Rotate(new Vector3(desiredRotation.x, desiredRotation.y, desiredRotation.z));
        
        _sessionMarkers.Add(sessionMarker);

        if (_sessionMarkers.Count == 1)
        {
            Debug.Log("The first marker was placed. This is the active marker");
            OnActiveMarker?.Invoke(sessionMarker);
        }
        
        OnMarkerPlaced?.Invoke(_sessionMarkers.Count);
        
        
    }

    public void RemoveAllMarkers()
    {
        foreach (var treasure in _sessionMarkers)
        {
            Destroy(treasure);
        }
        _sessionMarkers.Clear();
        OnActiveMarker?.Invoke(null);
    }
    private void RemoveMarker(RaycastHit hit)
    {
        if (hit.transform.gameObject == _markerPlacementIndicator || !_canRemoveMarkers) return;
        Debug.Log("Destroying a marker!");

        Debug.Log("_sessionGizmos count before removal:" + _sessionMarkers.Count);
        if (hit.transform.parent.gameObject == _sessionMarkers[0])
        {
            Debug.Log("The active marker was destroyed. set next marker to be active marker");

            OnActiveMarker?.Invoke(_sessionMarkers.Count == 1 ? null : _sessionMarkers[1]);
        }

        bool removed = _sessionMarkers.Remove(hit.transform.parent.gameObject);
        Debug.Log("_sessionGizmos count after removal:" + _sessionMarkers.Count);

        if (removed)
        {
            Debug.Log("gizmo was removed from list");
        }
        
        Destroy(hit.transform.parent.gameObject);

        if (StateManager.CurrentState() == SeekerState.SEEKER)
        {
            OnMarkerFound?.Invoke();
        }
    }
    public void HideAllMarkers()
    {
        foreach (var marker in _sessionMarkers)
        {
            MarkerPrefabScript markerPrefabScript = marker.GetComponent<MarkerPrefabScript>();
            markerPrefabScript.HidePrefab();
            markerPrefabScript.ActivateOuterBoxCollider();
        }
    }
    public void ShowAllMarkers()
    {
        foreach (var marker in _sessionMarkers)
        {
            MarkerPrefabScript markerPrefabScript = marker.GetComponent<MarkerPrefabScript>();
            markerPrefabScript.ShowPrefab();
            markerPrefabScript.DeactivateOuterBoxCollider();
        }
    }
    public void ShowMarker(GameObject marker)
    {
        MarkerPrefabScript markerPrefabScript = marker.transform.parent.GetComponent<MarkerPrefabScript>();
        markerPrefabScript.ShowPrefab();
        markerPrefabScript.DeactivateOuterBoxCollider();
    }
    public void HidePlacementIndicator()
    {
        Debug.Log("Hiding placement indicator.");
        _markerPlacementIndicator.SetActive(false);
    }
    public void ShowPlacementIndicator()
    {
        Debug.Log("Showing placement indicator.");
        _markerPlacementIndicator.SetActive(true);
    }
    public void CanPlaceMarkers(bool b)
    {
        _canPlaceMarkers = b;
    }
    public void CanRemoveMarkers(bool b)
    {
        _canRemoveMarkers = b;
    }

}

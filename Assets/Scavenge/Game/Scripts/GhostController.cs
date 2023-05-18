using UnityEngine;
using UnityEngine.XR.ARFoundation;

using Qualcomm.Snapdragon.Spaces.Samples;
using System;

public class GhostController : MonoBehaviour
{
    public float timeLimit = 15.0f;
    public float ghostSpawnDistance = 20.0f;
    public GameObject ghost;
    public GameObject ghostMeshed;
    public float cooldownTime = 15.0f;
    public float bounceSpeed = 0.005f;

    private float timeElapsed;
    private Transform _cameraTransform;
    private float currentDistance;
    private GazeInteractor gazeInteractor;
    private RandomGlitchEmitter glitchEmitter;
    private Vector3 lastCameraPosition;
    private bool ghostCooldown;
    private float ghostCooldownElapsed;
    private MeshRenderer _renderer;
    private int bounce = 0;

    public float CurrentDistance {
        get => currentDistance; 
        private set => currentDistance = value; 
    }
    public event Action TimeIsUp;
    public event Action GhostCaught;

    void Start()
    {
        ghost.SetActive(false);
        ARSessionOrigin _arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
        _cameraTransform = _arSessionOrigin.camera.transform;

        _renderer = ghostMeshed.GetComponent<MeshRenderer>();

        gazeInteractor = FindObjectOfType<GazeInteractor>();
        glitchEmitter = FindObjectOfType<RandomGlitchEmitter>();
        ghostCooldownElapsed = 0;
        timeElapsed = 0;
    }

    public void SpawnGhost()
    {
        timeElapsed = 0;
        lastCameraPosition = _cameraTransform.position;
        ghost.transform.position = GetGhostSpawn(this.ghostSpawnDistance);
        ghost.SetActive(true);
    }

    public float GetTimeLeft()
    {
        return this.timeLimit - this.timeElapsed - 0.9f; // 0.9 is our magic for the ghost distance calc
    }

    private void StopTimer()
    {
        ghost.SetActive(false);
        glitchEmitter.FuzzScreen(10.0f);

        if (TimeIsUp != null)
        {
            TimeIsUp();
        }
    }

    private Vector3 GetGhostSpawn(float distance)
    {
        float theta = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
        float x = Mathf.Sin(theta) * distance;
        float z = Mathf.Cos(theta) * distance;
        return new Vector3(x + _cameraTransform.position.x, _cameraTransform.position.y, z + _cameraTransform.position.z);
    }

    // Called when the user looks at the ghost
    void OnGhostHit()
    {
        ghost.transform.position = GetGhostSpawn(this.currentDistance);
        _renderer.forceRenderingOff = true;
        ghostCooldown = true;
        GhostCaught();
    }

    void Update()
    {
        if (ghostCooldown)
        {
            ghostCooldownElapsed += Time.deltaTime;
            if (ghostCooldownElapsed > cooldownTime)
            {
                _renderer.forceRenderingOff = false;
                ghostCooldown = false;
                ghostCooldownElapsed = 0;
            }
            return;
        }


        // Keep ghost distance relative to camera. As you approach the ghost, it shall move away at the same rate.
        ghost.transform.position += _cameraTransform.position - lastCameraPosition;

        // Calculate the travel distance needed for this game frame to reach our target at desired timeLimit
        float moveStep = (ghostSpawnDistance / this.timeLimit) * Time.deltaTime;
        ghost.transform.position = Vector3.MoveTowards(ghost.transform.position, _cameraTransform.position, moveStep);
        ghost.transform.LookAt(_cameraTransform.position);

        currentDistance = Vector3.Distance(ghost.transform.position, _cameraTransform.position);
        if (currentDistance < 0.2)
        {
            StopTimer();
        }

        // send a ray cast at the ghost's mesh
        RaycastHit hitinfo;
        if (Physics.Raycast(_cameraTransform.position, gazeInteractor.transform.forward, out hitinfo, this.currentDistance + 0.5f))
        {
            if (hitinfo.collider.gameObject == this.ghostMeshed)
            {
                this.OnGhostHit();
            }
        }

        // Ghost bouncing
        if (bounce > 400)
        {
            bounceSpeed *= -1;
        }
        else if (bounce < -400)
        {
            bounceSpeed *= -1;
        }

        if (!float.IsNegative(bounceSpeed))
        {
            bounce++;
        }
        else
        {
            bounce--;
        }

        Vector3 newPosition = new Vector3(ghost.transform.position.x, ghost.transform.position.y, ghost.transform.position.z);
        newPosition.y += bounceSpeed;
        ghost.transform.position = newPosition;
        lastCameraPosition = _cameraTransform.position;
        timeElapsed += Time.deltaTime;
    }
}




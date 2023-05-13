using Kino;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Haunt
{
    public class SeekerState : IGameState
    {
        private AnalogGlitch _vfxGlitch;
        private GhostController _ghost;

        private GameController controller;
        private Transform _cameraTransform;
        private GameObject closestAnchor;
        private float closestAnchorDistance;
        private Text timerText;
        private Text soulText;
        private int countLeft;
        private int totalSouls;
        private float compassTimeLeft = 0;

        public SeekerState(GameController controller)
        {
            this.controller = controller;
        }

        public void Start()
        {
            this._ghost.GhostCaught += OnGhostHit;
            this._vfxGlitch.enabled = true;
            _ghost.SpawnGhost();
            _cameraTransform = controller.GetCameraTransform();

            GameObject countObject = controller.GetCountObject();
            countObject.SetActive(true);
            this.soulText = countObject.GetComponent<Text>();

            CompassManager cm = Object.FindObjectOfType<CompassManager>();
            List<GameObject> anchors = this.controller.GetAnchors();
            totalSouls = countLeft = anchors.Count;
            cm.SetAnchorList(anchors);

            GameObject timerObject = controller.GetTimerObject();
            timerObject.SetActive(true);
            timerText = timerObject.GetComponentInChildren<Text>();

            Text title = GameObject.FindGameObjectWithTag("GameTextTitle").GetComponent<Text>();
            title.text = "Seeker Mode";
            Text body = GameObject.FindGameObjectWithTag("GameTextBody").GetComponent<Text>();
            body.text = "Search for nearby souls, carefully...";

            GameObject followPanel = controller.GetFollowPanel();
            followPanel.SetActive(false);

            _ghost.TimeIsUp += GameOver;
        }

        public void GameOver()
        {
            GameResults results = new GameResults();
            results.soulCount = totalSouls - countLeft;
            results.win = false;
            results.time = _ghost.GetTimeLeft();
            controller.OnGameEnd(results);
        }

        public void Awake()
        {
            _ghost = Object.FindObjectOfType<GhostController>();
            _vfxGlitch = Camera.main.GetComponent<AnalogGlitch>();
        }

        public void OnTriggerAction(InputAction.CallbackContext ctx)
        {
            throw new System.NotImplementedException();
        }

        public void OnSoulFound(GameObject anchor)
        {
            AudioSource audio = controller.GetSoulFoundSource();
            audio.Play();
            countLeft--;
            anchor.SetActive(true);
            if (countLeft == 0)
            {
                GameResults results = new GameResults();
                results.soulCount = totalSouls - countLeft;
                results.win = true;
                results.time = _ghost.GetTimeLeft();
                controller.OnGameEnd(results);
            }
        }

        public void OnGhostHit()
        {
            GameObject compass = controller.GetCompassObject();
            compass.SetActive(true);
            compassTimeLeft = this.controller.CompassTime;
        }

        public void Update()
        {
            if (compassTimeLeft > 0)
            {
                if (compassTimeLeft - Time.deltaTime > 0)
                {
                    compassTimeLeft -= Time.deltaTime;
                }
                else
                {
                    GameObject compass = controller.GetCompassObject();
                    compass.SetActive(false);
                    compassTimeLeft = 0;
                }
            }

            if (closestAnchor != null)
            {
                if (closestAnchorDistance < 1.5)
                {
                    if (!closestAnchor.activeSelf)
                    {
                        OnSoulFound(closestAnchor);
                    }
                }
            }

            CalculateDistancesAndDistort();
            timerText.text = string.Format("{0:0.0}s", _ghost.GetTimeLeft());
            soulText.text = string.Format("{0}/{1} remain", countLeft, totalSouls);

            if (this.closestAnchor)
            {
                Vector3 direction = this.closestAnchor.transform.position - this._cameraTransform.position;
                Vector2 dir = new Vector2(direction.x, direction.z);
                dir.Normalize();

                Vector3 cameraDirection = this._cameraTransform.forward;
                Vector2 cameraDir = new Vector2(cameraDirection.x, cameraDirection.z);
                cameraDir.Normalize();

                float dot = Vector2.Dot(dir, cameraDir);
                if (dot >= 0.90f)
                {
                    float amplitude = ConvertRange(0.0f, 0.1f, 0.0f, 0.30f, dot - 90f);
                    Qualcomm.Snapdragon.Spaces.Samples.XRControllerManager manager = Object.FindObjectOfType<Qualcomm.Snapdragon.Spaces.Samples.XRControllerManager>();
                    manager.SendHapticImpulse(amplitude, 60, Time.deltaTime);
                }
            }
        }

        public void CalculateDistancesAndDistort()
        {
            if (!_vfxGlitch)
            {
                return;
            }

            float closest_distance = float.MaxValue;
            List<GameObject> anchors = controller.GetAnchors();
            foreach (GameObject anchor in anchors)
            {
                float distance = Vector3.Distance(this._cameraTransform.position, anchor.transform.position);
                if (distance < closest_distance)
                {
                    closestAnchor = anchor;
                    closest_distance = distance;
                }
            }
            closestAnchorDistance = closest_distance;

            if (closest_distance > 6f)
            {
                _vfxGlitch.colorDrift = 0;
            }
            else
            {
                _vfxGlitch.colorDrift = 0.1f * (1.0f - ConvertRange(0, 6, 0, 1, closest_distance));
            }
        }

        public float ConvertRange(float originalStart, float originalEnd, float newStart, float newEnd, float value)
        {
            float originalDiff = originalEnd - originalStart;
            float newDiff = newEnd - newStart;
            float ratio = newDiff / originalDiff;
            float newProduct = value * ratio;
            float finalValue = newProduct + newStart;
            return finalValue;
        }

        public void End()
        {
            this._vfxGlitch.enabled = false;
            this.timerText.gameObject.SetActive(false);
            this.soulText.gameObject.SetActive(false);
            this.controller.compassObject.SetActive(false);
            this._ghost.gameObject.SetActive(false);
        }
    }

}
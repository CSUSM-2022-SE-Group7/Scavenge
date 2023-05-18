using Qualcomm.Snapdragon.Spaces.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

namespace Haunt
{
    public class HiderState : IGameState
    {
        private GazeInteractor _gazeInteractor;
        private bool _placeAnchorAtRaycastHit;
        private bool _canPlaceAnchorGizmos = true;

        private Transform _cameraTransform;

        private GameObject _indicatorGizmo;
        private GameObject _transparentGizmo;
        private GameObject _surfaceGizmo;

        private GameController controller;
        public HiderState(GameController controller)
        {
            this.controller = controller;
        }

        public void Start()
        {
            //tooltipManager.SetText("Carefully walk around your surroundings, hide souls as best as possible.\n"
            //+"When completed, return here and select \"Finish\"...");
            //tooltipManager.Send(9.0f);

            _cameraTransform = controller.GetCameraTransform();
            _indicatorGizmo = new GameObject("IndicatorGizmo");
            _transparentGizmo = Object.Instantiate(controller.TransparentSoul, _indicatorGizmo.transform.position, Quaternion.identity, _indicatorGizmo.transform);
            _surfaceGizmo = Object.Instantiate(controller.PlacedSoul, _indicatorGizmo.transform.position, Quaternion.identity, _indicatorGizmo.transform);
            _surfaceGizmo.SetActive(false);
        }

        public void Awake()
        {
            _gazeInteractor = controller.GazePointer.GetComponent<GazeInteractor>();
        }

        public void OnTriggerAction(InputAction.CallbackContext ctx)
        {
            if (!_canPlaceAnchorGizmos)
                return;

            CreateSoul();
        }

        public void CreateSoul()
        {
            var targetPosition = _indicatorGizmo.transform.position;

            List<GameObject> anchors = controller.GetAnchors();
            foreach (GameObject anchor in anchors)
            {
                if (Vector3.Distance(targetPosition, anchor.transform.position) < 5.0f)
                {
                    TooltipManager ttman = controller.GetTooltipManager();
                    ttman.SetText("You may not place a soul too close to another!");
                    ttman.Send(4.0f);
                    return;
                }
            }

            var soul = Object.Instantiate(controller.PlacedSoul, targetPosition, Quaternion.identity);
            anchors.Add(soul);
        }

        public void Update()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
            if (controller._raycastManager.Raycast(ray, hitResults))
            {
                _placeAnchorAtRaycastHit = !controller.RestrictRaycastDistance ||
                                           (hitResults[0].pose.position - _cameraTransform.position).magnitude <
                                           controller.PlacementDistance;
            }
            else
            {
                _placeAnchorAtRaycastHit = false;
            }

            if (_placeAnchorAtRaycastHit)
            {
                if (!_surfaceGizmo.activeSelf)
                {
                    _surfaceGizmo.SetActive(true);
                    _transparentGizmo.SetActive(false);
                }
                _indicatorGizmo.transform.position = hitResults[0].pose.position;
            }
            else
            {
                if (_surfaceGizmo.activeSelf)
                {
                    _surfaceGizmo.SetActive(false);
                    _transparentGizmo.SetActive(true);
                }
                _indicatorGizmo.transform.position = _cameraTransform.position + _cameraTransform.forward * controller.PlacementDistance;
            }

            if (controller.GazePointer.activeSelf)
            {
                _canPlaceAnchorGizmos = !_gazeInteractor.IsHovering;
            }
        }

        public void End()
        {
            List<GameObject> anchors = controller.GetAnchors();
            foreach (GameObject anchor in anchors)
            {
                anchor.SetActive(false);
            }

            _transparentGizmo.SetActive(false);
            _surfaceGizmo.SetActive(false);
            _indicatorGizmo.SetActive(false);
        }
    }
}
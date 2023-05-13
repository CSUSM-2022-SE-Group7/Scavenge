/******************************************************************************
 * File: AnchorSampleController.cs
 * Copyright (c) 2022 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 * Confidential and Proprietary - Qualcomm Technologies, Inc.
 *
 ******************************************************************************/

using Kino;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using Qualcomm.Snapdragon.Spaces.Samples;

namespace Scavenge
{
    public class TutorialController : SampleController
    {
        public InputActionReference TriggerAction;
        private GazeInteractor _gazeInteractor;
        private Transform _cameraTransform;

        // Panel Switching
        public GameObject[] panels;
        private int currentPanel = 0;

        public override void Start() {
            base.Start();
            _cameraTransform = Camera.main.transform;
            _gazeInteractor = GazePointer.GetComponent<GazeInteractor>();
        }

        public override void OnEnable() {
            base.OnEnable();
            StartCoroutine(LateOnEnable());
        }

        public void OnClick()
        {
            panels[currentPanel].SetActive(false);
            panels[++currentPanel].SetActive(true);
            
            // Panel specific code here
            if (currentPanel == 1)
            {
                // make ghost active or something
            }
        }

        private IEnumerator LateOnEnable() {
            yield return new WaitForSeconds(0.1f);
            TriggerAction.action.performed += OnTriggerAction;
        }

        public override void OnDisable() {
            base.OnDisable();
            TriggerAction.action.performed -= OnTriggerAction;
        }

        private void OnTriggerAction(InputAction.CallbackContext context) {

        }
        public override void Update() {
            base.Update();
        }
    }
}
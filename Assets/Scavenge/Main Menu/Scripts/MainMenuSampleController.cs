/******************************************************************************
 * File: MainMenuSampleController.cs
 * Copyright (c) 2022 Qualcomm Technologies, Inc. and/or its subsidiaries. All rights reserved.
 *
 * Confidential and Proprietary - Qualcomm Technologies, Inc.
 *
 ******************************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.OpenXR;
using Qualcomm.Snapdragon.Spaces.Samples;
using Qualcomm.Snapdragon.Spaces;
using System;
using System.Collections;

namespace Scavenge
{
    public class MainMenuSampleController : SampleController
    {
        public GameObject ContentGameObject;
        public GameObject ComponentVersionsGameObject;
        public Transform ComponentVersionContent;
        public GameObject ComponentVersionPrefab;

        public ScrollRect ScrollRect;
        public Scrollbar VerticalScrollbar;
        public GameObject GazeScrollButtons;
        public InputActionReference TouchpadInputAction;

        public GameObject ghostGameObject;
        public float FlickerTime = 7.0f;
        private float currentTime = 0.0f;
        private bool doingFlicker = false;
        private System.Random rand = new System.Random();
        private Coroutine currentCoRoutine;

        public override void Start() {
            base.Start();

            OnInputSwitch(new InputAction.CallbackContext());

            var baseRuntimeFeature = OpenXRSettings.Instance.GetFeature<BaseRuntimeFeature>();
            if (!baseRuntimeFeature) {
                Debug.LogWarning("Base Runtime Feature isn't available.");
                return;
            }

            var componentVersions = baseRuntimeFeature.ComponentVersions;
            for (int i = 0; i < componentVersions.Count; i++) {
                var componentVersionObject = Instantiate(ComponentVersionPrefab, ComponentVersionContent);

                var componentVersionDisplay = componentVersionObject.GetComponent<ComponentVersionDisplay>();
                componentVersionDisplay.ComponentNameText = componentVersions[i].ComponentName;
                componentVersionDisplay.BuildIdentifierText = componentVersions[i].BuildIdentifier;
                componentVersionDisplay.VersionIdentifierText = componentVersions[i].VersionIdentifier;
                componentVersionDisplay.BuildDateTimeText = componentVersions[i].BuildDateTime;
            }
        }

        public override void OnEnable() {
            base.OnEnable();
            SwitchInputAction.action.performed += OnInputSwitch;
            TouchpadInputAction.action.performed += OnTouchpadInput;
        }

        public override void OnDisable() {
            base.OnDisable();
            SwitchInputAction.action.performed -= OnInputSwitch;
            TouchpadInputAction.action.performed -= OnTouchpadInput;
        }

        private void OnTouchpadInput(InputAction.CallbackContext context) {
            var touchpadValue = context.ReadValue<Vector2>();

            if (touchpadValue.y > 0f) {
                OnVerticalScrollViewChanged(0.44f);
            }
            else {
                OnVerticalScrollViewChanged(-0.44f);
            }
        }

        public void OnInfoButtonPress() {
            ContentGameObject.SetActive(!ContentGameObject.activeSelf);
            ComponentVersionsGameObject.SetActive(!ComponentVersionsGameObject.activeSelf);
        }

        public void OnVerticalScrollViewChanged(float value) {
            ScrollRect.verticalNormalizedPosition = Mathf.Clamp01(ScrollRect.verticalNormalizedPosition + value * Time.deltaTime);
            VerticalScrollbar.value = ScrollRect.verticalNormalizedPosition;
        }

        public IEnumerator PreformFlicker()
        {
            doingFlicker = true;
            int numFlickers = rand.Next(2, 5);
            int[] flickerTime = new int[numFlickers];
            for (int i = 0; i < numFlickers; i++)
            {
                flickerTime[i] = rand.Next(3, 10);
            }

            int currentFlicker = 0;
            int oldFlickerTime;
            while (currentFlicker < numFlickers)
            {
                ghostGameObject.SetActive(false);
                oldFlickerTime = flickerTime[currentFlicker];

                while (oldFlickerTime > 0)
                {
                    oldFlickerTime--;
                    yield return null;
                }
                ghostGameObject.SetActive(true);

                while (flickerTime[currentFlicker] > 0)
                {
                    flickerTime[currentFlicker]--;
                    yield return null;
                }
                currentFlicker++;
            }
            doingFlicker = false;
            currentTime = 0.0f;
        }

        public override void Update()
        {
            if (!doingFlicker)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= FlickerTime)
                {
                   StartCoroutine(PreformFlicker());
                }
            }
        }

        private void OnInputSwitch(InputAction.CallbackContext ctx) {
            if (GazePointer.activeSelf) {
                ScrollRect.verticalScrollbar = null;
                VerticalScrollbar.gameObject.SetActive(false);

                GazeScrollButtons.SetActive(true);
            }
            else if (DevicePointer.activeSelf) {
                ScrollRect.verticalScrollbar = VerticalScrollbar;
                VerticalScrollbar.gameObject.SetActive(true);

                GazeScrollButtons.SetActive(false);
            }
        }
    }
}
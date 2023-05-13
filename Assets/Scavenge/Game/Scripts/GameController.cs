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

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using Qualcomm.Snapdragon.Spaces.Samples;
using System;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace Haunt
{

    public class GameController : SampleController
    {
        public enum GameState
        {
            HIDER,
            SEEKER,
            END
        }

        //Configurable variables (unity)
        public GameObject TransparentSoul;
        public GameObject PlacedSoul;

        public float PlacementDistance = 1f;
        public bool RestrictRaycastDistance = false;
        public float CompassTime = 7.5f;

        // UI & Events
        public GameObject followPanel;
        public GameObject timerObject;
        public GameObject countObject;
        public GameObject compassObject;
        public InputActionReference TriggerAction;
        public AudioSource soulFoundSound;

        // Managers
        public ARRaycastManager _raycastManager;
        private TooltipManager tooltipManager;

        // States
        private HiderState hiderState;
        private SeekerState seekerState;
        private EndGameState endState;
        private GameState state;
        private List<GameObject>_anchorGizmos = new();
        private GameResults result;

        private void Awake() {
            hiderState = new HiderState(this);
            hiderState.Awake();
            seekerState = new SeekerState(this);
            seekerState.Awake();
            endState = new EndGameState(this);
            endState.Awake();
            
            _raycastManager = FindObjectOfType<ARRaycastManager>();
            tooltipManager = FindObjectOfType<TooltipManager>();
        }

        public override void Start() {
            base.Start();
            hiderState.Start();
            state = GameState.HIDER;
        }

        public GameObject GetTimerObject()
        {
            return timerObject;
        }

        public GameObject GetCountObject()
        {
            return countObject;
        }
        public GameObject GetCompassObject()
        {
            return compassObject;
        }

        public AudioSource GetSoulFoundSource()
        {
            return soulFoundSound;
        }

        public GameObject GetFollowPanel()
        {
            return this.followPanel;
        }

        public List<GameObject> GetAnchors()
        {
            return _anchorGizmos;
        }

        internal TooltipManager GetTooltipManager()
        {
            return tooltipManager;
        }

        public Transform GetCameraTransform()
        {
            return this.ARCameraTransform;
        }

        public GameResults GetGameResults()
        {
            if (state != GameState.END)
            {
                throw new Exception("GetGameResults called before game is over!");
            }

            return result;
        }

        public void OnGameEnd(GameResults result) {
            this.result = result;
            seekerState.End();
            state = GameState.END;
            endState.Start();
        }

        public void OnFinishPress() {
            hiderState.End();
            seekerState.Start();
            state = GameState.SEEKER;
        }

        public override void OnEnable() {
            base.OnEnable();
            StartCoroutine(LateOnEnable());
        }

        private IEnumerator LateOnEnable() {
            yield return new WaitForSeconds(0.1f);
            TriggerAction.action.performed += OnTriggerAction;
        }

        public override void OnDisable() {
            base.OnDisable();
            TriggerAction.action.performed -= OnTriggerAction;
        }

        private void OnTriggerAction(InputAction.CallbackContext context)
        {
            switch (state)
            {
                case GameState.HIDER:
                    hiderState.OnTriggerAction(context);
                    break;
                case GameState.SEEKER:
                    seekerState.OnTriggerAction(context);
                    break;
            }
        }

        public override void Update() {
            base.Update();

            switch (state)
            {
                case GameState.HIDER:
                    hiderState.Update();
                    break;
                case GameState.SEEKER:
                    seekerState.Update();
                    break;
                case GameState.END:
                    endState.Update();
                    break;
            }
        }

        protected override bool CheckSubsystem() {
#if UNITY_ANDROID && !UNITY_EDITOR
            var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            var runtimeChecker = new AndroidJavaClass("com.qualcomm.snapdragon.spaces.unityserviceshelper.RuntimeChecker");

            if ( !runtimeChecker.CallStatic<bool>("CheckCameraPermissions", new object[] { activity }) ) {
                Debug.LogError("Snapdragon Spaces Services has no camera permissions!");
                return false;
            }
            return (_raycastManager.subsystem?.running ?? false);
#else
            return true;
#endif
        }
    }
}
using System;
using DefaultNamespace;
using LocalVersion;
using UnityEngine;
using UnityEngine.Events;

namespace LocalV2
{
    public class MasterController : MonoBehaviour, IMasterController
    {
        private ARController _arController;
        private MenuController _menuController;

        private readonly StateManager _stateManager = new StateManager();

        public UnityEvent<bool> onCanRemoveMarkers;
        void Start()
        {
            _arController = FindObjectOfType<ARController>();
            _menuController = FindObjectOfType<MenuController>();
            _stateManager.ChangeState(new MainMenuState(this));
        }
        void Update()
        {
            _stateManager.Update();
        }
        public void ShowAllMarkers()
        {
            _arController.ShowAllMarkers();
        }
        public void HideAllMarkers()
        {
            _arController.HideAllMarkers();
        }
        public void HidePlacementIndicator()
        {
            _arController.HidePlacementIndicator();
        }
        public void ShowPlacementIndicator()
        {
            _arController.ShowPlacementIndicator();
        }
        public void CanPlaceMarkers(bool b)
        {
            _arController.CanPlaceMarkers(b);
        }
        public void CanRemoveMarkers(bool canRemoveMarker)
        {
            onCanRemoveMarkers?.Invoke(canRemoveMarker);
        }
        public void ShowMainMenu()
        {
            _menuController.ShowMainMenu();
        }
        public void JoinHuntButtonPressed()
        {
            _arController.ShowAllMarkers();
            _menuController.ShowSeekerMenu();
            _stateManager.ChangeState(new SeekerState(this));
        }
        public void StartNewHuntButtonPressed()
        {
            _arController.RemoveAllMarkers();
            _menuController.ShowHiderMenu();
            _stateManager.ChangeState(new HiderState(this));
        }
        public void PlaceMarkerButtonPressed()
        {
            Debug.Log("Place Marker Button pressed");
            _arController.CanPlaceMarkers(true);
            onCanRemoveMarkers?.Invoke(false);

        }

        public void RemoveMarkerButtonPressed()
        {
            Debug.Log("Remove Marker Button pressed");
            onCanRemoveMarkers?.Invoke(true);
            _arController.CanPlaceMarkers(false);
        }

        public void HiderDoneHiding_ButtonPressed()
        {
            Debug.Log("Hider finished hiding marker. Switching to Seeker state");
            _menuController.ShowSeekerMenu();
            _stateManager.ChangeState(new SeekerState(this));
        }

        public void SeekerDoneSeeking_ButtonPressed()
        {
            Debug.Log("Seeker is done searching for markers.");
            _menuController.ShowMainMenu();
            _stateManager.ChangeState(new MainMenuState(this));
        }
        
    }
}
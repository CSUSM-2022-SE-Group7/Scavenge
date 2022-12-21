using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    /**
     * Controls what the button text says
     * and handles the deactivation of the buttons. 
     */
    public class HiderMenu: MonoBehaviour
    {
        [SerializeField] private Button placeMarkerButton;
        [SerializeField] private Button removeMarkerButton;

        private Text _placeMarkerButtonText;
        private Text _removeMarkerButtonText;
        
        private bool _isPlaceOn;
        private bool _isRemoveOn;

        private bool _pointerHoveringOverButton;

        
        private void Start()
        {
            _placeMarkerButtonText = placeMarkerButton.GetComponentInChildren<Text>();
            _removeMarkerButtonText = removeMarkerButton.GetComponentInChildren<Text>();
        }

        public void PlaceMarkerButtonPressed()
        {
            if (_pointerHoveringOverButton)
            {
                Debug.Log("pointer hovering over button. do not run the function.");
            }
            else
            {
                Debug.Log("pointer is Not hovering over button. Run the function.");

                _isPlaceOn = !_isPlaceOn;
            
                if (_isPlaceOn)
                {
                    Debug.Log("Ready to place markers.");
                    _placeMarkerButtonText.text = "Stop";
                    removeMarkerButton.interactable = false;
                }
                else
                {
                    Debug.Log("Stopped placing markers.");
                    _placeMarkerButtonText.text = "Place";
                    removeMarkerButton.interactable = true;
                }
            }
            
        }

        public void RemoveMarkerButtonPressed()
        {
            if (_pointerHoveringOverButton)
            {
                Debug.Log("pointer hovering over button. do not run the function.");
            }
            else
            {
                Debug.Log("pointer is Not hovering over button. Run the function.");

                _isRemoveOn = !_isRemoveOn;

                if (_isRemoveOn)
                {
                    Debug.Log("Ready to remove marker.");
                    _removeMarkerButtonText.text = "Stop";
                    placeMarkerButton.interactable = false;
                }
                else
                {
                    Debug.Log("Stopped removing markers.");
                    _removeMarkerButtonText.text = "Remove";
                    placeMarkerButton.interactable = true;
                }
            }
        }
        
        public void OnPointerEnterEvent()
        {
            _pointerHoveringOverButton = true;
        }

        public void OnPointerExitEvent()
        {
            _pointerHoveringOverButton = false;
        }
    }
}
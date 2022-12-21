using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    /**
     * Handles counting the markers found.
     */
    public class SeekerMenu: MonoBehaviour
    {
        [SerializeField] private Text count;
        private int _numFound = 0;
        private int _numHiddenMarkers = 0;
        private string _defaultText;

        private void Start()
        {
            if (!count)
            {
                Debug.Log("count is null");
            }
            _defaultText = count.text;
            count.text = 0 + _defaultText + 0;
        }

        private void OnEnable()
        {
            ARController.OnMarkerFound += IncrementNumMarkersFound;
            ARController.OnMarkerPlaced += SetNumHiddenMarkers;
        }

        private void OnDisable()
        {
            ARController.OnMarkerFound -= IncrementNumMarkersFound;
            ARController.OnMarkerPlaced -= SetNumHiddenMarkers;
        }

        private void IncrementNumMarkersFound()
        {
            Debug.Log("Increment called!");
            _numFound++;
            UpdateDisplay();
        }

        private void SetNumHiddenMarkers(int num)
        {
            _numHiddenMarkers = num;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            count.text = _numFound + " " + _defaultText + " " + _numHiddenMarkers;
        }

        public void Reset()
        {
            _numFound = 0;
            _numHiddenMarkers = 0;
        }
    }
}
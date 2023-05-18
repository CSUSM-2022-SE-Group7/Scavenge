using UnityEngine;


namespace Scavenge
{
    public class PanelTransition : MonoBehaviour
    {
        public GameObject[] panels;
        private int currentPanelIndex;

        void Start()
        {
            currentPanelIndex = 0;
            panels[currentPanelIndex].SetActive(true);
        }

        public void NextPanel()
        {
            panels[0].SetActive(false);
            panels[1].SetActive(true);
        }
    }
}
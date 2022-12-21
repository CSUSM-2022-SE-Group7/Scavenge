using UnityEngine;
using UnityEngine.Serialization;

namespace LocalV2
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject hiderMenu;
        [SerializeField] private GameObject seekerMenu;

        public void ShowMainMenu()
        {
            mainMenu.SetActive(true);
            hiderMenu.SetActive(false);
            seekerMenu.SetActive(false);
        }
        public void ShowHiderMenu()
        {
            Debug.Log("showing hider menu");
            mainMenu.SetActive(false);
            hiderMenu.SetActive(true);
            seekerMenu.SetActive(false);
        }
        public void ShowSeekerMenu()
        {
            mainMenu.SetActive(false);
            hiderMenu.SetActive(false);
            seekerMenu.SetActive(true);
        }
    }
}
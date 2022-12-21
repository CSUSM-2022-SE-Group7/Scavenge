using DefaultNamespace;
using LocalVersion;
using UnityEditor;
using UnityEngine;

namespace LocalV2
{
    public class MainMenuState : IState
    {
        private readonly IMasterController _controller;
        public string Name { get; }

        public static readonly string MAIN_MENU = "MAIN_MENU";

        public MainMenuState(IMasterController controller)
        {
            _controller = controller;
            Name = MAIN_MENU;
        }
        public void EnterState()
        {
            Debug.Log("Entered: Main Menu State");
            _controller.HideAllMarkers();
            _controller.HidePlacementIndicator();
            _controller.CanPlaceMarkers(false);
            _controller.CanRemoveMarkers(false);
            _controller.ShowMainMenu();
        }
        public void Execute() { }
        public void ExitState() { }

    }
}
using UnityEngine;

namespace LocalV2
{
    public class HiderState : IState
    {
        private readonly IMasterController _controller;
        public string Name { get; }
        public static readonly string HIDER = "HIDER";

        public HiderState(IMasterController controller)
        {
            _controller = controller;
            Name = HIDER;
        }

        public void EnterState()
        {
            Debug.Log("Current State: Hider");
            _controller.ShowAllMarkers();
            _controller.CanPlaceMarkers(false);
            _controller.CanRemoveMarkers(false);
            _controller.ShowPlacementIndicator();
        }

        public void Execute() { }

        public void ExitState() { }

    }
}
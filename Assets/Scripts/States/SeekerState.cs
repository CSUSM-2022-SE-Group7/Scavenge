using UnityEngine;

namespace LocalV2
{
    public class SeekerState : IState
    {
        private readonly IMasterController _controller;
        public string Name { get; }
        public static readonly string SEEKER = "SEEKER";

        public SeekerState(IMasterController controller)
        {
            _controller = controller;
            Name = SEEKER;
        }
        public void EnterState()
        {
            Debug.Log("Current State: Seeker");
            _controller.HideAllMarkers();
            _controller.CanRemoveMarkers(true);
            _controller.CanPlaceMarkers(false);
        }

        public void Execute() { }
        public void ExitState() { }

    }
}
namespace LocalV2
{
    public interface IMasterController
    {
        void HidePlacementIndicator();
        void ShowPlacementIndicator();
        void CanPlaceMarkers(bool b);
        void CanRemoveMarkers(bool b);
        void ShowAllMarkers();
        void HideAllMarkers();
        void ShowMainMenu();
        void StartNewHuntButtonPressed();
        void JoinHuntButtonPressed();
    }
}
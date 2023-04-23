public sealed class NonActiveMerdHungryHint : FishFeedHint
{
    private MerdCameraController cameraController;
    private FishSystemScript[] fishSystems;
    private FishSystemScript selectedFishSystem;
    protected override void Start()
    {
        base.Start();
        cameraController = FindObjectOfType<MerdCameraController>();
        fishSystems = FindObjectsOfType<FishSystemScript>();
        cameraController.SelectedFishSystemChanged
            .AddListener(FishSystemChanged);
    }

    protected override bool ShouldTrigger()
    {
        foreach (var fishSystem in fishSystems)
        {
            if (fishSystem == selectedFishSystem)
                continue;

            if (fishSystem.IsHungry())
            {
                return true;
            }
        }

        return false;
    }

    private void FishSystemChanged(FishSystemScript fishSystemScript)
    {
        selectedFishSystem = fishSystemScript;
    }
}

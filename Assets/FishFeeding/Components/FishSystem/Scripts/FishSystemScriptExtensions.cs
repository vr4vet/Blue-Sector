using FeedingIntensity = FishSystemScript.FeedingIntensity;
using FishState = FishSystemScript.FishState;

internal static class FishSystemScriptExtensions
{
    public static bool IsHungry(this FishSystemScript fishSystem)
        => fishSystem.feedingIntensity == FeedingIntensity.Low
        && fishSystem.state == FishState.Hungry;
}

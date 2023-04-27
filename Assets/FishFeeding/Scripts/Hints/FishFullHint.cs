using FeedingIntensity = FishSystemScript.FeedingIntensity;
using FishState = FishSystemScript.FishState;

public sealed class FishFullHint : FishFeedHint
{
    protected override bool ShouldTrigger()
    => FishSystem.feedingIntensity == FeedingIntensity.High
        && FishSystem.state == FishState.Full;
}

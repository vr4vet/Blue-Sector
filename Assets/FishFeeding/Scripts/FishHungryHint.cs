public sealed class FishHungryHint : FishFeedHint
{
    protected override bool ShouldTrigger()
    => FishSystem.IsHungry();
}

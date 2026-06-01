namespace Gardener.GardenerCode.Systems;


public interface IShouldConsumeNutrient
{
    bool ShouldConsumeNutrient();
    Task OnNutrientConsumeBlocked();
}

public interface IOnNutrientConsume
{
    Task OnNutrientConsume();
}


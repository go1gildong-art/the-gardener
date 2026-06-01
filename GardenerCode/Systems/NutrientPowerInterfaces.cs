using MegaCrit.Sts2.Core.Entities.Creatures;
namespace Gardener.GardenerCode.Systems;



public interface IShouldConsumeNutrient
{
    bool ShouldConsumeNutrient(Creature creature);
    Task OnNutrientConsumeBlocked();
}

public interface IOnNutrientConsume
{
    Task OnNutrientConsume();
}


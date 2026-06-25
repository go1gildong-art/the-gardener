namespace Gardener.GardenerCode.Cards;

using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Extensions;
using Gardener.GardenerCode.Systems;
using Gardener.GardenerCode.Systems.Nutrient;
using MegaCrit.Sts2.Core.Entities.Cards;

public abstract class NutrientCard : GardenerCard
{
    public NutrientCard
        (int cost, CardType type, CardRarity rarity, TargetType target, int baseNutrient)
        : base(cost, type, rarity, target)

    { NutrientModifier.AddTo(this, baseNutrient); }

    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public void IncreaseNutrient(int amount) { NutrientModifier.GetFrom(this)?.Increase(amount); }
}
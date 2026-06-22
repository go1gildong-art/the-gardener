using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

[Pool(typeof(GardenerCardPool))]
public class PyrrhicBlossom : GardenerCard
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public PyrrhicBlossom() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 10);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
    new IntVar("Multiplier", 2),
        new IntVar("Nutrient", Nutrient),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var thorns = base.Owner.Creature.GetPowerAmount<ThornsPower>();
        var mult = base.DynamicVars["Multiplier"].BaseValue;

        for (int i = 0; i < (mult - 1); i++)
        {
            await PowerCmd.Apply<ThornsPower>(
                choiceContext,
                base.Owner.Creature,
                thorns,
                base.Owner.Creature, this);
        }

        }

    protected override void OnUpgrade()
    {
        NutrientModifier.GetFrom(this)?.Increase(10);
    }
}

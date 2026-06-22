using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;

[Pool(typeof(GardenerCardPool))]
public class Vitality : GardenerCard
{
    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;

    public Vitality() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        NutrientModifier.AddTo(this, 8);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", Nutrient),
        new EnergyVar(2),
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
    {
        base.EnergyHoverTip
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
        }

    protected override void OnUpgrade()
    {
        NutrientModifier.GetFrom(this)?.Increase(3);
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}
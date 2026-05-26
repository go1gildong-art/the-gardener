using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using MegaCrit.Sts2.Core.Models.CardPools;
using Gardener.GardenerCode.Systems;

[Pool(typeof(GardenerCardPool))]
public class Vitality() : GardenerCode.Cards.GardenerCard(
  0,
  CardType.Skill,
  CardRarity.Common,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
{
    new EnergyVar(2),
  new IntVar("Nutrient", 3),
};

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
        base.EnergyHoverTip
        };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
        await GardenerCmd.ConsumeNutrient(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
        DynamicVars["Nutrient"].UpgradeValueBy(2);
    }
}
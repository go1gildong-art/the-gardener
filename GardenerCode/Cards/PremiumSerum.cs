using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;

[Pool(typeof(GardenerCardPool))]
public class PremiumSerum() : GardenerCode.Cards.GardenerCard(
  0,
  CardType.Skill,
  CardRarity.Rare,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", 8),
        new EnergyVar(1),
        new IntVar("Draw", 2),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
        await PlayerCmd.DrawCards(base.Owner, (int)DynamicVars["Draw"].BaseValue);
        await GardenerCmd.ConsumeNutrient(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
        DynamicVars["Draw"].UpgradeValueBy(1);
        DynamicVars["Nutrient"].UpgradeValueBy(3);
    }
}

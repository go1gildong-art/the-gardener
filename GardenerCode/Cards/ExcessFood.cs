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
public class ExcessFood() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Skill,
  CardRarity.Common,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new IntVar("Nutrient", 6),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await GardenerCmd.ConsumeNutrient(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Nutrient"].UpgradeValueBy(2);
    }
}

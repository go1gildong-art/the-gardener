using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;

[Pool(typeof(GardenerCardPool))]
public class RotIntoFood() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Power,
  CardRarity.Rare,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new PowerVar<RotIntoFoodPower>(1),
        new PowerVar<RotIntoFoodThornsPower>(0)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        await PowerCmd.Apply<RotIntoFoodPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars["RotIntoFoodThornsPower"].BaseValue,
            base.Owner.Creature, this);

            await PowerCmd.Apply<RotIntoFoodThornsPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars["RotIntoFoodThornsPower"].BaseValue,
            base.Owner.Creature, this);

    }

    protected override void OnUpgrade()
    {
        DynamicVars["RotIntoFoodThornsPower"].UpgradeValueBy(1);
    }
}

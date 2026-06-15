using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Extensions;
using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Saves.Runs;
using static MegaCrit.Sts2.Core.Runs.RunState;

[Pool(typeof(GardenerCardPool))]
public class YggdrasilForm() : GardenerCard(
  3,
  CardType.Power,
  CardRarity.Rare,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new PowerVar<YggdrasilFormPower>(2)
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        await PowerCmd.Apply<YggdrasilFormPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars["YggdrasilFormPower"].BaseValue,
            base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["YggdrasilFormPower"].UpgradeValueBy(1);
    }
}

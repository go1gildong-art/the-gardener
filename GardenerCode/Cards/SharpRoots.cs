using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using MegaCrit.Sts2.Core.Models.Powers;

[Pool(typeof(GardenerCardPool))]
public class SharpRoots() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Attack,
  CardRarity.Common,
  TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new DamageVar(4m, DamageProps.card),
        new PowerVar<TemporaryThornsPower>(3m),
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        await PowerCmd.Apply<TemporaryThornsPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars["TemporaryThornsPower"].BaseValue,
            base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
        DynamicVars.Power<TemporaryThornsPower>().UpgradeValueBy(3m);
    }
}


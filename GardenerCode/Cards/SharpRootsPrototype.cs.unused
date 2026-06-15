using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Extensions;
using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Models.Powers;



// [Pool(typeof(GardenerCardPool))]
public class SharpRootsPrototype() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Attack,
  CardRarity.Uncommon,
  TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
{ new DamageVar(6m, DamageProps.card),
new PowerVar<SharpRootsPower>(4m) };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
        .WithHitFx("vfx/vfx_attack_slash")
        .Execute(choiceContext);

        await PowerCmd.Apply<SharpRootsPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars["SharpRootsPower"].BaseValue,
            base.Owner.Creature, this);

        await PowerCmd.Apply<TemporaryThornsPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars["SharpRootsPower"].BaseValue,
            base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1m);
        DynamicVars.Power<SharpRootsPower>().UpgradeValueBy(3m);
    }
}

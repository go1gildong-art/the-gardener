using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using MegaCrit.Sts2.Core.Models.CardPools;

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;
/// <summary>
[Pool(typeof(GardenerCardPool))]
public class ThornBarrage() : GardenerCode.Cards.GardenerCard(
  1,
  CardType.Attack,
  CardRarity.Uncommon,
  TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new RepeatVar(2),
        new CalculationBaseVar(3m),
		new ExtraDamageVar(1m),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) =>
		{
			int num = card.Owner.Creature.GetPowerAmount<ThornsPower>();
			return num;
		})
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        var dmg = DynamicVars.CalculatedDamage.Calculate(cardPlay.Target);
        await DamageCmd.Attack(dmg).FromCard(this).Targeting(cardPlay.Target)
            .WithHitCount((int) DynamicVars.Repeat.BaseValue)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(1m);
    }
}

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Powers;
using MegaCrit.Sts2.Core.Models.CardPools;

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;

[Pool(typeof(GardenerCardPool))]
public class ThornClumping() : GardenerCard(
  1,
  CardType.Skill,
  CardRarity.Uncommon,
  TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
    {
        new RepeatVar(2),
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
		new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) =>
		{
			int num = card.Owner.Creature.GetPowerAmount<ThornsPower>();
			return num;
		})
    };

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var blockAmount = base.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target);
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        
        for (int i = 0; i < base.DynamicVars.Repeat.BaseValue; i++)
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, blockAmount, base.DynamicVars.CalculatedBlock.Props, cardPlay);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(2);
    }
}

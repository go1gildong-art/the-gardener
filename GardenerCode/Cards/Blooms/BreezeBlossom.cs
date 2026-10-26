using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards.Blooms;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

[Pool(typeof(TokenCardPool))]
public class BreezeBlossom() : NutrientCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, 6), IBloomCard
{
	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Defend };

	protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]{
        new CardsVar(3),
		new PowerVar<DrawCardsNextTurnPower>(1)
	};

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
		await PowerCmd.Apply<DrawCardsNextTurnPower>(
                choiceContext,
                base.Owner.Creature,
                DynamicVars["DrawCardsNextTurnPower"].BaseValue,
                base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars["DrawCardsNextTurnPower"].UpgradeValueBy(1);
        base.DynamicVars.Cards.UpgradeValueBy(1);
        IncreaseNutrient(3);
	}
}

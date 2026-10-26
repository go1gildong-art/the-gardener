using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gardener.GardenerCode.Cards.Blooms;

using BaseLib.Utils;
using Gardener.GardenerCode.Character;
using Gardener.GardenerCode.Systems;
using Gardener.GardenerCode.Powers;
using MegaCrit.Sts2.Core.Models.CardPools;

  
[Pool(typeof(TokenCardPool))]
public class BreezeFruit() : NutrientCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self, 4), IBloomCard
{
	public override bool GainsBlock => true;

	protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Defend };

	protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]{
        new CardsVar(1)
	};

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PowerCmd.Apply<BreezeFruitPower>(
            choiceContext,
            base.Owner.Creature,
            base.DynamicVars.Cards.BaseValue,
            base.Owner.Creature, this);
	}

	protected override void OnUpgrade()
	{
        base.DynamicVars.Cards.UpgradeValueBy(1);
        IncreaseNutrient(2);
	}
}
